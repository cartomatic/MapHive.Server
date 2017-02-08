using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public async Task<Organisation> CreateUserOrganisationAsync(DbContext dbCtx)
        {
            if (string.IsNullOrEmpty(Slug))
            {
                throw new Exception("Cannot create a user organisation without a valid user slug.");
            }

            var org = await new Organisation()
            {
                //need only a slug here; this is the link between a user and org and must always be intact
                Slug = Slug
            }.CreateAsync(dbCtx);

            this.AddLink(org);
            this.AddLink(await org.GetRoleOwnerAsync(dbCtx));

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
        /// gets user's organisation
        /// </summary>
        /// <param name="dbCtx"></param>
        /// <returns></returns>
        public async Task<Organisation> GetUserOrganisationAsync(DbContext dbCtx)
        {
            if (IsOrgUser)
            {
                return null;
            }
            return await dbCtx.Set<Organisation>().FirstOrDefaultAsync(o => o.Slug == Slug);
        }
    }
}
