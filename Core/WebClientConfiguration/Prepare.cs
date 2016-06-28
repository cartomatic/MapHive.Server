using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MapHive.Server.Core
{
    public partial class WebClientConfiguration
    {
        /// <summary>
        /// Prepares web client configuration
        /// </summary>
        /// <param name="appIdentifier"></param>
        /// <param name="langCode"></param>
        /// <returns></returns>
        public static async Task<Dictionary<string, object>> Prepare(string appIdentifiers)
        {
            var cfg = new Dictionary<string, object>();

            //output the app hash properties
            cfg[nameof(AppHashProperties)] = AppHashProperties;
            cfg[nameof(HashPropertyDelimiter)] = HashPropertyDelimiter;
            cfg[nameof(HashPropertyValueDelimiter)] = HashPropertyValueDelimiter;

            //do an 'auth preflight', so user gets better experience - app will prompt for authentication straight away.
            cfg["AuthRequiredAppIdentifiers"] = await GetIdentifiersForAppsRequiringAuth();


            //API endpoint - where the MapHive services are
            cfg["ApiEndPoint"] = ConfigurationManager.AppSettings["ApiEndPoint"];

            //Allowed origins for the xwindow post message communication
            cfg["AllowedXWindowMsgBusOrigins"] = ConfigurationManager.AppSettings["AllowedXWindowMsgBusOrigins"]?.Split(',');


            //API map - customisation of the default api endpoints declared in the MapHive ExtJs
            try
            {
                cfg["apiMap"] =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(
                        ConfigurationManager.AppSettings["apiMap"]);
            }
            catch
            {
                //ignore
            }

            //grab the localisation for the application
            cfg["LangCode"] = await GetRequestLang();
            cfg["Localisation"] = await GetAppLocalisation(appIdentifiers, (string)cfg["LangCode"]);

            return cfg;
        }
    }
}
