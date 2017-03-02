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
        /// updates a maphive user and his org if required
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TAccount"></typeparam>
        /// <param name="dbCtx"></param>
        /// <param name="userAccountService"></param>
        /// <param name="uuid"></param>
        /// <returns></returns>
        protected internal override async Task<T> UpdateAsync<T, TAccount>(DbContext dbCtx, UserAccountService<TAccount> userAccountService, Guid uuid)
        {
            //before updating, get org!
            //this is because the slug may have changed and it is necessary to grab an org before the update as otherwise it would not be possible.
            var org = await GetUserOrganisationAsync(dbCtx);
            
            var user = await base.UpdateAsync<T, TAccount>(dbCtx, userAccountService, uuid) as MapHiveUser;

            if (org != null && org.Slug != user.Slug)
            {
                await user.UpdateUserOrganisationAsync(dbCtx, org);
            }

            return (T)(Base)user;
        }
    }
}
