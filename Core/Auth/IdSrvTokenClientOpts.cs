using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MapHive.Server.Core
{
    public partial class Auth
    {
        public class IdSrvTokenClientOpts
        {
            /// <summary>
            /// IdSrv authority - where the server is located
            /// </summary>
            public string Authority { get; set; }

            /// <summary>
            /// Client id
            /// </summary>
            public string ClientId { get; set; }

            /// <summary>
            /// Client secret
            /// </summary>
            public string ClientSecret { get; set; }

            /// <summary>
            /// Scopes required when authenticating a user
            /// </summary>
            public string RequiredScopes { get; set; }

            /// <summary>
            /// Default ctor needed for serialisation
            /// </summary>
            public IdSrvTokenClientOpts()
            {
            }


            /// <summary>
            /// Extracts IdSrvTokenClientOpts from app settings via using the specified key; vakue of this key should be a json serialised IdSrvTokenClientOpts object
            /// </summary>
            /// <param name="cfgKey"></param>
            /// <param name="silent">Whether or not the constructor should throw if unable to deserialise opts object</param>
            public IdSrvTokenClientOpts(string cfgKey, bool silent = true)
            {
                IdSrvTokenClientOpts idSrvTokenClientOpts = null;
                try
                {
                    idSrvTokenClientOpts =
                        JsonConvert.DeserializeObject<IdSrvTokenClientOpts>(ConfigurationManager.AppSettings[cfgKey]);
                }
                catch (Exception ex)
                {
                    if (!silent)
                        throw;
                }

                Authority = idSrvTokenClientOpts?.Authority;
                ClientId = idSrvTokenClientOpts?.ClientId;
                ClientSecret = idSrvTokenClientOpts?.ClientSecret;
                RequiredScopes = idSrvTokenClientOpts?.RequiredScopes;
            }

            /// <summary>
            /// Creates an instance using the cfg supplied as a dictionary; dictionary keys should be equivalent to the oject properties
            /// </summary>
            /// <param name="cfg"></param>
            public IdSrvTokenClientOpts(Dictionary<string, string> cfg)
            {
                Authority = cfg.ContainsKey(nameof(Authority)) ? cfg[nameof(Authority)] : null;
                ClientId = cfg.ContainsKey(nameof(ClientId)) ? cfg[nameof(ClientId)] : null;
                ClientSecret = cfg.ContainsKey(nameof(ClientSecret)) ? cfg[nameof(ClientSecret)] : null;
                RequiredScopes = cfg.ContainsKey(nameof(RequiredScopes)) ? cfg[nameof(RequiredScopes)] : null;
            }

            /// <summary>
            /// Expects the IdSrvTokenClient configuration to be available via app settings under a "IdSrvTokenClientOpts" key; value of this key should be 
            /// a json serialised IdSrvTokenClientOpts object
            /// </summary>
            /// <param name="silent"></param>
            /// <returns></returns>
            public static IdSrvTokenClientOpts InitDefault(bool silent = true)
            {
                return new IdSrvTokenClientOpts("IdSrvTokenClientOpts", silent);
            }
        }
    }
}
