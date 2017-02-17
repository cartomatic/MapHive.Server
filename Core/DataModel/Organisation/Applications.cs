using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapHive.Server.Core.DataModel;
using MapHive.Server.Core.DAL.DbContext;

namespace MapHive.Server.Core.DataModel
{
    
    public partial class Organisation
    {
        public static async Task<bool> AllowsApplication(MapHiveDbContext dbCtx, Guid orgId, Guid appId)
        {
            var app = await dbCtx.Applications.FirstOrDefaultAsync(a => a.Uuid == appId);
            if (app == null)
                return false;

            if (app.IsCommon)
                return true;

            //app is not common, so need to see if an app is assigned to this very org
            var org = (await app.GetParentsAsync<Application, Organisation>(dbCtx)).FirstOrDefault(o => o.Uuid == orgId);
            return org != null;

            //TODO - perhaps should also check user context and roles. even though an org can use an app, a user within an org may not be allowed to do so!
        }
    }
}
