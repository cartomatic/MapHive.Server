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
        /// returns a collection of app identifying parts for the apps that require auth
        /// </summary>
        /// <param name="appUrl"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetIdentifiersForAppsRequiringAuth()
        {
            var output = new List<string>();

            var apps = Read();

            //Note:
            //depending on scenario - host vs hosted the app is recognised a bit differently.
            //in standalone mode the app is recognised by its url (without the hash)
            //in hostedmode, the app is recognised by its short name or uid, whatever is extracted from the url
            //because the hash is never sent to the server it is not possible to recognise a fact an app requires auth some scenarios
            //Therefore a collection of app identifying strings is returned for the apps that do require auth. These strings are ids, short name and urls

            foreach (var app in apps)
            {
                if (app.RequiresAuth)
                {
                    AddIdentifier(output, app.Id.ToString());
                    AddIdentifier(output, app.ShortName);
                    AddIdentifier(output, app.Url);
                }
            }

            return output.Count > 0 ? output : null;
        }

        protected static void AddIdentifier(IList<string> appIdentifiers, string appIdentifier)
        {
            if (!string.IsNullOrEmpty(appIdentifier))
            {
                appIdentifiers.Add(appIdentifier);
            }
        }
    }
}
