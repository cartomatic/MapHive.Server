using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cartomatic.CmdPrompt.Core;
using MapHive.Identity.MembershipReboot;
using MapHive.Server.Core.DataModel;
using MapHive.Server.Core.Events;
using MapHive.Server.Core.DAL;
using MapHive.Server.Core.DAL.DbContext;

namespace MapHive.Server.Cmd.Core
{
    public partial class CommandHandler
    {
        protected virtual async Task Handle_AddSuper(Dictionary<string, string> args)
        {
            var cmd = GetCallerName();

            args = args ?? new Dictionary<string, string>();

            if (GetHelp(args))
            {
                Console.WriteLine($"'{cmd}' : adds a maphive superuser to the system. expects the master of puppets app to be registered; will fail if it is not.");
                Console.WriteLine($"syntax: {cmd} space separated params: ");
                Console.WriteLine("\t[e:email]");
                Console.WriteLine("\t[p:pass]");
                Console.WriteLine();
                Console.WriteLine($"example: {cmd} e:queen@maphive.net p:test");
                return;
            }

            var email = ExtractParam("e", args);
            var pass = ExtractParam("p", args);

            //use the default account if email and pass not provided
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(pass))
            {
                email = "queen@maphive.net";
                pass = "test";
            }

            //delegate user creation
            await Handle_AddUser(new Dictionary<string, string>()
            {
                { "e", email }, {"p", pass }
            });
            
            
            try
            {
                ConsoleEx.Write("Enabling Puppeteer access... ", ConsoleColor.DarkYellow);

                var ctx = new MapHiveDbContext("MapHiveMeta");

                //get user by email
                var user = await ctx.Users.FirstOrDefaultAsync(u => u.Email == email);

                //get the maphive admin app
                var masterofpuppets = await ctx.Applications.FirstOrDefaultAsync(a => a.ShortName == "masterofpuppets");

                user.AddLink(masterofpuppets);
                await user.UpdateAsync(ctx, CustomUserAccountService.GetInstance("MapHiveMbr"));

                ConsoleEx.Write("Done!" + Environment.NewLine, ConsoleColor.DarkGreen);
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return;
            }
            
        }
    }
}
