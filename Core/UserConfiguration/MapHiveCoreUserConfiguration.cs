using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapHive.Server.Core.DataModel;
using MapHive.Server.Core.DAL.Interface;

namespace MapHive.Server.Core.UserConfiguration
{
    /// <summary>
    /// reads a basic user configuration for a given appllication; returns info on a user and his orgs that have access to the app;
    /// this basic cfg is then used to work out if an app can start for a user. org info is returned to allow for org-rescope if a current org does not have an access to an app but some other orgs do
    /// this is because after user authentiates he is scoped to a declared org (or if not declared, to his own org). When in standalone mode, it is required to let user use an app scoped to an org that can use it.
    /// further user & spp specific logic is executed by apps themselves
    /// </summary>
    /// <typeparam name="TDbCtx"></typeparam>
    /// <typeparam name="T"></typeparam>
    public class MapHiveBasicUserConfiguration<TDbCtx, T> : IUserConfiguration
        where TDbCtx : DbContext, IMapHiveUsers<T>, IMapHiveApps, new()
        where T: MapHiveUser
    {
        public async Task<IDictionary<string, object>> Read(string appUrl)
        {
            var output = new Dictionary<string, object>();

            var dbCtx = new TDbCtx();

            var userUuid = Utils.Identity.GetUserGuid();

            if (!userUuid.HasValue)
            {
                return output;
            }

            var user = await dbCtx.Users.FirstOrDefaultAsync(u => u.Uuid == userUuid.Value);
            output.Add("user", user);


            //work out the app that requested the cfg
            //apps as such do not recognise the app url tokens, so need to use a url to match the app
            //urls are pipe '|' separated strings.
            var app = await dbCtx.Set<Application>().FirstOrDefaultAsync(a => a.Urls.Contains(appUrl));
            if (app == null)
            {
                return output;
            }

            output.Add("app", app);

            
            //need to grab user orgs
            var orgs = await user.GetParentsAsync<MapHiveUser, Organisation>(dbCtx);

            //user app access credentials!
            var credentials = new List<Organisation.OrgUserAppAccessCredentials>();
            
            //and check if user can access the app from an org.
            //this is so when user has an org owner role, org admin role or is assigned to a team that has an access to an app
            foreach (var org in orgs)
            {
                var c = await org.GetUserAppAccessCredentials(dbCtx, user, app);
                if(c.CanUseApp)
                    credentials.Add(c);
            }

            //finally reorder the org access credentials, so user's org is always first
            output.Add(
                "AllowedOrgs",
                credentials.OrderByDescending(c => c.Organisation.Uuid == user.UserOrgId)
                    .ToDictionary(c => c.Organisation.Slug, c => new {c.CanUseApp, c.IsAppAdmin})
            );

            return output;
        }
    }
}
