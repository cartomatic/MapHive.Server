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
        /// Gets common apps
        /// </summary>
        /// <param name="dbCtx"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<Application>> GetCommonAppsAsync(DbContext dbCtx)
        {
            return await dbCtx.Set<Application>().Where(a => a.IsCommon).ToListAsync();
        }
    }
}
