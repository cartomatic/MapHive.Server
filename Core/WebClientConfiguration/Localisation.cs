using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace MapHive.Server.Core
{
    public partial class WebClientConfiguration
    {
        /// <summary>
        /// Pokes the configured API to obtain app localisation data
        /// </summary>
        /// <param name="appNames">Comma delimited names of the apps / packages to obtain the localisations for</param>
        /// <param name="langCode"></param>
        /// <returns></returns>
        private static async Task<Dictionary<string, Dictionary<string, Dictionary<string, string>>>> GetAppLocalisationAsync(string appNames, string langCode)
        {
            var client = new RestClient(GetApiEndpoint());
            var request = new RestRequest(GetApiCallUrl("applocalisations"), Method.GET);

            request.AddQueryParameter("appNames", appNames);
            request.AddQueryParameter("langCode", langCode);

            return (await client.ExecuteTaskAsync<Dictionary<string, Dictionary<string, Dictionary<string, string>>>>(request)).Data;
        }
    }
}
