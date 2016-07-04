using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cartomatic.CmdPrompt.Core;
using MapHive.Server.Core.DataModel.Validation;

namespace MapHive.Server.Cmd
{
    public partial class CommandHandler
    {
        private void HandleException(Exception ex, bool skipIntro = false)
        {
            
            if (!skipIntro)
            {
                ConsoleEx.WriteErr("Ooops, the following exception has been encountered: ");
            }

            //special treatment for validation exceptions; they may pop put when working with Base objects
            if (ex is ValidationFailedException)
            {
                var e = ex as ValidationFailedException;

                ;
                foreach (var validationError in e.ValidationErrors)
                {
                    ConsoleEx.WriteLine(validationError.Message, ConsoleColor.DarkRed);
                    foreach (var key in validationError.Info.Keys)
                    {
                        ConsoleEx.WriteLine($"\t{key}: {validationError.Info[key]}", ConsoleColor.DarkYellow);
                    }
                }
            }
            else
            {
                var e = ex;

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
            }
            
            Console.WriteLine();
        }
    }
}
