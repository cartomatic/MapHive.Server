using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cartomatic.CmdPrompt.Core;
using MapHive.Identity.MembershipReboot;
using MapHive.Server.Core.DataModel;
using MapHive.Server.Core.Events;
using MapHive.Server.DataModel.DAL;

namespace MapHive.Server.Cmd.Core
{
    public partial class CommandHandler
    {
        protected virtual async Task Handle_AddSuperUser(Dictionary<string, string> args)
        {
            var cmd = GetCallerName();

            if (GetHelp(args))
            {
                Console.WriteLine($"'{cmd}' : adds a superuser to the system");
                Console.WriteLine($"syntax: {cmd} space separated params: ");
                Console.WriteLine("\t[e:email]");
                Console.WriteLine("\t[p:pass]");
                Console.WriteLine();
                Console.WriteLine($"example: {cmd} e:dev@maphive.net p:test");
                return;
            }

            var email = ExtractParam("e", args);
            var pass = ExtractParam("p", args);

            //use the default account if email and pass not provided
            if (string.IsNullOrEmpty(email) && string.IsNullOrEmpty(pass))
            {
                email = "dev@maphive.net";
                pass = "test";
            }

            ConsoleEx.WriteLine($"Creating user: '{email}' with the following pass: '{pass}'", ConsoleColor.DarkYellow);
            
            //need a valid user to create a Core.Base object
            Server.Core.Utils.Identity.ImpersonateGhostUser();

            var user = new DataModel.User
            {
                Email = email
            };


            //Note: db context uses a connection defined in app cfg. 
            //TODO - make it somewhat dynamic!          
            try
            {
                //destroy a previous account if any
                await DestroyUser<DataModel.User>(email, new MapHiveDbContext("MapHiveMeta"), CustomUserAccountService.GetInstance("MapHiveMbr"));

                IDictionary<string, object> op = null;
                user.UserCreated += (sender, eventArgs) =>
                {
                    op = eventArgs.OperationFeedback;
                }; 

                await user.CreateAsync(new MapHiveDbContext("MapHiveMeta"), CustomUserAccountService.GetInstance("MapHiveMbr"));

                //once user is created, need to perform an update in order to set it as valid
                user.IsAccountVerified = true;
                await user.UpdateAsync(new MapHiveDbContext("MapHiveMeta"), CustomUserAccountService.GetInstance("MapHiveMbr"), user.Uuid);

                //and also need to change the pass as the default procedure autogenerates a pass
                CustomUserAccountService.GetInstance("MapHiveMbr")
                    .ChangePassword(user.Uuid, (string)op["InitialPassword"], pass);

                ConsoleEx.WriteOk($"User '{email}' with the following pass: '{pass}' has been created.");
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
