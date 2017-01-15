using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapHive.Server.Core.DataModel
{
    public partial class Application
    {
        /// <summary>
        /// Gets apps visible by a user
        /// </summary>
        /// <param name="dbCtx"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<Application>> GetUserAppsAsync(DbContext dbCtx, Guid? userId)
        {
            //TODO - the actual app filtering for a user
            //TODO - perhaps org uuid should be taken in as well...



            //TODO - always add HIVE!

            return await dbCtx.Set<Application>().ToListAsync();

            //return await dbCtx.Set<Application>().Where(a => a.IsCommon && !a.IsHidden).ToListAsync();
        }
    }
}
