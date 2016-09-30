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
    public class MapHiveBasicUserConfiguration<TDbCtx, T> : IUserConfiguration
        where TDbCtx : IMapHiveUsers<T>, new()
        where T: MapHiveUserBase
    {

        public async Task<IDictionary<string, object>> Read()
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

            //TODO - when roles, orgs, maybe modules, data sources, etc. are implemented will need them too!
            //Perhaps will need some more dbctx interfaces for this too: something like IMapHiveRoles, IMapHiveOrganisations and so on

            return output;
        }
    }
}
