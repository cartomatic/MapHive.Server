using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using MapHive.Server.Core.DataModel.Interface;
using MapHive.Server.Core.DAL.Interface;

namespace MapHive.Server.Core.DataModel
{
    public abstract partial class Base
    {
        /// <summary>
        /// Helper method, check if object exist in a database
        /// </summary>
        /// <param name="dbSet"></param>
        /// <param name="uuid"></param>
        /// <returns></returns>
        private static async Task<bool> ObjectExists<T>(DbSet<T> dbSet, Guid? uuid) where T : Base
        {
            return await dbSet.Where(e => e.Uuid == uuid).CountAsync() > 0;
        }

        /// <summary>
        /// Gets a ILinksDbContext; Throws if the db context is not ILinksDbContext
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public static ILinksDbContext<T> GetLinksDbContext<T>(DbContext db)
            where T: class, ILink
        {
            var iLinksDb = db as ILinksDbContext<T>;
            if (iLinksDb == null)
                throw new ArgumentException(
                    $"In order to materialise links DbContext must implement {nameof(ILinksDbContext<T>)}");

            return iLinksDb;
        }
    }
}
