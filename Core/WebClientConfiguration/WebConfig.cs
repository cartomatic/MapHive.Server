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
        /// Returns a map hive api endpoint as defined in web.config
        /// </summary>
        /// <returns></returns>
        private static string GetApiEndpoint()
        {
            return ConfigurationManager.AppSettings["ApiEndpoint"];
        }

        /// <summary>
        /// Api methods - some mh api endpoints that are consulted during the client config preparation
        /// </summary>
        private static Dictionary<string, string> CfgApiCalls { get; set; }


        /// <summary>
        /// Gets a specified mh api endpoint url; throws if app is misconfigured, or a non-existing key is requested
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static string GetApiCallUrl(string key)
        {
            if (CfgApiCalls == null)
            {
                CfgApiCalls =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(
                        ConfigurationManager.AppSettings["CfgApiCalls"]);
            }
            return CfgApiCalls[key];
        }
    }
}
