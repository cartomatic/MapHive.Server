using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cartomatic.CmdPrompt.Core;
using Cartomatic.Utils.Data;
using MapHive.Identity.MembershipReboot;
using MapHive.Server.Core.DAL.DbContext;
using Npgsql;

namespace MapHive.Server.Cmd.Core
{
    public partial class CommandHandler
    {

        /// <summary>
        /// Handles setting up the MapHive environment - maphive meta db, idsrv db and membership reboot db
        /// </summary>
        /// <param name="args"></param>
        protected virtual async Task Handle_SetUpDb(IDictionary<string, string> args)
        {
            var cmd = GetCallerName();

            if (GetHelp(args))
            {
                Console.WriteLine($"'{cmd}' : sets up the maphive dbs - maphive_meta, maphive_idsrv, maphive_mr; uses the configured db credentials to connect to the db server.");
                Console.WriteLine($"syntax: {cmd} space separated params: ");
                Console.WriteLine("\t[full:{presence}; whether or not all the databases created/upgraded]");
                Console.WriteLine("\t[mh:{presence}; whether or not maphive_meta should be created/upgraded]");
                Console.WriteLine("\t[mr:{presence}; whether or not maphive_mr (MembershipReboot) should be created/upgraded]");
                Console.WriteLine("\t[idsrv:{presence}; whether or not maphive_idsrv (IdentityServer) should be created/upgraded]");
                Console.WriteLine("\t[xfull:{presence}; whether or not all the databases should be dropped prior to being recreated]");
                Console.WriteLine("\t[xmh:{presence}; whether or not maphive_meta should be dropped prior to being recreated]");
                Console.WriteLine("\t[xmr:{presence}; whether or not maphive_mr (MembershipReboot) should be dropped prior to being recreated]");
                Console.WriteLine("\t[xidsrv:{presence}; whether or not maphive_idsrv (IdentityServer) should be dropped prior to being recreated]");

                Console.WriteLine($"example: {cmd} m mr idsrv xm xmr xidsrv");
                Console.WriteLine($"example: {cmd} full xfull");
                Console.WriteLine();

                return;
            }

            var dbsToDrop = new List<string>();
            var migrationConfigs = new Dictionary<DbMigrationsConfiguration, string>();

            if (ContainsParam("full", args) || ContainsParam("mh", args))
            {
                migrationConfigs[new MapHive.Server.Core.DAL.Migrations.MapHiveMetaConfiguration.Configuration()] = "maphive_meta";
            }
            if(ContainsParam("xfull", args) || ContainsParam("xmh", args))
            {
                dbsToDrop.Add("maphive_meta");
            }
            if (ContainsParam("full", args) || ContainsParam("mr", args))
            {
                migrationConfigs[new MapHive.Identity.MembershipReboot.Migrations.Configuration()] = "maphive_mr";
            }
            if (ContainsParam("xfull", args) || ContainsParam("xmr", args))
            {
                dbsToDrop.Add("maphive_mr");
            }
            if (ContainsParam("full", args) || ContainsParam("idsrv", args))
            {
                migrationConfigs[new MapHive.Identity.IdentityServer.Migrations.ClientConfiguration.Configuration()] = "maphive_idsrv";
                migrationConfigs[new MapHive.Identity.IdentityServer.Migrations.OperationalConfiguration.Configuration()] = "maphive_idsrv";
                migrationConfigs[new MapHive.Identity.IdentityServer.Migrations.ScopeConfiguration.Configuration()] = "maphive_idsrv";
            }
            if (ContainsParam("xfull", args) || ContainsParam("xidsrv", args))
            {
                dbsToDrop.Add("maphive_idsrv");
            }

            //got here, so need to drop the dbs first in order to recreate them later
            if (dbsToDrop.Count > 0)
            {
                if (
                    !PromptUser(
                        $"You are about to drop the following databases {string.Join(", ", dbsToDrop)}. Are you sure you want to proceed?"))
                    return;

                DropDb(dbsToDrop.ToArray());
            }


            if (migrationConfigs.Count > 0)
            {
                ConsoleEx.WriteLine("Updating dbs... ", ConsoleColor.DarkYellow);
                try
                {
                    foreach (var migrationCfg in migrationConfigs.Keys)
                    {
                        var dbc = new DataSourceCredentials
                        {
                            DbName = migrationConfigs[migrationCfg],
                            ServerHost = Dsc.ServerHost,
                            ServerPort = Dsc.ServerPort,
                            UserName = Dsc.UserName,
                            Pass = Dsc.Pass,
                            DataSourceType = DataSourceType.PgSql
                        };

                        try
                        {
                            ConsoleEx.Write($"{migrationCfg.ToString()}... ", ConsoleColor.DarkYellow);

                            //TODO - make the provider name somewhat more dynamic...
                            migrationCfg.TargetDatabase = new DbConnectionInfo(dbc.GetConnectionString(), "Npgsql");

                            var migrator = new DbMigrator(migrationCfg);


                            migrator.Update();
                            ConsoleEx.Write("Done!" + Environment.NewLine, ConsoleColor.DarkGreen);
                        }
                        catch (Exception ex)
                        {
                            ConsoleEx.WriteErr($"OOOPS... Failed to create/update database: {migrationCfg}");
                            HandleException(ex, true);
                            throw;
                        }

                    }
                    
                }
                catch (Exception ex)
                {
                    //ignore
                    return;
                }
            }


            if (dbsToDrop.Count == 0 && migrationConfigs.Count == 0)
            {
                ConsoleEx.WriteLine(
                    "Looks like i have nothing to do... Type 'setup help' for more details on how to use this command.",
                    ConsoleColor.DarkYellow);
            }
            else
            {
                EfConnectionsPoolCacheCleanup();
            }

            Console.WriteLine();
        }


        /// <summary>
        /// Clears the EF connections pool cache
        /// </summary>
        protected void EfConnectionsPoolCacheCleanup()
        {
            ConsoleEx.Write("EF connections pool cache cleanup... ", ConsoleColor.DarkYellow);

            //EF caches connections, so in some scenarios, when there was a conn to a db that is being dropped and recreated EF will fail to 
            //reconnect to it.
            //After such fail EF will reset the conns pool cache and open up new connections
            //So basically here just force the EF conn failure so it can drop previously cachec connections that are not valid anymore

            try
            {
                var ctx = new MapHiveDbContext();
                // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                ctx.Applications.FirstOrDefault();
            }
            catch (Exception)
            {
                //ignore
            }

            try
            {
                var userAccountService = CustomUserAccountService.GetInstance("MapHiveMbr");
                userAccountService.GetByEmail("some@email.com");
            }
            catch (Exception)
            {
                //ignore
            }

            ConsoleEx.Write("Done!" + Environment.NewLine, ConsoleColor.DarkGreen);
        }
    }
}
