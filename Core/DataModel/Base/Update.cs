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
        /// Updates an object; returns an updated object or null if the object does not exist
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbCtx"></param>
        /// <param name="uuid"></param>
        /// <returns></returns>
        protected internal virtual async Task<T> UpdateAsync<T>(DbContext dbCtx, Guid uuid) where T : Base
        {
            var dbSet = dbCtx.Set<T>();

            //reassign guid - it usually comes through rest api, so not always present on the object itself
            Uuid = uuid;

            await this.ValidateAsync(dbCtx);

            //test if an object exists
            if (!await ObjectExistsAsync(dbSet, uuid))
                return null;

            //looks like we're good to go with the update...

            dbCtx.Entry(this).State = EntityState.Modified;

            await dbCtx.SaveChangesAsync();

            await this.SaveLinksAsync(dbCtx);

            return (T)this;
        }
    }
}
