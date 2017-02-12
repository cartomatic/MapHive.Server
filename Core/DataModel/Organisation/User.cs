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
    }
}
