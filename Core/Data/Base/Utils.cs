using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace MapHive.Server.Core.Data
{
    public abstract partial class Base
    {
        /// <summary>
        /// Helper method, check if object exist in a database
        /// </summary>
        /// <param name="dbSet"></param>
        /// <param name="uuid"></param>
        /// <returns></returns>
        protected static async Task<bool> ObjectExists<T>(DbSet<T> dbSet, Guid? uuid) where T : Base
        {
            return await dbSet.Where(e => e.Uuid == uuid).CountAsync() > 0;
        }
    }
}
