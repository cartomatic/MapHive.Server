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
        /// Destroys an object; returns destroyed object or null in a case it has not been found
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbCtx"></param>
        /// <param name="uuid"></param>
        /// <returns></returns>
        protected internal virtual async Task<T> Destroy<T>(DbContext dbCtx, Guid uuid) where T : Base
        {
            var dbSet = dbCtx.Set<T>();
            var links = GetLinksDbContext(dbCtx).Links;

            //find the object
            var obj = await dbSet.FindAsync(uuid);

            if (obj == null)
                return null;

            dbSet.Remove(obj);

            links.RemoveRange(links.Where(x => x.ParentUuid == obj.Uuid || x.ChildUuid == obj.Uuid));

            await dbCtx.SaveChangesAsync();

            return obj;
        }
    }
}
