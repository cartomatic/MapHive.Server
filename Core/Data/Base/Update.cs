using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapHive.Server.Core.Data
{
    public abstract partial class Base
    {
        /// <summary>
        /// Updates an object; returns an updated object or null if the object does not exist
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbCtx"></param>
        /// <param name="uuid"></param>
        /// <returns></returns>
        protected internal virtual async Task<T> Update<T>(DbContext dbCtx, Guid uuid) where T : Base
        {
            var dbSet = dbCtx.Set<T>();

            this.Validate();

            //test if an object exists
            if (!await ObjectExists(dbSet, uuid))
                return null;


            //looks like we're good to go with the update...
            Uuid = uuid;

            dbCtx.Entry(this).State = EntityState.Modified;

            await dbCtx.SaveChangesAsync();

            await this.SaveLinks(dbCtx);

            return (T)this;
        }
    }
}
