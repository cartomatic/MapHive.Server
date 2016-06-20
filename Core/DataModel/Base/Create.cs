using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapHive.Server.Core.DataModel
{

    public abstract partial class Base
    {
        /// <summary>
        /// Creates an object; returns a created object or null if it was not possible to create it due to the fact a uuid is already reserved
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbCtx"></param>
        /// <returns></returns>
        protected internal virtual async Task<T> Create<T>(DbContext dbCtx) where T : Base
        {
            var dbSet = dbCtx.Set<T>();

            this.Validate();


            //new object so do generate uuid but only if different than default. otherwise guid has been set by other party
            if (Uuid != default(Guid))
                Uuid = Guid.NewGuid();

            //test if an object exists; needed in a case the uuid has already been reserved for the collection
            if (await ObjectExists(dbSet, this.Uuid))
                return null;


            //make ef aware of a new object
            dbSet.Add((T)this);

            //and save the object
            await dbCtx.SaveChangesAsync();

            //along with links
            await this.SaveLinks(dbCtx);

            return (T)this;
        }
    }
}
