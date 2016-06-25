using System.CodeDom;
using System.Collections.Generic;
using MapHive.Server.Core.DataModel;
using MapHive.Server.Core.Utils;

namespace MapHive.Server.DataModel.DAL.Migrations.MetadataConfiguration
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MapHiveDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "MapHive.Server.DataModel.DAL.MapHiveDbContext";
            MigrationsDirectory = @"_DAL\Migrations\MetadataConfiguration";
        }

        protected override void Seed(MapHiveDbContext context)
        {
            Identity.ImpersonateGhostUser();

#if DEBUG
            //FIXME - this is dev only; remove at some point
            TestLinkSeed(context);
#endif

            ApplicationsSeed(context);

            //LangsSeed(context);

            //AppLocalisationsSeed(context);
        }

        private void AppLocalisationsSeed(MapHiveDbContext context)
        {

            context.AppLocalisations.AddOrUpdate(
                new AppLocalisation
                {
                    ApplicationName = "mh",
                    ClassName = "some.class.Name",
                    TranslationKey = "someTranslationKey",
                    Translations = new Translations
                    {
                        { "en", "Some EN property" },
                        { "pl", "Some PL property" }
                    }
                },
                new AppLocalisation
                {
                    ApplicationName = "SomeApp",
                    ClassName = "some.class.Name",
                    TranslationKey = "someTranslationKey",
                    Translations = new Translations
                    {
                        { "en", "Some EN property" },
                        { "pl", "Some PL property" }
                    }
                }
            );
        }

        /// <summary>
        /// Default app langs
        /// </summary>
        /// <param name="context"></param>
        private void LangsSeed(MapHiveDbContext context)
        {
            context.Langs.AddOrUpdate(new Lang
            {
                LangCode = "pl",
                Name = "Polski"
            },
            new Lang
            {
                LangCode = "en",
                Name = "English",
                IsDefault = true
            });
        }

        /// <summary>
        /// Test link seeder to verify if all the stuff works...
        /// </summary>
        /// <param name="context"></param>
        private static void TestLinkSeed(MapHiveDbContext context)
        {
            try
            {
                var l = new Link
                {
                    ParentTypeUuid = default(Guid),
                    ChildTypeUuid = default(Guid),
                    ParentUuid = default(Guid),
                    ChildUuid = default(Guid)
                };

                l.LinkData.Add("some_link_data_consumer", new Dictionary<string, object>
                {
                    {"prop1", "Some textual property"},
                    {"prop2", 123},
                    {"prop3", DateTime.Now}
                });

                context.Links.AddOrUpdate(
                    l,
                    new Link
                    {
                        ParentTypeUuid = default(Guid),
                        ChildTypeUuid = default(Guid),
                        ParentUuid = default(Guid),
                        ChildUuid = default(Guid),
                        LinkData = new LinkData
                        {
                            {
                                "will_this_nicely_serialize", new Dictionary<string, object>
                                {
                                    {"prop1", new Application()},
                                    {"prop2", new Link()}
                                }
                            }
                        }
                    }
                );
            }
            catch (Exception ex)
            {
                System.IO.File.WriteAllText(@"f:\err.txt", ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        private static void ApplicationsSeed(MapHiveDbContext context)
        {
            context.Applications.AddOrUpdate(
                new Application
                {
                    Uuid = Guid.Parse("aebe8c25-92a8-49c5-a54e-1b542727c879"),
                    ShortName = "mhhgis",
                    Name = "HGIS v1",
                    Description = "A dev test port of the Cartomatic\'s HGIS; a good example of an external and/or exisiting app inclusion into the system",
                    Url = "https://hgis.maphive.local/",
                    IsCommon = true,
                    IsDefault = true
                },
                new Application
                {
                    Uuid = Guid.Parse("79f03dc6-7591-4011-92f3-09d07aa1b048"),
                    ShortName = "mhmapapp",
                    Name = "MapHive MapApp",
                    Description = "MapHIve Map App",
                    Url = "https://map.maphive.local/",
                    RequiresAuth = true,
                    IsCommon = true
                },
                new Application
                {
                    Uuid = Guid.Parse("9f1f40a6-94df-4e7b-b2b0-b2b14a3b70f6"),
                    Name = "Admin",
                    ShortName = "mhadmin",
                    Description = "MapHive Admin",
                    Url = "https://admin.maphive.local/",
                    UseSplashscreen = true,
                    RequiresAuth = true,
                    IsCommon = true
                },
                new Application
                {
                    Uuid = Guid.Parse("28f28ebd-b2f6-4872-857d-46763e193753"),
                    //no short name, so can test uuid in the url part!
                    Name = "MapHive SiteAdmin",
                    Description = "MapHive platform Admin app",
                    Url = "https://siteadmin.maphive.local/",
                    RequiresAuth = true
                });
        }
    }
}
