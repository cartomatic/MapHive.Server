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
        /// Adds test users to the setup cfg
        /// </summary>
        /// <param name="args"></param>
        protected virtual async Task Handle_AddTestUsers(Dictionary<string, string> args)
        {
            var cmd = GetCallerName();

            if (GetHelp(args))
            {
                Console.WriteLine($"'{cmd}' : adds test users to the setup cfg.");
                Console.WriteLine();

                //apps do not require help really
                Console.WriteLine();

                return;
            }

            //one more super in the hive
            await Handle_AddSuper(new Dictionary<string, string>()
            {
                {"e", "test1@maphive.net"},
                {"p", "test"}
            });

            
            //two standard users
            await Handle_AddUser(new Dictionary<string, string>()
            {
                {"e", "test2@maphive.net"},
                {"p", "test"}
            });
            await Handle_AddUser(new Dictionary<string, string>()
            {
                {"e", "test3@maphive.net"},
                {"p", "test"}
            });

            //maybe some org users too....

            Console.WriteLine();
        }
        
    }
}
