using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapHive.Server.Core
{
    public partial class Application
    {
        /// <summary>
        /// Checks if a given application requires authentication; 
        /// </summary>
        /// <param name="appUrl"></param>
        /// <returns></returns>
        public static bool RequiresAuth(string appUrl)
        {
            bool requiresAuth = false;

            var apps = Read();

            //Note:
            //depending on scenario - host vs hosted the app is recognised a bit differently.
            //in hosted mode the app is recognised by its url (without the hash)
            //in host mode, the app is recognised by its short name or uid, whatever is extracted from the url



            return requiresAuth;
        }
    }
}
