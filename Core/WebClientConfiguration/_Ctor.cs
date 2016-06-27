using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapHive.Server.Core
{
    public partial class WebClientConfiguration
    {
        //todo: make AppHashProperties, HashPropertyDelimiter, HashPropertyValueDelimiter web.config based at some point. Or better - overridable! not a high prio though...

        /// <summary>
        /// UrlPart / hash property names; used to pass some data between the apps
        /// </summary>
        public static Dictionary<string, string> AppHashProperties { get; set; } = new Dictionary<string, string>
            {
                { "app", "a" },
                { "route", "r" },
                { "accessToken", "at" },
                { "suppressAppToolbar", "suppress-app-toolbar" },
                { "hosted", "hosted" },
                { "suppressSplash", "suppress-splash" }
            };

        /// <summary>
        /// Hash property delimiters
        /// </summary>
        public static string HashPropertyDelimiter { get; set; } = ";";

        /// <summary>
        /// Hash property value delimiter
        /// </summary>
        public static string HashPropertyValueDelimiter { get; set; } = ":";

        /// <summary>
        /// Name of the settings cookie
        /// </summary>
        public static string MapHiveCookieName { get; set; } = "MapHiveSettings";

        /// <summary>
        /// Cookie lifetime expressed in seconds
        /// </summary>
        public static int CookieValidSeconds { get; set; } = 60 * 60 * 24 * 365; //make it a year or so

        /// <summary>
        /// URL param used to identify language the app should localise itself for
        /// </summary>
        public static string LangParam { get; set; } = "lng";
    }
}
