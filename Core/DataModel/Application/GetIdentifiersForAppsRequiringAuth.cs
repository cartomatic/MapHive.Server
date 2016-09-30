using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapHive.Server.Core.DataModel
{
    public partial class Application
    {
        /// <summary>
        /// returns a collection of app identifying parts for the apps that require auth;
        /// </summary>
        /// <param name="dbCtx"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<string>> GetIdentifiersForAppsRequiringAuthAsync(DbContext dbCtx)
        {
            var output = new List<string>();

            var apps = await dbCtx.Set<Application>().Where(a => a.RequiresAuth).ToListAsync();

            //Note:
            //depending on scenario - host vs hosted the app is recognised a bit differently.
            //in standalone mode the app is recognised by its url (without the hash)
            //in hostedmode, the app is recognised by its short name or uid, whatever is extracted from the url
            //because the hash is never sent to the server it is not possible to recognise a fact an app requires auth some scenarios
            //Therefore a collection of app identifying strings is returned for the apps that do require auth. These strings are ids, short name and urls

            foreach (var app in apps)
            {
                AddIdentifier(output, app.Uuid.ToString());
                AddIdentifier(output, app.ShortName);
                AddIdentifier(output, app.Urls.Split('|'));
            }

            return output.Count > 0 ? output : null;
        }

        /// <summary>
        /// Adds an app identifier to a collection
        /// </summary>
        /// <param name="outAppIdentifiers"></param>
        /// <param name="appIdentifiers"></param>
        private static void AddIdentifier(ICollection<string> outAppIdentifiers, params string[] appIdentifiers)
        {
            foreach (var appIdentifier in appIdentifiers)
            {
                if (!string.IsNullOrEmpty(appIdentifier))
                {
                    outAppIdentifiers.Add(appIdentifier);
                }
            }
        }
    }
}
