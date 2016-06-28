using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using RestSharp;
using Cartomatic.Utils.Web;

namespace MapHive.Server.Core
{
    public partial class WebClientConfiguration
    {
        /// <summary>
        /// Gets a request lang code
        /// </summary>
        /// <param name="setCookie"></param>
        /// <returns></returns>
        public static async Task<string> GetRequestLang(bool setCookie = true)
        {
            return await GetRequestLang(HttpContext.Current.Request, HttpContext.Current.Response, setCookie);
        }

        /// <summary>
        /// gets a request lang code
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <param name="setCookie"></param>
        /// <returns></returns>
        private static async Task<string> GetRequestLang(HttpRequest request, System.Web.HttpResponse response, bool setCookie = true)
        {
            string lng = null;

            //Note:
            //try to obtain the lang of a couple of places:
            //first test the url and a presence of a lng param. If present, it MUST override whatever could have been saved in a cookie before
            //if no url lng specifier is present, consult a cookie
            //finally, if no data in a cookie is present, check the lang defined in the request

            //url test
            foreach (string param in request.Params)
            {
                if (param == LangParam)
                {
                    lng = request.Params[param];

                    break;
                }
            }

            //now it's the cookie time
            if (string.IsNullOrEmpty(lng))
            {
                lng = request.GetCookieValue(MapHiveCookieName, LangParam);
            }

            //uhuh, no lang detected so far. inspect the request
            if (string.IsNullOrEmpty(lng))
            {
                if (request.UserLanguages?.Length > 0)
                {
                    lng = request.UserLanguages[0].Substring(0, 2).ToLower();
                }
            }

            //make sure the language defaults to whatever is set if it has not been worked out or is not supported
            if (string.IsNullOrEmpty(lng) || !(await GetSupportedLangCodes()).Contains(lng))
            {
                lng = await GetDefaultLangCode();
            }

            //finally before returning set the lang in cookie for further usage
            if (setCookie)
            {
                response?.SetCookieValue(MapHiveCookieName, LangParam, lng, true, CookieValidSeconds);
            }

            return lng;
        }

        /// <summary>
        /// Gets a list of supported lang codes of the configured WebAPI
        /// </summary>
        /// <returns></returns>
        private static async Task<IEnumerable<string>> GetSupportedLangCodes()
        {
            var client = new RestClient(GetApiEndpoint());
            var request = new RestRequest(GetApiCallUrl("supportedlangcodes"), Method.GET);

            return (await client.ExecuteTaskAsync<List<string>>(request)).Data;
        }


        /// <summary>
        /// Gets a default lang code off the confogured web api
        /// </summary>
        /// <returns></returns>
        private static async Task<string> GetDefaultLangCode()
        {
            var client = new RestClient(GetApiEndpoint());
            var request = new RestRequest(GetApiCallUrl("defaultlangcode"), Method.GET);

            return (await client.ExecuteTaskAsync<string>(request)).Data;
        }
    }
}
