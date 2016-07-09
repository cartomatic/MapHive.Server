using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapHive.Server.Cmd.Core;

namespace MapHive.Server.Cmd
{
    class Program
    {
        static void Main(string[] args)
        {
            Task.WaitAll(
                Task.Run(() => MainAsync(args))
            );
        }

        static async Task MainAsync(string[] args)
        {
            var cmdWatcher = new Cartomatic.CmdPrompt.Core.CmdWatcher(new CommandHandler())
            {
                Prompt = "MapHive...Bzz...>",
                PromptColor = ConsoleColor.DarkBlue
            };

            //setup if needed


            await cmdWatcher.Init();


            Console.ReadLine();
        }
    }
}
