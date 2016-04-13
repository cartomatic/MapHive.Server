using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using IdentityModel;
using IdentityModel.Client;
using Newtonsoft.Json;

namespace MapHive.Server.API.Controllers
{
    [RoutePrefix("auth")]
    public class AuthController : ApiController
    {
        [Route("login")]
        [HttpGet]
        [HttpPost]
        public async Task<IHttpActionResult> LogIn(string email, string pass)
        {

            //TODO - first check user agains membership reboot so can customise output

            //Note:
            //looks like MembershipReboot emails / usernames are case sensitive.
            //need to ensure they are lower case when authenticating and creating accounts

            //Note:
            //When required - it will be possible to cut the user token. then the services would pick this up when revalidating it, so likely within
            //a couple of seconds - whatever is configured for the bearer token authorisation

            //and if ok obtain a token
            var token = await GetToken(email, pass);

            return Ok(token);
        }


        /// <summary>
        /// Gets an authentication token for a particular user
        /// </summary>
        /// <param name="username"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        protected async Task<TokenResponse> GetToken(string username, string pass)
        {
            //Note:
            //settings here define the Client set up in the identity server - the token needs to be generated for a particular client to use!
            var idSrvTokenClientOpts = JsonConvert.DeserializeObject<Dictionary<string, string>>(ConfigurationManager.AppSettings["IdSrvTokenClientOpts"]);

            var client = new TokenClient(
               $"{idSrvTokenClientOpts["Authority"]}/connect/token",
               idSrvTokenClientOpts["ClientId"],
               idSrvTokenClientOpts["ClientSecret"]);


            //required scopes - this means get the access token for the required scopes.
            //if scopes are not allowed for the client (client does not have the access to them - see allowed scopes cfg on the client), the token will not be generated
            var result = await client.RequestResourceOwnerPasswordAsync(username, pass, idSrvTokenClientOpts["RequiredScopes"]);

            return result;
        }

    }
}
