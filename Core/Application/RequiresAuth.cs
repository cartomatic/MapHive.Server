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
            //
            //The problem on the serverside though is that the hash is not considered a url and therefore never makes it to the server,
            //so basically in HOST mode, the auth preflight MUST be done on the client.
            //In such case, server can only output some sort of short list of the app short names and uuids that do require auth, and based on that
            //the client can do the auth preflight on its own
            //TODO - implement the above  

            var cfg = new InitialCfg(); //note: so far the hash properties stuff is indeed hardcoded in the cfg object
            var appHashParam = $"{cfg.AppHashProperties["app"]}{cfg.HashPropertyValueDelimiter}";
            var hashAppRecognition = appUrl.IndexOf(appHashParam) > -1;

            if (hashAppRecognition)
            {
                //work out the app short name or uuid
                var urlParts = appUrl.Split('#');
                if (urlParts.Length > 0)
                {
                    var hashParts = urlParts[1].Split(cfg.HashPropertyDelimiter.ToCharArray()[0]);
                    var appParam = hashParts.FirstOrDefault(p => p.StartsWith(appHashParam));
                    if (!string.IsNullOrEmpty(appParam))
                    {
                        var appNameOrIdentifier = appParam.Split(cfg.HashPropertyValueDelimiter.ToCharArray()[0])[1];

                        requiresAuth = apps.FirstOrDefault(a => string.CompareOrdinal(a.ShortName,appNameOrIdentifier) == 0 || string.CompareOrdinal(a.Id.ToString(), appNameOrIdentifier) == 0)?.RequiresAuth ?? false;
                    }
                }
            }
            else
            {
                //just compare raw url without the url part
                var url = appUrl.Split('#')[0];

                requiresAuth = apps.FirstOrDefault(a => a.Url == url)?.RequiresAuth ?? false;
            }

            //Note:
            //if no match found above, then assume initially the auth is not required. A very first call to any protected webservice
            //shoud make the auth procedure kick in anyway


            return requiresAuth;
        }
    }
}
