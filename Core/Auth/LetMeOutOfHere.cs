using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapHive.Server.Core
{
    public partial class Auth
    {
        /// <summary>
        /// Finalises a session for a particular accessToken
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public static async Task LetMeOutOfHere(string accessToken)
        {
            //TODO
            //https://identityserver.github.io/Documentation/docs/endpoints/endSession.html
            //https://identityserver.github.io/Documentation/docs/configuration/authenticationOptions.html
        }
    }
}
