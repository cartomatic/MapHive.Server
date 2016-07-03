using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cartomatic.CmdPrompt.Core;
using Cartomatic.Utils.Data;

namespace MapHive.Server.Cmd
{
    public partial class CommandHandler
    {
        private Cartomatic.Utils.Data.DataSourceCredentials DataSourceCredentials { get; set; }

        /// <summary>
        /// Sets default db credentials
        /// </summary>
        private void SetDefaultDsc()
        {
            DataSourceCredentials = new DataSourceCredentials
            {
                DataSourceType = DataSourceType.PgSql,
                ServerHost = "localhost",
                ServerPort = 5434,
                UserName = "postgres",
                Pass = "postgres",
                UseDefaultServiceDb = true
            };
        }


        /// <summary>
        /// handles setting db credentials
        /// </summary>
        /// <param name="args"></param>
        protected void Handle_Dsc(Dictionary<string, string> args)
        {
            var cmd = GetCallerName();

            if (GetHelp(args))
            {
                Console.WriteLine($"'{cmd}' : prints or sets db connection details for default connection");
                Console.WriteLine($"syntax: {cmd} space separated params: ");
                Console.WriteLine("\t[h:serverhost]");
                Console.WriteLine("\t[n:servername]");
                Console.WriteLine("\t[p:serverport]");
                Console.WriteLine("\t[d:database]");
                Console.WriteLine("\t[u:user]");
                Console.WriteLine("\t[P:pass]");
                Console.WriteLine();
                Console.WriteLine($"example: {cmd} h:localhost n: p:5434 d:postgres u:postgres P:postgres");
                Console.WriteLine();
                Console.WriteLine("The order of params is not important; required params are: host, port, user, pass;");
                Console.WriteLine("In most cases database is also a required param, although not always ie: dropping a db");
                Console.WriteLine("invalid params will result in errors when trying to connect.");
                Console.WriteLine();

                return;
            }

            if (args.Count > 0)
            {
                var serverhost = ExtractParam("h", args);
                var servername = ExtractParam("n", args);
                var serverport = ExtractParam<int?>("p", args);
                var database = ExtractParam("d", args);
                var user = ExtractParam("u", args);
                var pass = ExtractParam("P", args);

                if (ValidateDbCredentials(serverhost, servername, serverport, database, user, pass))
                {
                    DataSourceCredentials = new DataSourceCredentials
                    {
                        ServerHost = serverhost,
                        ServerName = servername,
                        ServerPort = serverport,
                        DbName = database,
                        UserName = user,
                        Pass = pass,
                        DataSourceType = DataSourceType.PgSql
                    };
                }
                else
                {
                    ConsoleEx.WriteErr($"Invalid database credentials. Type {cmd} help to get more details on the command usage.");
                }

            }

            PrintDbc();

        }

        /// <summary>
        /// Prints currently configured database credentials
        /// </summary>
        private void PrintDbc()
        {
            Console.WriteLine("Current database credentials:");

            var cl = ConsoleColor.DarkMagenta;

            Console.Write("serverhost:");
            ConsoleEx.Write(DataSourceCredentials.ServerHost, cl);
            Console.Write(", servername:");
            ConsoleEx.Write(DataSourceCredentials.ServerName, cl);
            Console.Write(", serverport:");
            ConsoleEx.Write(DataSourceCredentials.ServerPort.ToString(), cl);
            Console.Write(", database:");
            ConsoleEx.Write(DataSourceCredentials.DbName, cl);
            Console.Write(", user:");
            ConsoleEx.Write(DataSourceCredentials.UserName, cl);
            Console.Write(", pass:");
            ConsoleEx.Write(DataSourceCredentials.Pass, cl);
            Console.Write(Environment.NewLine);

            Console.WriteLine();
        }

        /// <summary>
        /// simplistic datasource validation
        /// </summary>
        /// <param name="serverhost"></param>
        /// <param name="servername"></param>
        /// <param name="serverport"></param>
        /// <param name="database"></param>
        /// <param name="user"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        private static bool ValidateDbCredentials(string serverhost, string servername, int? serverport, string database, string user, string pass)
        {
            return !string.IsNullOrEmpty(serverhost) &&
                   serverport.HasValue &&
                   !string.IsNullOrEmpty(user) &&
                   !string.IsNullOrEmpty(pass);
        }
    }
}
