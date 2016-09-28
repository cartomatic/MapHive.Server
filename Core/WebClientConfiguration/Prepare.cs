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
        public static async Task<Dictionary<string, object>> PrepareAsync(string appIdentifiers)
        {
            var cfg = new Dictionary<string, object>();

            //output the app hash properties
            cfg[nameof(AppHashProperties)] = AppHashProperties;
            cfg[nameof(HashPropertyDelimiter)] = HashPropertyDelimiter;
            cfg[nameof(HashPropertyValueDelimiter)] = HashPropertyValueDelimiter;

            //do an 'auth preflight', so user gets better experience - app will prompt for authentication straight away.
            cfg["AuthRequiredAppIdentifiers"] = await GetIdentifiersForAppsRequiringAuthAsync();


            //API endpoint - where the MapHive services are
            cfg["ApiEndPoints"] = GetApiEndpoints();

            //Allowed origins for the xwindow post message communication
            cfg["AllowedXWindowMsgBusOrigins"] = await GetAllowedXWindowOriginsAsync();


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
            cfg["LangCode"] = await GetRequestLangAsync();
            cfg[nameof(LangParam)] = LangParam;
            cfg["Localisation"] = await GetAppLocalisationAsync(appIdentifiers, (string)cfg["LangCode"]);
            cfg[nameof(HeaderLang)] = HeaderLang;

            //some other xtra headers
            cfg[nameof(HeaderTotal)] = HeaderTotal;
            cfg[nameof(HeaderSource)] = HeaderSource;

            return cfg;
        }
    }
}
