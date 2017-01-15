using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapHive.Server.Cmd.Core
{
    public partial class CommandHandler : Cartomatic.CmdPrompt.Core.DefaultCmdCommandHandler
    {
        public CommandHandler(string handlerInfo)
            : base (handlerInfo)
        {
            //register some xtra command aliases
            SetUpCommandMap(new Dictionary<string, string>
            {
                {"s", "setup" },
                { "conn", "dsc" },
                { "xuser", "destroyuser" },
                { "defaultapps", "adddefaultapps" },
                { "dfltapps", "adddefaultapps" },
                { "testapps", "addtestapps" }
            });


            //default db credentials
            SetDefaultDsc();

            //'ghost' user
            MapHive.Server.Core.Utils.Identity.ImpersonateGhostUser();
        }

        public CommandHandler()
            : this ("MapHive CMD v1.0....")
        {
        }
    }
}
