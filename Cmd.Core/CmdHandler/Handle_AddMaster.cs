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
                Console.WriteLine("\t[s:slug] user's slug");
                Console.WriteLine("\t[o:{presence}] whether or not user is an org user");
                Console.WriteLine();
                Console.WriteLine($"example: {cmd} e:queen@maphive.net p:test");
                return;
            }

            var email = ExtractParam("e", args);
            var pass = ExtractParam("p", args);
            var slug = ExtractParam("s", args);
            var isOrgUser = ContainsParam("o", args);

            //use the default account if email and pass not provided
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(pass))
            {
                email = "queen@maphive.net";
                pass = "test";
            }

            if (!isOrgUser && string.IsNullOrEmpty(slug))
                slug = email.Split('@')[0];

            //delegate user creation
            var prms = new Dictionary<string, string>()
            {
                {"e", email},
                {"p", pass},
                {"s", slug}
            };
            if (isOrgUser)
            {
                prms.Add("o", null);
            }
            await Handle_AddUser(prms);
            
            try
            {
                ConsoleEx.Write("Setting up super user access... ", ConsoleColor.DarkYellow);

                var ctx = new MapHiveDbContext("MapHiveMeta");

                //create the master org if not exists!
                var org = await ctx.Organisations.FirstOrDefaultAsync(o => o.Slug == "thehive");
                if (org == null)
                {
                    org = await new Organisation
                    {
                        Slug = "thehive",
                        DisplayName = "The Hive"
                    }.CreateAsync(ctx);

                    //get the maphive admin app
                    var masterofpuppets = await ctx.Applications.FirstOrDefaultAsync(a => a.ShortName == "masterofpuppets");

                    //add a link to a non-public app!
                    //users with access to this organisation will be able to access this app too
                    //note: perhaps with some app access privs defined at some point
                    org.AddLink(masterofpuppets);
                    await org.UpdateAsync(ctx);
                }
                
                //get user by email
                var user = await ctx.Users.FirstOrDefaultAsync(u => u.Email == email);

                //assign user to the org, so user is visible 'within' an org
                org.AddLink(user);
                await org.UpdateAsync(ctx);

                //assing the master org owner role to a user
                var orgOwnerR = await org.GetOrgOwnerRoleAsync(ctx);
                user.AddLink(orgOwnerR);
                

                //finally save a user
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
