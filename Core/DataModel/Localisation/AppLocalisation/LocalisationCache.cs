using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapHive.Server.Core.DAL.Interface;

namespace MapHive.Server.Core.DataModel
{
    public static partial class AppLocalisation
    {
        /// <summary>
        /// App localisations records cache. invalidated on app localisation create, update, destroy
        /// </summary>
        private static Dictionary<string, IEnumerable<LocalisationClass>> AppLocalisationsCache { get; set; } = new Dictionary<string, IEnumerable<LocalisationClass>>();


        /// <summary>
        /// Client localisations cache - cache of the localisations prepared for the client output
        /// </summary>
        private static Dictionary<string, object> ClientLocalisationsCache { get; set; } = new Dictionary<string, object>();
        //Q: how will this behave? Will the static live in app domain, so will be shared even when there are multiple worker processes?

        /// <summary>
        /// Caches guids => class names of the localisation classes
        /// </summary>
        private static Dictionary<Guid, string> LocalisationClassClassNamesCache { get; set; } = new Dictionary<Guid, string>();

        /// <summary>
        /// Caches guids => app names of the localisation classes
        /// </summary>
        private static Dictionary<Guid, string> LocalisationClassAppNamesCache { get; set; } = new Dictionary<Guid, string>();

        /// <summary>
        /// Caches guids => fully qualified class names of the localisation classes
        /// </summary>
        private static Dictionary<Guid, string> LocalisationClassFullClassNamesCache { get; set; } = new Dictionary<Guid, string>();

        /// <summary>
        /// Ivalidates app localisations cache
        /// </summary>
        /// <param name="appName"></param>
        public static void InvalidateAppLocalisationsCache(string appName)
        {
            InvalidateAppLocalisationsCache(appName, string.Empty);
        }

        /// <summary>
        /// Ivalidates app localisations cache
        /// </summary>
        /// <param name="appName"></param>
        public static void InvalidateAppLocalisationsCache(string appName, string className)
        {
            //app localisations cache first

            if (AppLocalisationsCache.ContainsKey(appName) && string.IsNullOrEmpty(className))
            {
                AppLocalisationsCache.Remove(appName);
            }
            else if (AppLocalisationsCache.ContainsKey(appName))
            {
                AppLocalisationsCache.Remove(appName);

                //FIXME - this requires better reading - need to read from db to 'top up' the cache

                //var localisations = AppLocalisationsCache[appName].ToList();

                //var toBeRemoved = localisations.FirstOrDefault(l => l.ClassName == className);
                //if(toBeRemoved != null)
                //    localisations.Remove(toBeRemoved);

                //AppLocalisationsCache[appName] = localisations;
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
        public static async Task<string> GetLocalisationClassClassNameAsync<T>(T dbCtx, Guid localisationClassIdentifier)
            where T : DbContext
        {
            if (LocalisationClassClassNamesCache.ContainsKey(localisationClassIdentifier))
            {
                return LocalisationClassClassNamesCache[localisationClassIdentifier];
            }

            var localisedDbCtx = (ILocalised)dbCtx;

            var className =
                (await localisedDbCtx.LocalisationClasses.FirstOrDefaultAsync(lc => lc.Uuid == localisationClassIdentifier))?
                    .ClassName;

            //cache the output
            if(!string.IsNullOrEmpty(className))
                LocalisationClassClassNamesCache[localisationClassIdentifier] = className;

            return className;
        }

        /// <summary>
        /// Gets a LocalisationClass ClassName by the object uuid
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbCtx"></param>
        /// <param name="localisationClassIdentifier"></param>
        /// <returns></returns>
        public static async Task<string> GetLocalisationClassAppNameAsync<T>(T dbCtx, Guid localisationClassIdentifier)
            where T : DbContext
        {
            if (LocalisationClassAppNamesCache.ContainsKey(localisationClassIdentifier))
            {
                return LocalisationClassAppNamesCache[localisationClassIdentifier];
            }

            var localisedDbCtx = (ILocalised)dbCtx;

            var appName =
                (await localisedDbCtx.LocalisationClasses.FirstOrDefaultAsync(lc => lc.Uuid == localisationClassIdentifier))?
                .ApplicationName;

            //cache the output
            if (!string.IsNullOrEmpty(appName))
                LocalisationClassAppNamesCache[localisationClassIdentifier] = appName;

            return appName;
        }


        /// <summary>
        /// Gets a LocalisationClass full ClassName by the object uuid
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbCtx"></param>
        /// <param name="localisationClassIdentifier"></param>
        /// <returns></returns>
        public static async Task<string> GetLocalisationClassFullClassNameAsync<T>(T dbCtx, Guid localisationClassIdentifier)
            where T : DbContext
        {
            if (LocalisationClassFullClassNamesCache.ContainsKey(localisationClassIdentifier))
            {
                return LocalisationClassFullClassNamesCache[localisationClassIdentifier];
            }

            var localisedDbCtx = (ILocalised)dbCtx;
            var localisationClass = await localisedDbCtx.LocalisationClasses.FirstOrDefaultAsync(
                lc => lc.Uuid == localisationClassIdentifier);
            var fullClassName = $"{localisationClass?.ApplicationName}.{localisationClass?.ClassName}";

            //cache the output
            if (!string.IsNullOrEmpty(fullClassName))
                LocalisationClassFullClassNamesCache[localisationClassIdentifier] = fullClassName;

            return fullClassName;
        }
    }
}
