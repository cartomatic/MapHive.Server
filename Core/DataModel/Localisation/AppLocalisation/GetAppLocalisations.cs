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
        /// Gets translations for the specified apps
        /// </summary>
        /// <typeparam name="TDbCtx"></typeparam>
        /// <param name="dbCtx"></param>
        /// <param name="langCode"></param>
        /// <param name="appNames"></param>
        /// <returns></returns>
        public static async Task<Dictionary<string, Dictionary<string, Dictionary<string, string>>>> GetAppLocalisationsAsync<TDbCtx>(TDbCtx dbCtx, string langCode, params string[] appNames)
            where TDbCtx : DbContext, ILocalised
        {
            return await GetAppLocalisationsAsync(dbCtx, new[] { langCode }, appNames);
        }

        /// <summary>
        /// Gets localisations for specified lang codes and apps
        /// </summary>
        /// <typeparam name="TDbCtx"></typeparam>
        /// <param name="dbCtx"></param>
        /// <param name="langCodes"></param>
        /// <param name="appNames"></param>
        /// <returns></returns>
        public static async Task<Dictionary<string, Dictionary<string, Dictionary<string, string>>>> GetAppLocalisationsAsync<TDbCtx>(TDbCtx dbCtx, IEnumerable<string> langCodes, IEnumerable<string> appNames)
            where TDbCtx : DbContext, ILocalised
        {
            var ret = new Dictionary<string, Dictionary<string, Dictionary<string, string>>>();

            if (appNames == null || !appNames.Any())
                return ret;

            //grab the default lng
            var defaultLang = await Lang.GetDefaultLangAsync(dbCtx);

            if (langCodes == null || !langCodes.Any())
            {
                if (defaultLang == null)
                {
                    return ret;
                }

                //no langs provided, so the calling client may not be aware of the lang yet.
                //in this scenario just lookup the default lang and get the localisation fot the default lng
                langCodes = new[] {defaultLang.LangCode};
            }


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
                    //read all the Localisation classes for the given AppName
                    var localisationClasses = await
                        dbCtx.LocalisationClasses.Where(lc => lc.ApplicationName == appName).ToListAsync();

                    //now grab the identifiers - need them in order to request translation keys. when using IQueryable, the range MUST BE static, simple types
                    //so even though compiler will not complain if a range is passes as localisationClasses.Select(lc => lc.Uuid) it will fail in the runtime
                    //saving this to a variable solves the issue!
                    var localisationClassesIdentifiers = localisationClasses.Select(lc => lc.Uuid);

                    var translationKeys = await
                        dbCtx.TranslationKeys.Where(
                            tk => localisationClassesIdentifiers.Contains(tk.LocalisationClassUuid)).ToListAsync();

                    AppLocalisationsCache[appName] = localisationClasses.GroupJoin(
                        translationKeys,
                        lc => lc.Uuid,
                        tk => tk.LocalisationClassUuid,
                        (lc, tk) => new LocalisationClass()
                        {
                            ApplicationName = lc.ApplicationName,
                            ClassName = lc.ClassName,
                            InheritedClassName = lc.InheritedClassName,
                            TranslationKeys = tk
                        }
                    );
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


                    foreach (var tk in appL.TranslationKeys)
                    {
                        if (!classTranslations.ContainsKey(tk.Key))
                        {
                            classTranslations[tk.Key] = new Dictionary<string, string>();
                        }

                        foreach (var translation in tk.Translations.Where(t => t.Key == defaultLang.LangCode || langCodes.Contains(t.Key)))
                        {
                            classTranslations[tk.Key].Add(translation.Key, translation.Value);
                        }
                    }
                }
            }

            //cahce the output
            ClientLocalisationsCache[cacheKey] = ret;

            return ret;
        }
    }
}
