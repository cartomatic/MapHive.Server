using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MapHive.Server.Core
{
    public static partial class WebClientConfigurationExtensions
    {
        /// <summary>
        /// Injects a client config js script into page header
        /// </summary>
        /// <param name="p"></param>
        /// <param name="appIdentifiers"></param>
        /// <returns></returns>
        public static async Task InjectMhCfg(this System.Web.UI.Page p, string appIdentifiers)
        {
            //search for title index, so can inject the script just after that
            //assuming that all the meta stuff goes before the title though...
            var idx = 0;
            foreach (var control in p.Header.Controls)
            {
                if (control.GetType() == typeof (System.Web.UI.HtmlControls.HtmlTitle))
                {
                    break;
                }
                idx += 1;
            }

            //Just inject a script with mh dynamic configuration
            p.Header.Controls.AddAt(++idx, new LiteralControl(Environment.NewLine + Environment.NewLine + '\t'));

            System.Web.UI.HtmlControls.HtmlGenericControl mhCfgScript = new System.Web.UI.HtmlControls.HtmlGenericControl("script");
            mhCfgScript.Attributes["type"] = "text/javascript";

            mhCfgScript.InnerHtml = await GetScriptContent(appIdentifiers);
            p.Header.Controls.AddAt(++idx, mhCfgScript);

        }


        /// <summary>
        /// Returns a cfg in a form of an injectable script content
        /// </summary>
        /// <param name="appIdentifier">identifier of an application being launched - used to extract appropriate localisation for current request</param>
        /// <param name="langCode">language code to extract the localisation for</param>
        /// <returns></returns>
        private static async Task<string> GetScriptContent(string appIdentifier = null, string langCode = null)
        {
            var cfg = await WebClientConfiguration.Prepare(appIdentifier);


            //Note:
            //In general camelised property names are expected on the clientside, and this is achieved with
            //new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() }
            //
            //there is a little problem with that though - localisation is a dict, therefore it will get camelised when serialising. And this is NOT
            //a good thing here, as localisation keys are class names and they should be maintained...
            //
            //in order to achieve that, a little trick is done. localisation is replaced with a token, serialised, and then put back into the output.
            object localisation = null;
            if (cfg.ContainsKey("Localisation"))
            {
                localisation = cfg["Localisation"];
            };
            var localeToken = "{LOCALISATION_REPLACEMENT}";
            cfg["Localisation"] = localeToken;

            var cfgStr = JsonConvert.SerializeObject(cfg, Formatting.None,
                new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() });

            var localeStr = JsonConvert.SerializeObject(localisation, Formatting.None);

            return $"var __mhcfg__ = {cfgStr.Replace($"\"{localeToken}\"", localeStr)};";
        }
    }
}
