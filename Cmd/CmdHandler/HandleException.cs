using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cartomatic.CmdPrompt.Core;

namespace MapHive.Server.Cmd
{
    public partial class CommandHandler
    {
        private void HandleException(Exception ex, bool skipIntro = false)
        {
            var e = ex;

            if (!skipIntro)
            {
                ConsoleEx.WriteErr("Ooops, the following exception has been encountered: ");
            }
            
            ConsoleEx.WriteLine(e.Message, ConsoleColor.DarkRed);

            if (e.InnerException != null)
            {
                ConsoleEx.WriteLine("Inner exceptions are: ", ConsoleColor.DarkYellow);
            }

            while (e.InnerException != null)
            {
                e = e.InnerException;
                ConsoleEx.WriteLine(e.Message, ConsoleColor.DarkYellow);

            }

            Console.WriteLine();
        }
    }
}
