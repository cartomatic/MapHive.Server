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
        /// Registers the default apps, creates a super user with the access to the master of puppets app
        /// </summary>
        /// <param name="args"></param>
        protected virtual async Task Handle_SetUp(Dictionary<string, string> args)
        {
            var cmd = GetCallerName();

            if (GetHelp(args))
            {
                Console.WriteLine($"'{cmd}' : sets up the full maphive platform environment - dbs, apps, default master user.");
                Console.WriteLine();

                await Handle_SetUpDb(args);

                //apps do not require help really, as it just adds default apps

                await Handle_AddSuper(args);
                Console.WriteLine();

                return;
            }

            await Handle_SetUpDb(args);

            //when ready add apps and a master user
            if (ContainsParam("full", args) || ContainsParam("mh", args))
            {
                Console.WriteLine();
                await Handle_AddDefaultApps(null);
                Console.WriteLine();
                await Handle_AddSuper(null);
            }

            Console.WriteLine();
        }
        
    }
}
