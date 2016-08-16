using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using MapHive.Server.Core.DataModel;
using MapHive.Server.Core.DataModel.Interface;

namespace MapHive.Server.Core.API
{
    public abstract partial class BaseApiCrudController<T, TDbCtx>
    {
        /// <summary>
        /// Defualt get all action
        /// </summary>
        /// <param name="sort"></param>
        /// <param name="filter"></param>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <param name="db">DbContext to be used; when not provided a default instance of TDbCtx will be used</param>
        /// <returns></returns>
        public virtual async Task<IHttpActionResult> Get(string sort = null, string filter = null, int start = 0, int limit = 25, DbContext db = null)
        {
            return await Read<T>(db ?? _dbCtx, sort, filter, start, limit);
        }

        /// <summary>
        /// Defualt get all action
        /// </summary>
        /// <typeparam name="TDto">Defualt get by id action with automated DTO operation output</typeparam>
        /// <param name="sort"></param>
        /// <param name="filter"></param>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <param name="db">DbContext to be used; when not provided a default instance of TDbCtx will be used</param>
        /// <returns></returns>
        public virtual async Task<IHttpActionResult> Get<TDto>(string sort = null, string filter = null, int start = 0, int limit = 25, DbContext db = null) where TDto : class
        {
            return await Read<TDto>(db ?? _dbCtx, sort, filter, start, limit);
        }

        /// <summary>
        /// Gets alist of objects; performs automated conversion of output into specified DTO
        /// </summary>
        /// <typeparam name="TDto">DTO Type to convert the output into</typeparam>
        /// <param name="db">DbContext to be used; when not provided a default instance of TDbCtx will be used</param>
        /// <param name="sort"></param>
        /// <param name="filter"></param>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        protected virtual async Task<IHttpActionResult> Read<TDto>(DbContext db, string sort = null, string filter = null,
            int start = 0, int limit = 25) where TDto : class
        {
            //all stuff is instance based, so need to obtain one first
            T obj = (T)Activator.CreateInstance(typeof(T));

            try
            {
                var filters = filter.ExtJsJsonFiltersToReadFilters();

                //this is web service read call so read as no tracking - detached 
                var data = await obj.Read(db, sort.ExtJsJsonSortersToReadSorters(), filters, start, limit, detached: true);

                if (data.Any())
                {
                    //got the data, so can get the count too.
                    AppendTotalHeader(await obj.ReadCount(db, filters));

                    //Note: this could and should be done in a more elegant way. but had no smart ideas at a time. will come back to this at some stage...
                    //do dto hocus pocus if needed
                    if (typeof(T) != typeof(TDto))
                    {
                        //Note: IDTO is on the DTO and is implemented on instance obviously. so need one
                        var inst = CrateIDtoInstance<TDto>();

                        return Ok(data.Select(d => inst.ToDto(d)).ToList());
                    }
                    else
                    {
                        return Ok(data);
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return this.HandleException(ex);
            }
        }

        /// <summary>
        /// Defualt get by id action
        /// </summary>
        /// <param name="uuid"></param>
        /// <param name="db">DbContext to be used; when not provided a default instance of TDbCtx will be used</param>
        /// <returns></returns>
        public virtual async Task<IHttpActionResult> Get(Guid uuid, DbContext db = null)
        {
            return await Read<T>(db ?? _dbCtx, uuid);
        }

        /// <summary>
        /// Defualt get by id action with automated DTO operation output
        /// </summary>
        /// <typeparam name="TDto">DTO Type to convert the output into</typeparam>
        /// <param name="uuid"></param>
        /// <param name="db">DbContext to be used; when not provided a default instance of TDbCtx will be used</param>
        /// <returns></returns>
        public virtual async Task<IHttpActionResult> Get<TDto>(Guid uuid, DbContext db = null) where TDto : class
        {
            return await Read<TDto>(db ?? _dbCtx, uuid);
        }

        /// <summary>
        /// Gets an object by id
        /// </summary>
        /// <param name="db">DbContext to be used; when not provided a default instance of TDbCtx will be used</param>
        /// <param name="uuid"></param>
        /// <typeparam name="TDto">Type to convert to; must implement IDTO of DTO</typeparam>
        /// <returns></returns>
        protected virtual async Task<IHttpActionResult> Read<TDto>(DbContext db, Guid uuid) where TDto : class
        {
            //all stuff is instance based, so need to obtain one first
            T obj = (T)Activator.CreateInstance(typeof(T));

            try
            {
                //this is web service read call so read as no tracking - detached 
                var entity = await obj.Read(db, uuid, detached: true);
                if (entity != null)
                {
                    //Note: this could and should be done in a more elegant way. but had no smart ideas at a time. will come back to this at some stage...
                    //do dto hocus pocus if needed
                    if (typeof(T) != typeof(TDto))
                    {
                        //Note: IDTO is on the DTO and is implemented on instance obviously. so need one
                        var inst = CrateIDtoInstance<TDto>();

                        return Ok(inst.ToDto(entity));
                    }
                    else
                    {
                        return Ok(entity);
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return this.HandleException(ex);
            }
        }

        /// <summary>
        /// Reads links for given property
        /// </summary>
        /// <param name="uuid"></param>
        /// <param name="propertySpecifier"></param>
        /// <returns></returns>
        public virtual async Task<IHttpActionResult> ReadLinks(Guid uuid, Expression<Func<T, IEnumerable<Base>>> propertySpecifier)
        {
            return await ReadLinks(_dbCtx, uuid, propertySpecifier);
        }

        /// <summary>
        /// Reads links for given property
        /// </summary>
        /// <param name="db"></param>
        /// <param name="uuid"></param>
        /// <param name="propertySpecifier"></param>
        /// <returns></returns>
        public virtual async Task<IHttpActionResult> ReadLinks(DbContext db, Guid uuid, Expression<Func<T, IEnumerable<Base>>> propertySpecifier)
        {
            //first get an instance of T to call the appropriate methods on it
            var obj = (T)Activator.CreateInstance(typeof(T));

            //read the object from the db
            //this is web service read call so read as no tracking - detached 
            obj = await obj.Read(db, uuid, detached: true);

            if (obj == null)
            {
                return BadRequest();
            }

            //looks like the object has been retrieved, so can load the links for it now
            try
            {
                //read the links for a specific property
                //this is web service read call so read as no tracking - detached 
                await obj.MaterialiseLinksAsDetached(db, propertySpecifier);

                //at this stage should have the links loaded, so can return the content of a property
                var mi = Utils.Reflection.GetPropertyMemberInfoFromExpression(propertySpecifier);

                var property = obj.GetType().GetProperty(mi.Name);

                var data = property.GetValue(obj) as IEnumerable<Base>;

                if (data == null || !data.Any())
                {
                    return NotFound();
                }
                else
                {
                    return Ok(data);
                }

            }
            catch (Exception ex)
            {
                //if something goes wrong just fail
                return InternalServerError();
            }
        }


        /// <summary>
        /// Reads parents of given type
        /// </summary>
        /// <typeparam name="TParent"></typeparam>
        /// <param name="uuid"></param>
        /// <returns></returns>
        public virtual async Task<IHttpActionResult> ReadParents<TParent>(Guid uuid)
            where TParent : Base
        {

            return await ReadParents<TParent>(_dbCtx, uuid);
        }

        /// <summary>
        /// Reads parents of given type
        /// </summary>
        /// <typeparam name="TParent"></typeparam>
        /// <param name="db"></param>
        /// <param name="uuid"></param>
        /// <returns></returns>
        public virtual async Task<IHttpActionResult> ReadParents<TParent>(DbContext db, Guid uuid)
            where TParent : Base
        {
            //first get an instance of T to call the appropriate methods on it
            var obj = (T)Activator.CreateInstance(typeof(T));

            //read the object from the db
            //this is web service read call so read as no tracking - detached 
            obj = await obj.Read(db, uuid, detached: true);

            if (obj == null)
            {
                return BadRequest();
            }

            //looks like the object has been retrieved, so can load the links for it now
            try
            {
                //read the links for a specific property
                //this is web service read call so read as no tracking - detached 
                var data = await obj.GetParents<T, TParent>(db, detached: true);

                if (data == null || !data.Any())
                {
                    return NotFound();
                }
                else
                {
                    return Ok(data);
                }

            }
            catch (Exception ex)
            {
                //if something goes wrong just fail
                return InternalServerError();
            }
        }


        /// <summary>
        /// Reads children of given type
        /// </summary>
        /// <typeparam name="TChild"></typeparam>
        /// <param name="uuid"></param>
        /// <returns></returns>
        public virtual async Task<IHttpActionResult> ReadChildren<TChild>(Guid uuid)
            where TChild : Base
        {

            return await ReadChildren<TChild>(_dbCtx, uuid);
        }

        /// <summary>
        /// Reads children of given type
        /// </summary>
        /// <typeparam name="TChild"></typeparam>
        /// <param name="db"></param>
        /// <param name="uuid"></param>
        /// <returns></returns>
        public virtual async Task<IHttpActionResult> ReadChildren<TChild>(DbContext db, Guid uuid)
            where TChild : Base
        {
            //first get an instance of T to call the appropriate methods on it
            var obj = (T)Activator.CreateInstance(typeof(T));

            //read the object from the db
            //this is web service read call so read as no tracking - detached 
            obj = await obj.Read(db, uuid, detached: true);

            if (obj == null)
            {
                return BadRequest();
            }

            //looks like the object has been retrieved, so can load the links for it now
            try
            {
                //read the links for a specific property
                //this is web service read call so read as no tracking - detached 
                var data = await obj.GetChildren<T, TChild>(db, detached: true);

                if (data == null || !data.Any())
                {
                    return NotFound();
                }
                else
                {
                    return Ok(data);
                }

            }
            catch (Exception ex)
            {
                //if something goes wrong just fail
                return InternalServerError();
            }
        }

        /// <summary>
        /// Reads first child of given type
        /// </summary>
        /// <typeparam name="TChild"></typeparam>
        /// <param name="uuid"></param>
        /// <returns></returns>
        public virtual async Task<IHttpActionResult> ReadFirstChild<TChild>(Guid uuid)
            where TChild : Base
        {

            return await ReadFirstChild<TChild>(_dbCtx, uuid);
        }

        /// <summary>
        /// Reads first child of given type
        /// </summary>
        /// <typeparam name="TChild"></typeparam>
        /// <param name="db"></param>
        /// <param name="uuid"></param>
        /// <returns></returns>
        public virtual async Task<IHttpActionResult> ReadFirstChild<TChild>(DbContext db, Guid uuid)
            where TChild : Base
        {
            //first get an instance of T to call the appropriate methods on it
            var obj = (T)Activator.CreateInstance(typeof(T));

            //read the object from the db
            //this is web service read call so read as no tracking - detached 
            obj = await obj.Read(db, uuid, detached: true);

            if (obj == null)
            {
                return BadRequest();
            }

            //looks like the object has been retrieved, so can load the links for it now
            try
            {
                //read the links for a specific property
                //this is web service read call so read as no tracking - detached 
                var data = await obj.GetFirstChild<T, TChild>(db, detached: true);

                if (data == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(data);
                }

            }
            catch (Exception ex)
            {
                //if something goes wrong just fail
                return InternalServerError();
            }
        }
    }
}
