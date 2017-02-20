using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MapHive.Server.Core.DataModel;
using MapHive.Server.Core.DAL.DbContext;

namespace MapHive.Server.Core.DataModel
{
    
    public partial class Organisation
    {
        /// <summary>
        /// checks if an org can access an application
        /// </summary>
        /// <param name="dbCtx"></param>
        /// <param name="orgId"></param>
        /// <param name="appId"></param>
        /// <returns></returns>
        public static async Task<bool> CanUseApp(MapHiveDbContext dbCtx, Guid orgId, Guid appId)
        {
            var app = await dbCtx.Applications.FirstOrDefaultAsync(a => a.Uuid == appId);
            if (app == null)
                return false;

            //if an ap is common then every organisation can access it
            if (app.IsCommon)
                return true;

            //app is not common, so need to see if an app is assigned to this very org
            return await app.HasParentLinkAsync(dbCtx, orgId);
        }

        /// <summary>
        /// Determines whether or not an organisation has an access to an application
        /// </summary>
        /// <param name="dbCtx"></param>
        /// <param name="app"></param>
        /// <returns></returns>
        public async Task<bool> CanUseApp(DbContext dbCtx, Application app)
        {
            return app.IsCommon || await this.HasChildLinkAsync(dbCtx, app);
        }
    }
}
