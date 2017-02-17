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
            var outApps = new List<Application>();
            
            //common apps
            //do not return hive apps! they should not be listed in user apps even though they will usually be common apps
            var commonApps = await dbCtx.Set<Application>().Where(a => a.IsCommon && !a.IsHive).ToListAsync();


            outApps.AddRange(commonApps);


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
            
            //make sure user has an access to an org prior to reading org apps!
            //TODO
            

            //get org apps
            if (user != null && org != null)
            {
                var orgApps = await org.GetChildrenAsync<Organisation, Application>(dbCtx);

                //TODO - roles and such. perhaps not every single user will have access to each application... 

                foreach (var app in orgApps)
                {
                    if (!outApps.Exists(a => a.Uuid == app.Uuid))
                    {
                        outApps.Add(app);
                    }
                }
            }
            
            

            //TODO - some app ordering perhaps so it is somewhat senshible.
            //stuff like home & dashboard always at the beginning and such...


            return outApps;
        }
    }
}
