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
        /// Pokes the configured API to obtain identifiers for the apps that do require authentication
        /// </summary>
        /// <returns></returns>
        private static async Task<IEnumerable<string>> GetIdentifiersForAppsRequiringAuthAsync()
        {
            var client = new RestClient(GetApiEndpoint());
            var request = new RestRequest(GetApiCallUrl("authidentifiers"), Method.GET);

            return (await client.ExecuteTaskAsync<List<string>>(request)).Data;
        }
    }
}
