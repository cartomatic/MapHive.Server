using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapHive.Server.Core
{
    public partial class InitialCfg
    {
        /// <summary>
        /// Maphive API endpoint
        /// </summary>
        public string MhApiEndPoint { get; set; }

        /// <summary>
        /// Allowed origins for the XWindow communication
        /// </summary>
        public IEnumerable<string> AllowedXWindowMsgBusOrigins { get; set; }

        /// <summary>
        /// Whether or not the application should prompt a user with auth UI straight away
        /// </summary>
        public bool RequiresAuth { get; set; }

        /// <summary>
        /// UrlPart / hash property names; used to pass some data between the apps
        /// </summary>
        public Dictionary<string, string> AppHashProperties { get; set; } = new Dictionary<string, string>
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
        public string HashPropertyDelimiter { get; set; } = ";";

        /// <summary>
        /// Hash property value delimiter
        /// </summary>
        public string HashPropertyValueDelimiter { get; set; } = ":";
    }
}
