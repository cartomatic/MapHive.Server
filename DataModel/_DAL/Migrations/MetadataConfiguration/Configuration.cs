using System.CodeDom;
using System.Collections.Generic;
using MapHive.Server.Core.DataModel;
using MapHive.Server.Core.DAL.Interface;
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

            LangsSeed(context);

            AppLocalisationsSeed(context);

            XWindowOriginsSeed(context);
        }

        private void AppLocalisationsSeed(MapHiveDbContext context)
        {

            context.AppLocalisations.AddOrUpdate(
                new AppLocalisation
                {
                    Uuid = Guid.NewGuid(),
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
                    Uuid = Guid.NewGuid(),
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
                Uuid = Guid.NewGuid(),
                LangCode = "pl",
                Name = "Polski"
            },
            new Lang
            {
                Uuid = Guid.NewGuid(),
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
                    Uuid = Guid.NewGuid(),
                    ShortName = "mhhgis",
                    Name = "HGIS v1",
                    Description = "A dev test port of the Cartomatic\'s HGIS; a good example of an external and/or exisiting app inclusion into the system",
                    Url = "https://hgis.maphive.local/",
                    IsCommon = true,
                    IsDefault = true
                },
                new Application
                {
                    Uuid = Guid.NewGuid(),
                    ShortName = "mhmapapp",
                    Name = "MapHive MapApp",
                    Description = "MapHIve Map App",
                    Url = "https://map.maphive.local/",
                    RequiresAuth = true,
                    IsCommon = true
                },
                new Application
                {
                    Uuid = Guid.NewGuid(),
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
                    Uuid = Guid.NewGuid(),
                    //no short name, so can test uuid in the url part!
                    Name = "MapHive MapApp",
                    Description = "MapHive map app",
                    Url = "https://maps.maphive.local/",
                    RequiresAuth = false
                },
                new Application
                {
                    Uuid = Guid.NewGuid(),
                    //no short name, so can test uuid in the url part!
                    Name = "MapHive SiteAdmin",
                    Description = "MapHive platform Admin app",
                    Url = "https://masterofpuppets.maphive.local/",
                    RequiresAuth = false
                },
                new Application
                {
                    Uuid = Guid.NewGuid(),
                    ShortName = "tapp1",
                    Name = "TApp1",
                    Description = "A test HOST app that suppresses nested framed apps",
                    Url = "https://test1.maphive.local/?suppressnested=true#some/hash/123/456",
                    IsCommon = true
                },
                new Application
                {
                    Uuid = Guid.NewGuid(),
                    ShortName = "tapp2",
                    Name = "TApp2",
                    Description = "A test HOST app that suppresses nested framed apps",
                    Url = "https://test2.maphive.local/?param=test param so can be sure paraterised app urls also work&suppressnested=true",
                    UseSplashscreen = true,
                    IsCommon = true
                },
                new Application
                {
                    Uuid = Guid.NewGuid(),
                    ShortName = "tapp3",
                    Name = "TApp3",
                    Description = "A test HOST app that suppresses nested framed apps",
                    Url = "https://test3.maphive.local/",
                    IsCommon = true
                }
                );
        }

        private static void XWindowOriginsSeed(IXWindow context)
        {
            var origins = new[]
            {
                "", "hive", "hgis", "admin", "masterofpuppets", "testhive", "test1", "test2", "test3"
            };

            foreach (var origin in origins)
            {
                context.XWindowOrigins.AddOrUpdate(
                    new XWindowOrigin
                    {
                        Uuid = Guid.NewGuid(),
                        Origin = $"{origin}{(string.IsNullOrEmpty(origin) ? "" : ".")}maphive.local"
                    },
                    new XWindowOrigin
                    {
                        Uuid = Guid.NewGuid(),
                        Origin = $"{origin}{(string.IsNullOrEmpty(origin) ? "" : ".")}maphive.net"
                    }
                );
            }
        }
    }
}
