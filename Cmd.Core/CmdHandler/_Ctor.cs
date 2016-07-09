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
            //register some xtra commands
            SetUpCommandMap(new Dictionary<string, string>
            {
                { "dsc", "dsc" }, { "conn", "dsc" },
                { "dropdb", "dropdb" },
                { "setup", "setup" },
                { "addsuperuser", "addsuperuser" }, { "addsuper", "addsuperuser" }, { "addmaster", "addsuperuser" }
            });


            //default db credentials
            SetDefaultDsc();
        }

        public CommandHandler()
            : this ("MapHive CMD v1.0....")
        {
        }
    }
}
