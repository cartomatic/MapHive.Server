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
    public class MapHiveBasicUserConfiguration<TDbCtx> : IUserConfiguration
        where TDbCtx : IMapHiveUser<MapHiveUserBase>, new()
    {
        private IEnumerable<string> AppShortNames { get; set; }

        /// <summary>
        /// Reads basic user configuration at the MapHiveMeta level
        /// </summary>
        public MapHiveBasicUserConfiguration()
        {
        }

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

            return output;
        }
    }
}
