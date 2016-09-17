using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MapHive.Server.Core.DataModel.Interface;
using static MapHive.Server.Core.Utils.Reflection;

namespace MapHive.Server.Core.DataModel
{

    public static partial class BaseCrudExtensions
    {
        /// <summary>
        /// Loads links as objects attached to db context
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="db"></param>
        /// <param name="propertySpecifiers"></param>
        /// <returns></returns>
        public static async Task<T> MaterialiseLinksAsAttachedAsync<T>(this T obj, DbContext db,
            params Expression<Func<T, IEnumerable<Base>>>[] propertySpecifiers)
            where T : Base
        {
            return await obj.MaterialiseLinksAsync(db, propertySpecifiers, false);
        }

        /// <summary>
        /// Loads links as objects detached from db context
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="db"></param>
        /// <param name="propertySpecifiers"></param>
        /// <returns></returns>
        public static async Task<T> MaterialiseLinksAsDetachedAsync<T>(this T obj, DbContext db,
            params Expression<Func<T, IEnumerable<Base>>>[] propertySpecifiers)
            where T : Base
        {
            return await obj.MaterialiseLinksAsync(db, propertySpecifiers, true);
        }

        /// <summary>
        /// Loads linked objects for given property, provided the property is an IEnumerable of Base
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="db"></param>
        /// <param name="detached">Whether or not the loaded objects should be deatched from db context or not</param>
        /// <param name="propertySpecifiers"></param>
        /// <returns></returns>
        private static async Task<T> MaterialiseLinksAsync<T>(this T obj, DbContext db,
            IEnumerable<Expression<Func<T, IEnumerable<Base>>>> propertySpecifiers, bool detached)
            where T : Base
        {
            var list = propertySpecifiers.Select(GetPropertyMemberInfoFromExpression).ToList();

            await obj.MaterialiseLinksAsync(db, list, detached);

            return obj;
        }


        /// <summary>
        /// Adds a link to an object; adds appropriate info to the LinksDiff.Upsert collection
        /// Does not save the data.  In order to materialise modified links, saving object is required.
        /// Saving requires calling Create / Update
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent"></param>
        /// <param name="child"></param>
        /// <param name="sortOrder"></param>
        /// <param name="linkData"></param>
        /// <returns></returns>
        public static TEntity AddLink<TEntity, T>(this TEntity parent, T child, int sortOrder = 0, ILinkData linkData = null)
            where TEntity : Base
            where T : Base
        {
            //grab a link if it exists, so it can be modified, or insert a new one
            var link = parent.Links.Upsert.FirstOrDefault(l => l.ParentUuid == parent.Uuid && l.ChildUuid == child.Uuid);
            if (link == null)
            {
                link = new Link()
                {
                    ParentTypeUuid = parent.TypeUuid,
                    ParentUuid = parent.Uuid,
                    ChildUuid = child.Uuid,
                    ChildTypeUuid = child.TypeUuid
                };
                parent.Links.Upsert.Add(link);
            }

            //update the link data that can change
            link.SetLinkData(linkData);
            link.SortOrder = sortOrder;

            //since link is added also make sure to remove it from destroy diff as otherwise it would be first added to a db and then destroyed
            var destroy = parent.Links.Destroy.FirstOrDefault(d => d == child.Uuid);
            if (destroy != default(Guid))
                parent.Links.Destroy.Remove(destroy);

            return parent;
        }

        /// <summary>
        /// Removes link from an object; adds appropriate info to the LinksDiff.Destroy collection
        /// Does not save the object. In order to materialise modified links, saving object is required.
        /// To save calling Create / Update is required
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="child"></param>
        /// <returns></returns>
        public static TEntity RemoveLink<TEntity, T>(this TEntity obj, T child)
            where TEntity : Base
            where T : Base
        {
            //no point in multiplying the removed uuids. even though it would work just fine
            if (obj.Links.Destroy.All(d => d != child.Uuid))
            {
                obj.Links.Destroy.Add(obj.Uuid);
            }

            //also make sure to remove a link from Upsert collection if it exists there
            var upsert = obj.Links.Upsert.FirstOrDefault(l => l.ChildUuid == child.Uuid);
            if (upsert != null)
                obj.Links.Upsert.Remove(upsert);

            return obj;
        }

        /// <summary>
        /// Materialises links for specified properties - reads links of given type and pupulates the property with the obtained data.
        /// Uses reflection in order to work out what to load where
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="db">DbContext; Must implement ILinksDbContext, so the the links info can be read off a db</param>
        /// <param name="props">IEnumerable Of T where T : Base properties to load the links into</param>
        /// <param name="detached">Whether or not the loaded objects should be deatched from db context or not</param>
        /// <returns></returns>
        private static async Task<T> MaterialiseLinksAsync<T>(this T obj, DbContext db, IEnumerable<MemberInfo> props,
            bool detached = true)
            where T : Base
        {
            var iLinksDb = Base.GetLinksDbContext(db);
            if (iLinksDb == null)
                return obj;


            //First get all the links for the object in question
            //since links are not going to be modified can load them detached!
            var links = iLinksDb.Links.AsNoTracking().Where(x => x.ParentUuid == obj.Uuid)
                .OrderBy(x => x.SortOrder)
                .ToList();

            //Load data for all the required properties
            foreach (var prop in props)
            {
                // Get property name and look it up in the obj instance; if not available skip loading data for it
                var propertyName = prop.Name;
                var property = obj.GetType().GetProperty(propertyName);

                if (property == null)
                    continue;


                //Get type of a linked child object
                var childType = property.PropertyType.GetGenericArguments()[0];

                //Since the linked object must be Base derivatives, there should be a Read method on them 
                //need to grab one for a proper class
                var method = GetReadByUuidsMethodInfo(childType);

                //Read method is generic, so need to construct a generic read with an appropriate generic type
                var generic = method.MakeGenericMethod(childType);

                //Create instance of a linked type
                //This is needed to call an appropriate Read method in it
                var childObj = Activator.CreateInstance(childType);

                //Obtain the child type unique identifier
                var typeUuid = childObj.GetType().GetProperty(nameof(Base.TypeUuid)).GetValue(childObj, null);

                //Read the appropriate objects
                var result = await (dynamic) generic.Invoke(
                    childObj, //invoking the read on the appropriate type
                    new object[] //with the appropriate params
                    {
                        db, //db context
                        links.Where(x => x.ChildTypeUuid == (Guid) typeUuid).Select(x => x.ChildUuid).ToList(),
                        //list of uuids to filter the dataset
                        detached //whether or not returned objects should be detached from db context
                    }
                    );

                //finally set whatever has been read on the property
                property.SetValue(obj, result);
            }

            return obj;
        }


        /// <summary>
        /// Finds an appropriate 'Read' method
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static MethodInfo GetReadByUuidsMethodInfo(Type type)
        {
            MethodInfo retVal;

            do
            {
                retVal = type.GetMethod(
                    "Read",
                    BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static,
                    null,
                    new[]
                    {
                        typeof (DbContext), //db context
                        typeof (IEnumerable<Guid>), //a collection of uuids to filter the data
                        typeof (bool) //whether or not returned objects should be detached from db context
                    },
                    null
                    );

                if (retVal != null)
                    break;

                //hmm... looks like the type does not implement a Read method in question. Need to dig deeper
                type = type.BaseType;

            } while (type != null);

            return retVal;
        }


        /// <summary>
        /// Saves links based on the current LinksDiff
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public static async Task SaveLinksAsync<T>(this T obj, DbContext db)
            where T : Base
        {
            await obj.UpsertLinksAsync(db);
            await obj.DestroyLinksAsync(db);
        }

        /// <summary>
        /// Creates or updates links in the database; uses the diff defined in Links.Uspert
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        private static async Task UpsertLinksAsync<T>(this T obj, DbContext db)
            where T : Base
        {

            var upsert = obj.Links.Upsert;

            // If no links then ignore the op
            if (upsert == null || !upsert.Any())
                return;


            var iLinksDb = Base.GetLinksDbContext(db);
            if (iLinksDb == null)
                return;

            // Get all relationships/links for object
            var links = await iLinksDb.Links.Where(x => x.ParentUuid == obj.Uuid).ToListAsync();

            // Update/add object links
            foreach (var link in upsert)
            {
                //Make sure the link's parent uuid is same as the object's uuid; otherwise it would be a link for a different object
                if (link.ParentUuid != default(Guid) && link.ParentUuid != obj.Uuid)
                    throw new ArgumentException(
                        "It looks like you're trying to save link for a different parent here...");

                //also make sure to silently cut links to self
                if (link.ChildUuid == obj.Uuid && link.ChildTypeUuid == obj.TypeUuid)
                    continue;


                // Get all links of given type uuid 
                var sameTypeChildren = links.Where(x => x.ChildTypeUuid == link.ChildTypeUuid).ToList();

                // Get child object link if exist
                var child = sameTypeChildren.FirstOrDefault(x => x.ChildUuid == link.ChildUuid) ??
                            //when digging out a link from local context, make sure to also include parent uuid in the search, otherwise, when one object is being added
                            //as a ling to many parents, we can have problems overwriting wring links here
                            iLinksDb.Links.Local.FirstOrDefault(x => x.ChildUuid == link.ChildUuid && x.ParentUuid == link.ParentUuid);

                link.ParentTypeUuid = obj.TypeUuid;
                link.ParentUuid = obj.Uuid;


                // If child is null then add else update
                if (child == null)
                {
                    //attach to context
                    db.Entry(link).State = EntityState.Added;
                }
                else
                {
                    //looks like the link does exist in the db, need to make sure it will get updated properly
                    link.Id = child.Id;

                    //copy data over to the link that will get updated
                    db.Entry(child).CurrentValues.SetValues(link);
                }
            }

            await db.SaveChangesAsync();
        }


        /// <summary>
        /// Destroys links; uses the diff defined in Links.Destroy
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        private static async Task DestroyLinksAsync<T>(this T obj, DbContext db)
            where T : Base
        {
            var destory = obj.Links.Destroy;

            //if there is nothing to destroy, just ignore the op
            if (destory == null || !destory.Any())
                return;


            var iLinksDb = Base.GetLinksDbContext(db);
            if (iLinksDb == null)
                return;

            iLinksDb.Links.RemoveRange(
            iLinksDb.Links.Where(x => x.ParentUuid == obj.Uuid && destory.Contains(x.ChildUuid)));

            await db.SaveChangesAsync();
        }

        /// <summary>
        /// Gets parents of specified type for the object in question
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TParent"></typeparam>
        /// <param name="obj"></param>
        /// <param name="db"></param>
        /// <param name="detached">Whether or not the loaded objects should be deatched from db context or not</param>
        /// <returns></returns>
        public static async Task<IEnumerable<TParent>> GetParentsAsync<T, TParent>(this T obj, DbContext db,
            bool detached = true)
            where T : Base
            where TParent : Base
        {
            //init the parent object as need to get the type uuid off it
            var parent = (TParent) Activator.CreateInstance(typeof (TParent));

            var iLinksDb = Base.GetLinksDbContext(db);
            if (iLinksDb == null) return null;


            //Get relationship links from database for this object; make sure to obey the detached param
            var links = (detached ? iLinksDb.Links.AsNoTracking() : iLinksDb.Links)
                .Where(x => x.ParentTypeUuid == parent.TypeUuid && x.ChildUuid == obj.Uuid)
                .OrderBy(x => x.Id)
                //this should order by the actual insertion, so will give an indication which link has been assigned first
                .ToList();

            //at this stage got the ids of parents, so can read them by uuid
            return await parent.ReadAsync<TParent>(db, links.Select(l => l.ParentUuid), detached: detached);
        }

        /// <summary>
        /// Gets a first parent of given type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TParent"></typeparam>
        /// <param name="obj"></param>
        /// <param name="db"></param>
        /// <param name="detached"></param>
        /// <returns></returns>
        public static async Task<TParent> GetFirstParentAsync<T, TParent>(this T obj, DbContext db,
            bool detached = true)
            where T : Base
            where TParent : Base
        {
            //init the child object as need to get the type uuid off it
            var parent = (TParent)Activator.CreateInstance(typeof(TParent));

            var iLinksDb = Base.GetLinksDbContext(db);
            if (iLinksDb == null) return null;


            //Get relationship links from database for this object; make sure to obey the detached param
            var link = (detached ? iLinksDb.Links.AsNoTracking() : iLinksDb.Links)
                .FirstOrDefault(x => x.ParentTypeUuid == parent.TypeUuid && x.ChildUuid == obj.Uuid);

            //at this stage got the ids of parents, so can read them by uuid
            if (link != null)
                return await parent.ReadAsync<TParent>(db, link.ChildUuid, detached: detached);
            else
                return null;
        }

        /// <summary>
        /// Determines if an object has any parents links of given type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TParent"></typeparam>
        /// <param name="obj"></param>
        /// <param name="db"></param>
        /// <param name="detached"></param>
        /// <returns></returns>
        public static async Task<bool> HasParentsAsync<T, TParent>(this T obj, DbContext db, bool detached)
            where T : Base
            where TParent : Base
        {
            var iLinksDb = Base.GetLinksDbContext(db);
            if (iLinksDb == null) return false;

            var parent = (TParent)Activator.CreateInstance(typeof(TParent));

            return await (detached ? iLinksDb.Links.AsNoTracking() : iLinksDb.Links)
                .AnyAsync(x => x.ParentTypeUuid == parent.TypeUuid && x.ChildUuid == obj.Uuid);
        }

        /// <summary>
        /// Reads children of given type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TChild"></typeparam>
        /// <param name="obj"></param>
        /// <param name="db"></param>
        /// <param name="detached"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<TChild>> GetChildrenAsync<T, TChild>(this T obj, DbContext db,
            bool detached = true)
            where T : Base
            where TChild : Base
        {
            //init the child object as need to get the type uuid off it
            var child = (TChild)Activator.CreateInstance(typeof(TChild));

            var iLinksDb = Base.GetLinksDbContext(db);
            if (iLinksDb == null) return null;


            //Get relationship links from database for this object; make sure to obey the detached param
            var links = (detached ? iLinksDb.Links.AsNoTracking() : iLinksDb.Links)
                .Where(x => x.ChildTypeUuid == child.TypeUuid && x.ParentUuid == obj.Uuid)
                .OrderBy(x => x.Id)
                //this should order by the actual insertion, so will give an indication which link has been assigned first
                .ToList();

            //at this stage got the ids of children, so can read them by uuid
            return await child.ReadAsync<TChild>(db, links.Select(l => l.ChildUuid), detached: detached);
        }

        /// <summary>
        /// Gets a first child of given type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TChild"></typeparam>
        /// <param name="obj"></param>
        /// <param name="db"></param>
        /// <param name="detached"></param>
        /// <returns></returns>
        public static async Task<TChild> GetFirstChildAsync<T, TChild>(this T obj, DbContext db,
            bool detached = true)
            where T : Base
            where TChild : Base
        {
            //init the child object as need to get the type uuid off it
            var child = (TChild)Activator.CreateInstance(typeof(TChild));

            var iLinksDb = Base.GetLinksDbContext(db);
            if (iLinksDb == null) return null;


            //Get relationship links from database for this object; make sure to obey the detached param
            var link = (detached ? iLinksDb.Links.AsNoTracking() : iLinksDb.Links)
                .FirstOrDefault(x => x.ChildTypeUuid == child.TypeUuid && x.ParentUuid == obj.Uuid);

            //at this stage got the ids of parents, so can read them by uuid
            if(link != null)
                return await child.ReadAsync<TChild>(db, link.ChildUuid, detached: detached);
            else
                return null;
        }

        /// <summary>
        /// Determines if an object has any child links of given type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TChild"></typeparam>
        /// <param name="obj"></param>
        /// <param name="db"></param>
        /// <param name="detached"></param>
        /// <returns></returns>
        public static async Task<bool> HasChildrenAsync<T, TChild>(this T obj, DbContext db, bool detached)
            where T : Base
            where TChild : Base
        {
            var iLinksDb = Base.GetLinksDbContext(db);
            if (iLinksDb == null) return false;

            var child = (TChild) Activator.CreateInstance(typeof (TChild));

            return await (detached ? iLinksDb.Links.AsNoTracking() : iLinksDb.Links)
                .AnyAsync(x => x.ChildTypeUuid == child.TypeUuid && x.ParentUuid == obj.Uuid);
        }
    }
}
