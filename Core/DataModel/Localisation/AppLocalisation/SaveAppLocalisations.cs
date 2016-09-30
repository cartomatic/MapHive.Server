using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MapHive.Server.Core.DAL.Interface;

namespace MapHive.Server.Core.DataModel
{
    public static partial class AppLocalisation
    {
        public static async Task SaveLocalisations(DbContext dbCtx, IEnumerable<LocalisationClass> localisations,
            bool? overwrite, IEnumerable<string> langsToImport)
        {

            var localisedDbCtx = (ILocalised) dbCtx;
            var appNames = new List<string>();

            foreach (var incomingLc in localisations)
            {
                if (!appNames.Contains(incomingLc.ApplicationName))
                {
                    appNames.Add(incomingLc.ApplicationName);
                }

                var lc =
                        await localisedDbCtx.LocalisationClasses.FirstOrDefaultAsync(
                            x => x.ApplicationName == incomingLc.ApplicationName && x.ClassName == incomingLc.ClassName);

                if (overwrite == true)
                {
                    if (lc != null)
                    {
                        //this should nicely cleanup the translation keys too
                        await lc.DestroyAsync<LocalisationClass>(dbCtx, lc.Uuid);
                    }

                    lc = new LocalisationClass
                    {
                        ApplicationName = incomingLc.ApplicationName,
                        ClassName = incomingLc.ClassName,
                        InheritedClassName = incomingLc.InheritedClassName
                    };
                    await lc.CreateAsync(dbCtx);

                    foreach (var translationKey in incomingLc.TranslationKeys)
                    {
                        //filter out translations that are about to be saved
                        var translations = new Translations();
                        foreach (var lng in translationKey.Translations.Keys)
                        {
                            if (langsToImport == null || langsToImport.Contains(lng))
                            {
                                translations.Add(lng, translationKey.Translations[lng]);
                            }
                        }

                        var tk = new TranslationKey
                        {
                            LocalisationClassUuid = lc.Uuid,
                            Key = translationKey.Key,
                            Translations = translations ?? new Translations()
                        };
                        await tk.CreateAsync(dbCtx);
                    }
                }
                else
                {
                    //this is a non-overwrite, so it applies to both - localisation and translation keys; whatever is in the db must be maintained
                    //only the missing bits and pieces are added

                    //take care of the translation key - if not there it must be created
                    if (lc == null)
                    {
                        lc = new LocalisationClass
                        {
                            ApplicationName = incomingLc.ApplicationName,
                            ClassName = incomingLc.ClassName,
                            InheritedClassName = incomingLc.InheritedClassName
                        };
                        await lc.CreateAsync(dbCtx);
                    }

                    //review the translations now
                    foreach (var translationKey in incomingLc.TranslationKeys)
                    {
                        //check if the translation key is already there
                        var tk =
                            await
                                localisedDbCtx.TranslationKeys.FirstOrDefaultAsync(
                                    x => x.LocalisationClassUuid == lc.Uuid && x.Key == translationKey.Key);

                        //create key if not there
                        if (tk == null)
                        {
                            tk = new TranslationKey
                            {
                                LocalisationClassUuid = lc.Uuid,
                                Key = translationKey.Key,
                                Translations = new Translations()
                            };
                            await tk.CreateAsync(dbCtx);
                        }

                        //filter out translations that are about to be saved
                        var translations = new Translations();
                        foreach (var lng in translationKey.Translations.Keys)
                        {
                            if (langsToImport == null || langsToImport.Contains(lng))
                            {
                                translations.Add(lng, translationKey.Translations[lng]);
                            }
                        }
                        
                        //check translations
                        foreach (var lng in translations.Keys)
                        {
                            if (!tk.Translations.ContainsKey(lng))
                            {
                                tk.Translations[lng] = translations[lng];
                            }
                        }
                        await tk.UpdateAsync(dbCtx, tk.Uuid);
                    }
                }
            }

            //need to wipe out cache too...
            foreach (var appName in appNames)
            {
                InvalidateAppLocalisationsCache(appName);
            }
        }
    }
}
