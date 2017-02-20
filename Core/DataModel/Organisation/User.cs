using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapHive.Server.Core.DataModel;

namespace MapHive.Server.Core.DataModel
{
    
    public partial class Organisation
    {
        /// <summary>
        /// Gets a user that is associated with given organisation (org is the user's profile counterpart
        /// </summary>
        /// <param name="dbCtx"></param>
        /// <returns></returns>
        public async Task<MapHiveUser> GetOrganisationUserAsync(DbContext dbCtx)
        {
            return await dbCtx.Set<MapHiveUser>().FirstOrDefaultAsync(u => u.UserOrgId == Uuid);
        }


        /// <summary>
        /// User application access credentials within an organisation
        /// </summary>
        public class OrgUserAppAccessCredentials
        {
            /// <summary>
            /// Organisation that the app access credentials are tested for
            /// </summary>
            public Organisation Organisation { get; set; }


            /// <summary>
            /// Application for which the access credentials are tested within an organisation
            /// </summary>
            public Application Application { get; set; }

            /// <summary>
            /// Whether or not user can use given application within an organisation
            /// </summary>
            public bool CanUseApp { get; set; }

            /// <summary>
            /// Whether or not user has application administration rights within an organisation
            /// </summary>
            public bool IsAppAdmin { get; set; }
        }


        /// <summary>
        /// Returns user application credentials within an organisation
        /// </summary>
        /// <param name="dbCtx"></param>
        /// <param name="user"></param>
        /// <param name="app"></param>
        /// <returns></returns>
        public async Task<OrgUserAppAccessCredentials> GetUserAppAccessCredentials(DbContext dbCtx, MapHiveUser user,
            Application app)
        {
            var output = new OrgUserAppAccessCredentials
            {
                Organisation = this,
                Application = app
            };

            //check if organisation can access and application if not, then good bye
            if (!await CanUseApp(dbCtx, app))
                return output;



            //check if user is an org owner or org admin
            output.IsAppAdmin = await IsOrgOwner(dbCtx, user) || await IsOrgAdmin(dbCtx, user);
            output.CanUseApp = output.IsAppAdmin;


            //user is not granted app admin access via org owner / admin roles, so need to check if user can access the app as a 'normal' user, so via teams
            //note: teams can also grant app admin role!
            if (!output.IsAppAdmin)
            {
                var orgTeams = await this.GetChildrenAsync<Organisation, Team>(dbCtx);
                foreach (var team in orgTeams)
                {
                    //get team's app link
                    var teamAppLink = await team.GetChildLink(dbCtx, app);

                    //make sure team grants access to an app
                    if (teamAppLink == null)
                        continue;


                    //get team's user link
                    var teamUserLink = await team.GetChildLink(dbCtx, user);
                    var userCanUseApp = teamUserLink != null;

                    //if a team grants an access to an app for a user we can test if it also test if it is admin access
                    if (userCanUseApp)
                    {
                        //only apply true assignment here so it does not get reset when searching for app admin credentials!
                        output.CanUseApp = userCanUseApp;

                        //extract app access credentials link data
                        var appAccessCredentials = teamAppLink.LinkData.GetByKey(Team.AppAccessCredentialsLinkDataObject);


                        output.IsAppAdmin = appAccessCredentials != null &&
                                            appAccessCredentials.ContainsKey(Team.AppAdminAccess) &&
                                            (bool) appAccessCredentials[Team.AppAdminAccess];

                    }

                    //no point in testing other teams, as full app access is already granted
                    if (output.IsAppAdmin)
                        break;

                }

            }

            return output;
        }
    }
}
