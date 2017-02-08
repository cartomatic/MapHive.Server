using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrockAllen.MembershipReboot;
using MapHive.Server.Core.DataModel;

namespace MapHive.Server.Core.DataModel
{
    public partial class MapHiveUser
    {
        /// <summary>
        /// destroys a maphive user object and the user's organisation if any
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TAccount"></typeparam>
        /// <param name="dbCtx"></param>
        /// <param name="userAccountService"></param>
        /// <param name="uuid"></param>
        /// <returns></returns>
        protected internal override async Task<T> DestroyAsync<T, TAccount>(DbContext dbCtx, UserAccountService<TAccount> userAccountService, Guid uuid)
        {
            if (!IsOrgUser)
            {
                var org = await GetUserOrganisationAsync(dbCtx);
                await org.DestroyAsync(dbCtx);
            }

            return await base.DestroyAsync<T, TAccount>(dbCtx, userAccountService, uuid);
        }
    }
}
