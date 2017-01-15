using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapHive.Server.Core.DAL.Interface;

namespace MapHive.Server.Core.DataModel
{
    public partial class Application
    {

        /// <summary>
        /// Gets urls of the apps visible for current user
        /// </summary>
        /// <param name="dbCtx"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<string>> GetUserAppsUrlsAsync<T>(T dbCtx, Guid? userId)
            where T: DbContext, IMapHiveApps
        {
            return (await GetUserAppsAsync(dbCtx, userId)).Aggregate(new List<string>(), (agg, app) =>
            {
                var urls = app.Urls.Split('|');

                foreach (var url in urls)
                {
                    var uri = new Uri(url);
                    if (!agg.Contains(uri.Host))
                    {
                        agg.Add(uri.Host);
                    }
                }
                return agg;
            });
        }
    }
}
