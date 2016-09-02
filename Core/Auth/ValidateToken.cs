using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace MapHive.Server.Core
{
    public partial class Auth
    {
        public static async Task<AuthOutput> ValidateTokenAsync(string accessToken)
        {
            var idSrvTokenClientOpts = IdSrvTokenClientOpts.InitDefault();

            //GET /connect/accesstokenvalidation?token=<token>

            var client = new RestClient($"{idSrvTokenClientOpts.Authority}/connect");
            var request = new RestRequest("accesstokenvalidation", Method.GET);
            request.AddQueryParameter("token", accessToken);

            var response = await client.ExecuteTaskAsync<Dictionary<string, string>>(request);

            return new AuthOutput
            {
                Success = response.StatusCode == HttpStatusCode.OK,
                AccessToken = accessToken,

                //Note: accesstokenvalidation returns expiration in seconds since epoch.
                //Code below should give time in UTC 
                AccessTOkenExpirationTimeUtc = new DateTime(1970, 01, 01, 0, 0, 0, DateTimeKind.Utc).AddSeconds(long.Parse(response.Data["exp"]))
            };
        }
    }
}
