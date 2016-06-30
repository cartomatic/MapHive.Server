using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Validators;
using IdentityModel.Client;

namespace MapHive.Server.Core
{
    public partial class Auth
    {
        /// <summary>
        /// Authenticates user based on his email and password
        /// </summary>
        /// <param name="email"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public static async Task<AuthOutput> LetMeIn(string email, string pass)
        {
            return AuthOutput.FromTokenResponse(
                await AuthenticateUser(email, pass)    
            );
        }

        /// <summary>
        /// Authenticates a user against the IdSrv
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private static async Task<TokenResponse> AuthenticateUser(string email, string password)
        {
            var idSrvTokenClientOpts = IdSrvTokenClientOpts.InitDefault();

            var tokenClient = new TokenClient(
                 $"{idSrvTokenClientOpts.Authority}/connect/token",
                idSrvTokenClientOpts.ClientId,
                idSrvTokenClientOpts.ClientSecret
            );

            try
            {
                return
                    await
                        tokenClient.RequestResourceOwnerPasswordAsync(email, password,
                            idSrvTokenClientOpts.RequiredScopes);
            }
            catch (Exception ex)
            {

                return null;
            }
            
        }
    }
}
