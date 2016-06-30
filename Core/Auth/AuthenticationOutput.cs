using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityModel.Client;

namespace MapHive.Server.Core
{
    public partial class Auth
    {
        public class AuthOutput
        {
            /// <summary>
            /// whether or not the auth request was successful
            /// </summary>
            public bool Success { get; set; }

            /// <summary>
            /// Access token
            /// </summary>
            public string AccessToken { get; set; }

            /// <summary>
            /// Refresh token
            /// </summary>
            public string RefreshToken { get; set; }

            /// <summary>
            /// Access token expiration coordinated universal time (UTC)
            /// </summary>
            public DateTime? AccessTOkenExpirationTimeUtc { get; set; }

            public static AuthOutput FromTokenResponse(TokenResponse token)
            {
                if(token == null)
                    return new AuthOutput();

                return new AuthOutput()
                {
                    Success = !token.IsError,
                    AccessToken = token.AccessToken,
                    RefreshToken = token.RefreshToken,
                    AccessTOkenExpirationTimeUtc = DateTime.Now.AddSeconds(token.ExpiresIn).ToUniversalTime()
                };
            }
        }
    }
}
