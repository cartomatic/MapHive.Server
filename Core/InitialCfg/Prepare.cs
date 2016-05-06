using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MapHive.Server.Core
{
    public partial class InitialCfg
    {
        /// <summary>
        /// Prepares a dynamic mh configuration object
        /// </summary>
        /// <param name="appUrl"></param>
        /// <returns></returns>
        public static InitialCfg Prepare(string appUrl)
        {
            var cfg = new InitialCfg();

            //do an auth preflight, so user gets better experience - app will prompt for authentication straight away.
            cfg.RequiresAuth = Application.RequiresAuth(appUrl);

            //Note: make AppHashProperties, HashPropertyDelimiter, HashPropertyValueDelimiter web.config based at some point. Or better - overridable!

            cfg.MhApiEndPoint = ConfigurationManager.AppSettings["MhApiEndpoint"];

            cfg.AllowedXWindowMsgBusOrigins = ConfigurationManager.AppSettings["AllowedXWindowMsgBusOrigins"].Split(',');


            try
            {
                cfg.MhApiMap =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(
                        ConfigurationManager.AppSettings["MhApiMap"]);
            }
            catch
            {
                //ignore
            }

            return cfg;
        }

        /// <summary>
        /// Returns a cfg in a form of an injectable script content
        /// </summary>
        /// <param name="appUrl"></param>
        /// <returns></returns>
        public static string GetScriptContent(string appUrl)
        {
            return $"var __mhcfg__ = {JsonConvert.SerializeObject(Prepare(appUrl), Formatting.None, new JsonSerializerSettings() {ContractResolver = new CamelCasePropertyNamesContractResolver() })};";
        }
    }
}
