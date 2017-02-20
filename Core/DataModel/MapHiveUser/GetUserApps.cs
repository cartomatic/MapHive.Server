using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapHive.Server.Core.DataModel
{
    public partial class MapHiveUser
    {
        /// <summary>
        /// Gets apps visible by a user
        /// </summary>
        /// <param name="dbCtx"></param>
        /// <param name="userId"></param>
        /// <param name="orgIdentifier"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<Application>> GetUserAppsAsync(DbContext dbCtx, Guid? userId, string orgIdentifier = null)
        {
            var appCollector = new List<Application>();
            
            //common apps
            //do not return hive apps! they should not be listed in user apps even though they will usually be common apps
            var commonApps = await dbCtx.Set<Application>().Where(a => a.IsCommon && !a.IsHive).ToListAsync();
            appCollector.AddRange(commonApps);
            

            MapHiveUser user = null;
            if (userId.HasValue)
            {
                user = await dbCtx.Set<MapHiveUser>().FirstOrDefaultAsync(u => u.Uuid == userId);
            }

            Organisation org = null;
            if (user != null && !string.IsNullOrEmpty(orgIdentifier))
            {
                org = await dbCtx.Set<Organisation>().FirstOrDefaultAsync(o => o.Slug == orgIdentifier);
            } 
            

            //get org apps - the apps that are not public, but assigned to orgs directly
            if (user != null && org != null)
            {
                var orgApps = await org.GetChildrenAsync<Organisation, Application>(dbCtx);

                foreach (var app in orgApps)
                {
                    if (!appCollector.Exists(a => a.Uuid == app.Uuid))
                    {
                        appCollector.Add(app);
                    }
                }
            }

            var outApps = new List<Application>();

            foreach (var a in appCollector)
            {
                if (
                    a.IsDefault || //always return the dashboard
                    (a.IsCommon && !a.RequiresAuth) || //and the public apps with no auth
                    (org != null && (await org.GetUserAppAccessCredentials(dbCtx, user, a)).CanUseApp)
                )
                    outApps.Add(a);
            }

            //TODO - more ordering - stuff like special apps that are not public, but assigned to orgs directly, etc. Also, maybe some differentiation between freely accessible apps and the apps with auth.

            //stuff like home & dashboard always at the beginning and such...
            return outApps.OrderByDescending(a => a.IsHome).ThenByDescending(a => a.IsDefault).ThenBy(a => a.Name); ;
        }
    }
}
