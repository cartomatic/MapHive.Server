using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapHive.Identity.MembershipReboot;
using MapHive.Server.Core.DataModel;
using MapHive.Server.DataModel.DAL;

namespace MapHive.Server.Cmd
{
    public partial class CommandHandler
    {
        protected async Task Handle_AddSuperUser(Dictionary<string, string> args)
        {
            var cmd = GetCallerName();

            if (GetHelp(args))
            {
                Console.WriteLine($"'{cmd}' : adds a superuser to the system");
                Console.WriteLine($"syntax: {cmd} space separated params: ");
                Console.WriteLine("\t[e:emnail]");
                Console.WriteLine("\t[p:pass]");
                Console.WriteLine();
                Console.WriteLine($"example: {cmd} e:dev@maphive.net p:test");
                return;
            }

            var email = ExtractParam("e", args);
            var pass = ExtractParam("p", args);


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
                //TODO - need mbr service!

                await user.Create(new MapHiveDbContext("MapHiveMeta"), CustomUserAccountService.GetInstance("MapHiveMbr"));
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return;
            }

            //TODO - need to change the pass too!!! This requires interacting with MemebrshipReboot


        }
    }
}
