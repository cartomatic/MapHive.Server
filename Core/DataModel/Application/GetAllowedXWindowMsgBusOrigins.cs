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
        /// Gets urls of the apps visible for current user so the xwindow communication can be safely established;
        /// returns urls of the apps that are visible to a user but also urls of the hive apps (HOST APPS)
        /// </summary>
        /// <param name="dbCtx"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<string>> GetAllowedXWindowMsgBusOriginsAsync<T>(T dbCtx, Guid? userId)
            where T: DbContext, IMapHiveApps
        {
            //first get hives an non-hives
            var hives = GetAppUrls(await dbCtx.Applications.Where(a => a.IsHive).ToListAsync());
            var nonHives = GetAppUrls(await GetUserAppsAsync(dbCtx, userId));

            //and merge them together
            hives.AddRange(nonHives);

            return hives;
        }

        /// <summary>
        /// Returns a list of app urls
        /// </summary>
        /// <param name="apps"></param>
        /// <returns></returns>
        protected internal static List<string> GetAppUrls(IEnumerable<Application> apps)
        {
            return apps.Aggregate(new List<string>(), (agg, app) =>
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
