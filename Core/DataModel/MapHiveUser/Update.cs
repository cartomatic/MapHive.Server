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
        /// updates a maphive user and his org if required
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbCtx"></param>
        /// <param name="uuid"></param>
        /// <returns></returns>
        protected internal override async Task<T> UpdateAsync<T>(DbContext dbCtx, Guid uuid)
        {
            //before updating, get org!
            //this is because the slug may have changed and it is necessary to grab an org before the update as otherwise it would not be possible.
            var org = await GetUserOrganisationAsync(dbCtx);

            var user = base.UpdateAsync<T>(dbCtx, uuid) as MapHiveUser;

            if (org != null && org.Slug != user.Slug)
            {
                await user.UpdateUserOrganisationAsync(dbCtx, org);
            }

            return (T)(Base)user;
        }
    }
}
