using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapHive.Server.Cmd
{
    class Program
    {
        static void Main(string[] args)
        {
            var cmdWatcher = new Cartomatic.CmdPrompt.Core.CmdWatcher(new CommandHandler())
            {
                Prompt = "MapHive...Bzz...>",
                PromptColor = ConsoleColor.DarkBlue
            };

            //setup if needed


            cmdWatcher.Init();


            Console.ReadLine();
        }
    }
}
