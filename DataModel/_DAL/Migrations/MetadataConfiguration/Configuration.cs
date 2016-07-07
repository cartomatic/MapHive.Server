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

    public sealed class Configuration : DbMigrationsConfiguration<MapHiveDbContext>
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
            EmailLocalisationsSeed(context);

            XWindowOriginsSeed(context);
        }

        private void AppLocalisationsSeed(MapHiveDbContext context)
        {

            context.AppLocalisations.AddOrUpdate(
                new AppLocalisation
                {
                    Uuid = Guid.Parse("17dd0189-ed1b-404f-b39c-b1e3f4718d40"),
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
                    Uuid = Guid.Parse("7dbb4b9a-e192-4d00-941e-c90069604e4f"),
                    ApplicationName = "SomeApp",
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
                    Uuid = Guid.Parse("cc58679a-ac75-43c6-8b1c-94d3238b95d6"),
                    ApplicationName = "mh",
                    ClassName = "module.data.DataViewBase",
                    TranslationKey = "btnCreate",
                    Translations = new Translations
                    {
                        { "en", "NEW WHOAA!!!" }
                    }
                },
                new AppLocalisation
                {
                    Uuid = Guid.Parse("b77cdffe-4da6-4327-a087-41ee45018ead"),
                    ApplicationName = "MasterOfPuppets",
                    ClassName = "view.applications.Applications",
                    TranslationKey = "btnCreate",
                    Translations = new Translations
                    {
                        { "en", "XXX" }
                    }
                }
            );
        }

        private void EmailLocalisationsSeed(MapHiveDbContext context)
        {
            context.EmailTemplates.AddOrUpdate(
                new EmailTemplateLocalisation
                {
                    Name = "User created",
                    Description = "Email sent when user has been created. Replacement tokens are: {InitialPassword}, {VerificationKey}, ... to be updated...",
                    Identifier = "user_created",
                    IsBodyHtml = true,
                    Translations = new EmailTranslations
                    {
                        { "en", new EmailTemplate{Title = "User created email title.", Body = "<font color=\"FF00FF\" face=\"courier new\" size=\"5\"><i>Some </i></font>nice HTML or so<b>met</b>hi<span style=\"background-color: rgb(153, 204, 0); \"><font color=\"800000\" size=\"7\">n<u>g</u>!</font></span>"} },
                        { "pl", new EmailTemplate{Title = "Utworzono konto użytkownika.", Body = "Email z informacjami co dalej po utworzeniu konta usera..."} }
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
                Uuid = Guid.Parse("ece753c3-f079-4772-8aa2-0960aeabc94d"),
                LangCode = "pl",
                Name = "Polski"
            },
            new Lang
            {
                Uuid = Guid.Parse("8323d1bb-e6f5-49d3-a441-837017d6e97e"),
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
                    Uuid = Guid.Parse("30aca350-41a4-4906-be82-da1247537f19"),
                    ShortName = "mhhgis",
                    Name = "HGIS v1",
                    Description = "A dev test port of the Cartomatic\'s HGIS; a good example of an external and/or exisiting app inclusion into the system",
                    Url = "https://hgis.maphive.local/",
                    IsCommon = true,
                    IsDefault = true
                },
                new Application
                {
                    Uuid = Guid.Parse("473cb87f-815f-4362-aaca-021d163b60b7"),
                    ShortName = "mhmapapp",
                    Name = "MapHive MapApp",
                    Description = "MapHIve Map App",
                    Url = "https://map.maphive.local/",
                    RequiresAuth = true,
                    IsCommon = true
                },
                new Application
                {
                    Uuid = Guid.Parse("744e10e2-ffd8-4857-8909-d8638c8eb6f5"),
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
                    Uuid = Guid.Parse("cc6dbf2e-f2f0-462f-a4a3-e2c13aa21e49"),
                    //no short name, so can test uuid in the url part!
                    Name = "MapHive MapApp",
                    Description = "MapHive map app",
                    Url = "https://maps.maphive.local/",
                    RequiresAuth = false
                },
                new Application
                {
                    Uuid = Guid.Parse("1e025446-1a25-4639-a302-9ce0e2017a59"),
                    //no short name, so can test uuid in the url part!
                    Name = "MapHive SiteAdmin",
                    Description = "MapHive platform Admin app",
                    Url = "https://masterofpuppets.maphive.local/",
                    RequiresAuth = true
                },
                new Application
                {
                    Uuid = Guid.Parse("2781a6da-38f7-49a5-8837-05c825e776b6"),
                    ShortName = "tapp1",
                    Name = "TApp1",
                    Description = "A test HOST app that suppresses nested framed apps",
                    Url = "https://test1.maphive.local/?suppressnested=true#some/hash/123/456",
                    IsCommon = true
                },
                new Application
                {
                    Uuid = Guid.Parse("bddb6bfb-b6a9-4478-a236-5e5ba5d8f8fc"),
                    ShortName = "tapp2",
                    Name = "TApp2",
                    Description = "A test HOST app that suppresses nested framed apps",
                    Url = "https://test2.maphive.local/?param=test param so can be sure paraterised app urls also work&suppressnested=true",
                    UseSplashscreen = true,
                    IsCommon = true
                },
                new Application
                {
                    Uuid = Guid.Parse("748acb4b-ab56-46bb-a348-a669ebd19f6f"),
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
