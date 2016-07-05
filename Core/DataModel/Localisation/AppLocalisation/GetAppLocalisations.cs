using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapHive.Server.Core.DataModel.Interface;
using MapHive.Server.Core.DAL.Interface;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

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
        /// Ivalidates app localisations cache
        /// </summary>
        /// <param name="appName"></param>
        private static void InvalidateAppLocalisationsCache(string appName)
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
        private static string GetClientLocalisationsCacheKey(IEnumerable<string> langCodes, IEnumerable<string> appNames)
        {
            return $"{string.Join("_", appNames.OrderBy(s => s))}___{string.Join("_", langCodes.OrderBy(s => s))}";
        }

        /// <summary>
        /// Gets translations for the specified apps
        /// </summary>
        /// <typeparam name="TDbCtx"></typeparam>
        /// <param name="dbCtx"></param>
        /// <param name="langCode"></param>
        /// <param name="appNames"></param>
        /// <returns></returns>
        public static async Task<Dictionary<string, Dictionary<string, Dictionary<string, string>>>> GetAppLocalisations<TDbCtx>(TDbCtx dbCtx, string langCode, params string[] appNames)
            where TDbCtx : DbContext, ILocalised
        {
            return await GetAppLocalisations(dbCtx, new[] {langCode}, appNames);
        }

        /// <summary>
        /// Gets localisations for specified lang codes and apps
        /// </summary>
        /// <typeparam name="TDbCtx"></typeparam>
        /// <param name="dbCtx"></param>
        /// <param name="langCodes"></param>
        /// <param name="appNames"></param>
        /// <returns></returns>
        public static async Task<Dictionary<string, Dictionary<string, Dictionary<string, string>>>> GetAppLocalisations<TDbCtx>(TDbCtx dbCtx, IEnumerable<string> langCodes, IEnumerable<string> appNames)
            where TDbCtx : DbContext, ILocalised
        {
            var ret = new Dictionary<string, Dictionary<string, Dictionary<string, string>>>();

            if (langCodes == null || !langCodes.Any() || appNames == null || !appNames.Any())
                return ret;

            var defaultLang = await Lang.GetDefaultLang(dbCtx);


            //see if there is cache for the current combination already
            var cacheKey = GetClientLocalisationsCacheKey(langCodes, appNames);
            if (ClientLocalisationsCache.ContainsKey(cacheKey))
            {
                return (Dictionary<string, Dictionary<string, Dictionary<string, string>>>)ClientLocalisationsCache[cacheKey];
            }

            //fetch localisations if needed
            foreach (var appName in appNames)
            {
                if (!AppLocalisationsCache.ContainsKey(appName) || AppLocalisationsCache[appName] == null)
                {
                    AppLocalisationsCache[appName] =
                        await dbCtx.AppLocalisations.Where(al => al.ApplicationName == appName).ToListAsync();
                }

                var appLocalisations = AppLocalisationsCache[appName];

                foreach (var appL in appLocalisations)
                {
                    var key = $"{appL.ApplicationName}.{appL.ClassName}";
                    if (!ret.ContainsKey(key))
                    {
                        ret[key] = new Dictionary<string, Dictionary<string, string>>();
                    }

                    var classTranslations = ret[key];
                    if (!classTranslations.ContainsKey(appL.TranslationKey))
                    {
                        classTranslations[appL.TranslationKey] = new Dictionary<string, string>();
                    }
                    foreach (var translation in appL.Translations.Where(t => t.Key == defaultLang.LangCode || langCodes.Contains(t.Key)))
                    {
                        classTranslations[appL.TranslationKey].Add(translation.Key, translation.Value);
                    }
                }
            }

            //cahce the output
            ClientLocalisationsCache[cacheKey] = ret;

            return ret;
        }
    }
}
