using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapHive.Server.Core.DAL.Interface;

namespace MapHive.Server.Core.DataModel
{
    public partial class AppLocalisation
    {
        /// <summary>
        /// App localisations records cache. invalidated on app localisation create, update, destroy
        /// </summary>
        private static Dictionary<string, IEnumerable<AppLocalisation>> AppLocalisationsCache { get; set; } = new Dictionary<string, IEnumerable<AppLocalisation>>();


        /// <summary>
        /// Client localisations cache - cahce of the localisations prepared for the client output
        /// </summary>
        private static Dictionary<string, object> ClientLocalisationsCache { get; set; } = new Dictionary<string, object>();
        //Q: how will this behave? Will the static live in app domain, so will be shared even when there are multiple worker processes?

        /// <summary>
        /// Caches guids => class names of the localisation classes
        /// </summary>
        private static Dictionary<Guid, string> LocalisationClassNamesCache { get; set; } = new Dictionary<Guid, string>(); 

        /// <summary>
        /// Ivalidates app localisations cache
        /// </summary>
        /// <param name="appName"></param>
        public static void InvalidateAppLocalisationsCache(string appName)
        {
            if (AppLocalisationsCache.ContainsKey(appName))
            {
                AppLocalisationsCache.Remove(appName);
            }

            //clients cache too...
            var keysToInvalidate = ClientLocalisationsCache.Keys.Where(k => k.IndexOf(appName) > -1).ToList();
            foreach (var key in keysToInvalidate)
            {
                ClientLocalisationsCache.Remove(key);
            }
        }

        /// <summary>
        /// Generates a cache key for the client localisations
        /// </summary>
        /// <param name="langCodes"></param>
        /// <param name="appNames"></param>
        /// <returns></returns>
        public static string GetClientLocalisationsCacheKey(IEnumerable<string> langCodes, IEnumerable<string> appNames)
        {
            return $"{string.Join("_", appNames.OrderBy(s => s))}___{string.Join("_", langCodes.OrderBy(s => s))}";
        }


        /// <summary>
        /// Gets a LocalisationClass ClassName by the object uuid
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbCtx"></param>
        /// <param name="localisationClassIdentifier"></param>
        /// <returns></returns>
        public static async Task<string> GetLocalisationClassNameAsync<T>(T dbCtx, Guid localisationClassIdentifier)
            where T : DbContext
        {
            if (LocalisationClassNamesCache.ContainsKey(localisationClassIdentifier))
            {
                return LocalisationClassNamesCache[localisationClassIdentifier];
            }

            var localisedDbCtx = (ILocalised)dbCtx;

            var className =
                (await localisedDbCtx.LocalisationClasses.FirstOrDefaultAsync(lc => lc.Uuid == localisationClassIdentifier))?
                    .ClassName;

            //cache the output
            if(!string.IsNullOrEmpty(className))
                LocalisationClassNamesCache[localisationClassIdentifier] = className;

            return className;
        }
    }
}
