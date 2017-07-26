using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Cartomatic.Utils.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;

namespace MapHive.Server
{
    public static class DictionaryExtensions
    {
        public static void Merge<TKey, TValue>(this Dictionary<TKey, TValue> dict, Dictionary<TKey, TValue> mergedDict, bool overwrite = false)
        {
            if (mergedDict == null)
            {
                return;
            }

            foreach (var key in mergedDict.Keys)
            {
                if (overwrite || !dict.ContainsKey(key))
                {
                    dict[key] = mergedDict[key];
                }
            }
        }
    }

    /// <summary>
    /// reads the client configuration off a specified API endpoint and returns it in a form of js script
    /// </summary>
    public partial class WebClientConfigurationHelper
    {
        public static async Task GetConfigurationScript(HttpContext context)
        {
            //Grab the api endpoints; this is where the MapHive services are; this lets the client call the appropriate backend(s)
            var apiEndPoints = JsonConvert.DeserializeObject<Dictionary<string, string>>(
                ConfigurationManager.AppSettings["ApiEndPoints"]);
            
            var cfg = new Dictionary<string, object>
            {
                {"ApiEndPoints", apiEndPoints}
            };

            //API re-map - customisation of the default api endpoints declared in the MapHive ExtJs
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


            RestClient client;
            RestRequest request;


            //pull the core web client configuration
            if (apiEndPoints.ContainsKey("mhApi"))
            {
                client = new RestClient(apiEndPoints["mhApi"]);
                request = new RestRequest("configuration/webclient", Method.GET)
                {
                    JsonSerializer = new RestSharp.Serializers.NewtonsoftJsonSerializer()
                };

                //read the web client cfg off the api
                cfg.Merge((await client.ExecuteTaskAsync<Dictionary<string, object>>(request)).Data);
            }

            //try to work out waht is the request lang so can pull the client localisation
            var langParam = cfg.ContainsKey("langParam") ? (string)cfg["langParam"] : string.Empty;
            var mapHiveCookieName = cfg.ContainsKey("mapHiveCookieName") ? (string)cfg["mapHiveCookieName"] : string.Empty;
            var cookieValidSeconds = cfg.ContainsKey("cookieValidSeconds") ? (int)(long)cfg["cookieValidSeconds"] : 60 * 60 * 24 * 365; //make it a year or so if not present

            //Note:
            //try to obtain the lang of a couple of places:
            //first test the url and a presence of a lng param. If present, it MUST override whatever could have been saved in a cookie before
            //if no url lng specifier is present, consult a cookie
            //finally, if no data in a cookie is present, check the lang defined in the request

            //url test
            var lng = context.Request.Params[langParam];

            //now it's the cookie time
            if (string.IsNullOrEmpty(lng))
            {
                lng = context.Request.GetCookieValue(mapHiveCookieName, langParam);
            }

            //uhuh, no lang detected so far. inspect the request
            if (string.IsNullOrEmpty(lng))
            {
                if (context.Request.UserLanguages?.Length > 0)
                {
                    lng = context.Request.UserLanguages[0].Substring(0, 2).ToLower();
                }
            }

            //make sure the language defaults to whatever is set if it has not been worked out or is not supported
            var supportedLangCodes = cfg.ContainsKey("supportedLangCodes")
                ? JsonConvert.DeserializeObject< string[]>(JsonConvert.SerializeObject(cfg["supportedLangCodes"]))
                : new string[] {};

            var defaultLangCode = cfg.ContainsKey("defaultLangCode") ? (string)cfg["defaultLangCode"] : string.Empty;
                
            if (string.IsNullOrEmpty(lng) || !(supportedLangCodes).Contains(lng))
            {
                lng = defaultLangCode;
            }

            cfg["langCode"] = lng;

            //finally returning set the lang in cookie for further usage
            context.Response?.SetCookieValue(mapHiveCookieName, langParam, lng, true, cookieValidSeconds);

            //at this stage should be ready to obtain the apps localisations....
            client = new RestClient(apiEndPoints["mhApi"]);
            request = new RestRequest("applocalisations/localiseit", Method.GET)
            {
                JsonSerializer = new RestSharp.Serializers.NewtonsoftJsonSerializer()
            };

            request.AddQueryParameter("appIdentifiers", ConfigurationManager.AppSettings["AppIdentifiers"]);
            request.AddQueryParameter("langCodes", lng);


            //applocalisations/localiseit
            var localisation = (await client.ExecuteTaskAsync<Dictionary<string, Dictionary<string, Dictionary<string, string>>>>(request)).Data;

            //Note:
            //In general camelised property names are expected on the clientside, and this is achieved with
            //new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() }
            //
            //there is a little problem with that though - localisation is a dict, therefore it will get camelised when serialising. And this is NOT
            //a good thing here, as localisation keys are class names and they should be maintained...
            //so basically for localisations need to use the default contract resolver
            var script =
                $"var __mhcfg__={JsonConvert.SerializeObject(cfg, Formatting.None, new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() })}" +
                Environment.NewLine +
                $"var __mhlocalisation__={JsonConvert.SerializeObject(localisation, Formatting.None)};";

            //finally ready to generate the script and write it to the response
            context.Response?.Write(script);

        }
    }
}
