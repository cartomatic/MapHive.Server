using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cartomatic.CmdPrompt.Core;
using MapHive.Server.Core.DataModel;
using MapHive.Server.Core.DAL.DbContext;

namespace MapHive.Server.Cmd.Core
{
    public partial class CommandHandler
    {
        /// <summary>
        /// Handles adding default platform applications
        /// </summary>
        /// <param name="args"></param>
        protected virtual async Task Handle_AddDefaultApps(IDictionary<string, string> args)
        {
            var cmd = GetCallerName();

            if (GetHelp(args))
            {
                Console.WriteLine(
                    $"'{cmd}' : adds default apps to.");
                Console.WriteLine($"syntax: {cmd}");
                Console.WriteLine();

                return;
            }

            ConsoleEx.Write("Registering default platform apps...", ConsoleColor.DarkYellow);
            var ctx = new MapHiveDbContext();
            ctx.Applications.AddOrUpdate(GetApps());
            await ctx.SaveChangesAsync();
            ConsoleEx.WriteOk("Done!");
        }

        /// <summary>
        /// handles adding test platform applications
        /// </summary>
        /// <param name="args"></param>
        protected virtual async Task Handle_AddTestApps(IDictionary<string, string> args)
        {
            var cmd = GetCallerName();

            if (GetHelp(args))
            {
                Console.WriteLine(
                    $"'{cmd}' : adds default apps to.");
                Console.WriteLine($"syntax: {cmd}");
                Console.WriteLine();

                return;
            }

            ConsoleEx.Write("Registering test platform apps...", ConsoleColor.DarkYellow);

            var ctx = new MapHiveDbContext();
            ctx.Applications.AddOrUpdate(GetTestApps());
            await ctx.SaveChangesAsync();
            ConsoleEx.WriteOk("Done!");

        }

        /// <summary>
        /// gets a default platform appset
        /// </summary>
        /// <returns></returns>
        protected static Application[] GetApps()
        {
            return new Application[]
            {
                new Application
                {
                    Uuid = Guid.Parse("5f541902-4f42-4a58-8dee-523ea02cd1fd"),
                    ShortName = "mhhive",
                    Name = "Hive@MapHive",
                    Description = "The Hive",
                    Urls = "https://maphive.local/|https://maphive.net/|https://hive.maphive.local/|https://hive.maphive.net/",
                    IsCommon = false,
                    IsDefault = false,
                    IsHidden = true
                },
                new Application
                {
                    Uuid = Guid.Parse("30aca350-41a4-4906-be82-da1247537f19"),
                    ShortName = "mhhgis",
                    Name = "HGIS v1",
                    Description = "A dev test port of the Cartomatic\'s HGIS; a good example of an external and/or exisiting app inclusion into the system",
                    Urls = "https://hgisold.maphive.local/|https://hgisold.maphive.net/",
                    IsCommon = true,
                    IsDefault = true
                },
                
                new Application
                {
                    Uuid = Guid.Parse("1e025446-1a25-4639-a302-9ce0e2017a59"),
                    //no short name, so can test uuid in the url part!
                    Name = "MapHive SiteAdmin",

                    //TODO - make this app only available to the queen organization!

                    Description = "MapHive platform Admin app",
                    Urls = "https://masterofpuppets.maphive.local/|https://masterofpuppets.maphive.net/",
                    RequiresAuth = true
                }
            };
        }

        /// <summary>
        /// Returns a set of test applications
        /// </summary>
        /// <returns></returns>
        protected static Application[] GetTestApps()
        {
            return new Application[]
            {
                new Application
                {
                    Uuid = Guid.Parse("a2f1eaf8-491f-45ab-be64-e84ad74ce7d2"),
                    ShortName = "mhtesthive",
                    Name = "TestHive@MapHive",
                    Description = "The Test Hive",
                    Urls = "https://testhive.maphive.local/|https://testhive.maphive.net/",
                    IsCommon = false,
                    IsDefault = false,
                    IsHidden=true
                },
                new Application
                {
                    Uuid = Guid.Parse("30aca350-41a4-4906-be82-da1247537f19"),
                    ShortName = "mhhgis",
                    Name = "HGIS v1",
                    Description = "A dev test port of the Cartomatic\'s HGIS; a good example of an external and/or exisiting app inclusion into the system",
                    Urls = "https://hgisold.maphive.local/|https://hgisold.maphive.net/",
                    IsCommon = true,
                    IsDefault = true
                },
                new Application
                {
                    Uuid = Guid.Parse("473cb87f-815f-4362-aaca-021d163b60b7"),
                    ShortName = "mhmapapp",
                    Name = "MapHive MapApp",
                    Description = "MapHIve Map App",
                    Urls = "https://map.maphive.local/|https://map.maphive.net/",
                    RequiresAuth = true,
                    IsCommon = true
                },
                new Application
                {
                    Uuid = Guid.Parse("744e10e2-ffd8-4857-8909-d8638c8eb6f5"),
                    Name = "Admin",
                    ShortName = "mhadmin",
                    Description = "MapHive Admin - this will be the basic admin app users will use to control their accounts",
                    Urls = "https://admin.maphive.local/|https://admin.maphive.net/",
                    UseSplashscreen = true,
                    RequiresAuth = true,
                    IsCommon = true
                },
                //new Application
                //{
                //    Uuid = Guid.Parse("cc6dbf2e-f2f0-462f-a4a3-e2c13aa21e49"),
                //    //no short name, so can test uuid in the url part!
                //    Name = "MapHive MapApp",
                //    Description = "MapHive map app",
                //    Urls = "https://maps.maphive.local/|https://maps.maphive.net/",
                //    RequiresAuth = false
                //},
                new Application
                {
                    Uuid = Guid.Parse("1e025446-1a25-4639-a302-9ce0e2017a59"),
                    //no short name, so can test uuid in the url part!
                    Name = "MapHive SiteAdmin",
                    Description = "MapHive platform Admin app",
                    Urls = "https://masterofpuppets.maphive.local/|https://masterofpuppets.maphive.net/",
                    RequiresAuth = true
                },
                new Application
                {
                    Uuid = Guid.Parse("2781a6da-38f7-49a5-8837-05c825e776b6"),
                    ShortName = "tapp1",
                    Name = "TApp1",
                    Description = "A test HOSTED app that suppresses nested framed apps",
                    Urls = "https://test1.maphive.local/?suppressnested=true#some/hash/123/456|https://test1.maphive.net/?suppressnested=true#some/hash/123/456",
                    IsCommon = true
                },
                new Application
                {
                    Uuid = Guid.Parse("bddb6bfb-b6a9-4478-a236-5e5ba5d8f8fc"),
                    ShortName = "tapp2",
                    Name = "TApp2",
                    Description = "A test HOSTED app that suppresses nested framed apps",
                    Urls = "https://test2.maphive.local/?param=test param so can be sure paraterised app urls also work&suppressnested=true|https://test2.maphive.net/?param=test param so can be sure paraterised app urls also work&suppressnested=true",
                    UseSplashscreen = true,
                    IsCommon = true
                },
                new Application
                {
                    Uuid = Guid.Parse("748acb4b-ab56-46bb-a348-a669ebd19f6f"),
                    ShortName = "tapp3",
                    Name = "TApp3",
                    Description = "A test HOSTED app that suppresses nested framed apps",
                    Urls = "https://test3.maphive.local/|https://test3.maphive.net/",
                    IsCommon = true
                }
            };
        }
    }
}
