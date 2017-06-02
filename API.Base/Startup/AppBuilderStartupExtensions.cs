using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using IdentityServer3.AccessTokenValidation;
using MapHive.Server.API;
using Microsoft.IdentityModel.Protocols;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Owin;

namespace MapHive.Server.Core.API.Startup
{
    public static class AppBuilderStartupExtensions
    {
        public static void ConfigureMapHiveApi(
            this IAppBuilder app,
            string apiVersion, string apiTitle,
            string xmlCommentsPath
        )
        {
            var idSrvBearerTokenAuthOpts = JsonConvert.DeserializeObject<Dictionary<string, string>>(ConfigurationManager.AppSettings["IdSrvBearerTokenAuthOpts"]);

            // Wire token validation  - comes from Identity3.AccessTokenValidation
            app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            {
                Authority = idSrvBearerTokenAuthOpts["Authority"],

                //adding clientId, secret (scope based!!!!) makes the middleware use the introspection endpoint instead of
                //the validation endpoint

                //For access to the introspection endpoint
                //ClientId = idSrvBearerTokenAuthOpts["ClientId"], //In IdentityServer this is a name of an authorised scope

                //In IdentityServer this is a secret associated with a scope
                //ClientSecret = idSrvBearerTokenAuthOpts["ClientSecret"],


                //scopes that are required - client also needs to have them configured as allowed scopes
                //and also an access token needs to be requested asking specifically for that scope.
                //only if the scope is present in the JWT scope arr, the bearer token auth will allow pass through.
                //only one of the scopes is required - token does not have to have all of them
                RequiredScopes = !string.IsNullOrWhiteSpace(idSrvBearerTokenAuthOpts["RequiredScopes"]) ?
                    idSrvBearerTokenAuthOpts["RequiredScopes"].Split(' ')
                    :
                    new string[0],

                //time the validation result remains in cache before being validated again
                //so there is no need to poke the id server too often!
                //ValidationResultCacheDuration = 
            });


            var config = new HttpConfiguration();
            config.ConfigureSwagger(apiVersion, apiTitle, xmlCommentsPath);

            config.MapHttpAttributeRoutes();

            //newtosoft json formatting! so nicely indented json is returned
            config.Formatters.JsonFormatter.SerializerSettings.Formatting = Formatting.Indented;

            //make the json props be camel case!
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            //add authorise attribute to all requests
            config.Filters.Add(new AuthorizeAttribute());

            var corsCfg =
                JsonConvert.DeserializeObject<Dictionary<string, string>>(ConfigurationManager.AppSettings["CorsCfg"]);

            config.EnableCors(new EnableCorsAttribute(
                origins: corsCfg.ContainsKey("origins") && !string.IsNullOrEmpty(corsCfg["origins"]) ? corsCfg["origins"] : "*",
                headers: corsCfg.ContainsKey("headers") && !string.IsNullOrEmpty(corsCfg["headers"]) ? corsCfg["headers"] : "*",
                methods: corsCfg.ContainsKey("methods") && !string.IsNullOrEmpty(corsCfg["methods"]) ? corsCfg["methods"] : "*"
            ));


            app.UseWebApi(config);


            //finally make sure to register maphivebase type uids for this runtime
            MapHive.Server.Core.DataModel.BaseObjectTypeIdentifierExtensions.AutoRegisterBaseTypes();
        }
    }
}
