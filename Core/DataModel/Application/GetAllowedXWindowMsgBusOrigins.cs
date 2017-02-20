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
        /// Gets urls of the apps registerd in the system so potential xwindow communication between apps can be safely established;
        /// returns urls of all the apps registered in the system
        /// </summary>
        /// <param name="dbCtx"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<string>> GetAllowedXWindowMsgBusOriginsAsync<T>(T dbCtx)
            where T: DbContext, IMapHiveApps
        {
            //get all the urls.
            //identifiers of apps requiring auth are returned always anyway, so revealing the other urls should not really cause too much trouble.
            //the apps should verify whether or not it is ok to use them in given ctx, so...
            return GetAppUrls(await dbCtx.Applications.ToListAsync());
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
