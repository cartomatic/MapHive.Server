using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Relational;
using MapHive.Server.Core.DataModel;

namespace MapHive.Server.Core.DataModel
{
    public partial class MapHiveUser
    {
        /// <summary>
        /// creates an organisation for maphhive user and sets links as expected
        /// </summary>
        /// <param name="dbCtx"></param>
        /// <returns></returns>
        public async Task<Organisation> CreateUserOrganisationAsync<TAccount>(DbContext dbCtx, UserAccountService<TAccount> userAccountService)
            where TAccount : RelationalUserAccount
        {
            if (string.IsNullOrEmpty(Slug))
            {
                throw new Exception("Cannot create a user organisation without a valid user slug.");
            }

            //Note: creating user org in 2 steps so the org slug validation does not complain
            //This is because org always needs a slug and the validation checks if a user has not already reserved it.
            //because this org is only being created now it is not tied up with a user in anyway, so cannot check if its slug is ok at this stage

            //org creation step 1
            var org = await new Organisation
            {
                Slug = Guid.NewGuid().ToString() //fake slug that will get updated in the next step
            }.CreateAsync(dbCtx);

            //tie the org to a user
            UserOrgId = org.Uuid;
            this.AddLink(org);
            this.AddLink(await org.GetRoleOwnerAsync(dbCtx));
            await this.UpdateAsync(dbCtx, userAccountService);

            //step 2 - update org slug; now the validation should not complain
            org.Slug = Slug;
            await org.UpdateAsync(dbCtx);

            return org;
        }

        /// <summary>
        /// updates user organisation
        /// </summary>
        /// <param name="dbCtx"></param>
        /// <param name="org"></param>
        /// <returns></returns>
        public async Task<Organisation> UpdateUserOrganisationAsync(DbContext dbCtx, Organisation org)
        {
            if (string.IsNullOrEmpty(Slug))
            {
                throw new Exception("Cannot create a user organisation without a valid user slug.");
            }

            org.Slug = Slug;
            await org.UpdateAsync(dbCtx);
            return org;
        }

        /// <summary>
        /// gets user's organisation - the org that is a counter part of user profile
        /// </summary>
        /// <param name="dbCtx"></param>
        /// <returns></returns>
        public async Task<Organisation> GetUserOrganisationAsync(DbContext dbCtx)
        {
            if (!UserOrgId.HasValue)
            {
                return null;
            }
            return await dbCtx.Set<Organisation>().FirstOrDefaultAsync(o => o.Uuid == UserOrgId.Value);
        }

        /// <summary>
        /// Gets organisations a user has an access to. If user has an own org, then it is returned at the begining
        /// </summary>
        /// <param name="dbCtx"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<Organisation>> GetUserOrganisationsAsync(DbContext dbCtx, Guid userId)
        {
            var user = await dbCtx.Set<MapHiveUser>().FirstOrDefaultAsync(u => u.Uuid == userId);

            if (user == null)
            {
                throw new InvalidOperationException("Unknown user");
            }

            return await user.GetUserOrganisationsAsync(dbCtx);
        }

        /// <summary>
        /// Gets organisations a user has an access to. If user has an own org, then it is returned at the begining
        /// </summary>
        /// <param name="dbCtx"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Organisation>> GetUserOrganisationsAsync(DbContext dbCtx)
        {
            //users are assigned to orgs
            //make sure the 'user' org is at the very begining

            return (await this.GetParentsAsync<MapHiveUser, Organisation>(dbCtx)).OrderBy(o => o.Slug == Slug);

            //todo: will need to also properly order orgs a user has an owner role, so they seem a bit more important than the other orgs
        }
    }
}
