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
        /// App localisations cache. invalidated on app localisation create, update, destroy
        /// </summary>
        private static Dictionary<string, IEnumerable<AppLocalisation>> AppLocalisations { get; set; } = new Dictionary<string, IEnumerable<AppLocalisation>>();


        //TODO - think about implementing extra cache for lang, app???


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

            //fetch localisations if needed
            foreach (var appName in appNames)
            {
                if (!AppLocalisations.ContainsKey(appName))
                {
                    AppLocalisations[appName] =
                        await dbCtx.AppLocalisations.Where(al => al.ApplicationName == appName).ToListAsync();
                }

                var appLocalisations = AppLocalisations[appName];

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

            return ret;
        }
    }
}
