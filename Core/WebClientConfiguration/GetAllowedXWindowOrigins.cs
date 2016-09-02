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
        private static async Task<IEnumerable<string>> GetAllowedXWindowOriginsAsync()
        {
            var client = new RestClient(GetApiEndpoint());
            var request = new RestRequest(GetApiCallUrl("allowedxwindoworigins"), Method.GET);

            var x = client.Execute(request);

            return (await client.ExecuteTaskAsync<List<string>>(request)).Data ?? new List<string>();
        }
    }
}
