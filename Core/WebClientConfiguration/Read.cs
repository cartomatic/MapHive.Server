using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapHive.Server.Core.DataModel;
using MapHive.Server.Core.DAL.DbContext;
using MapHive.Server.Core.DAL.Interface;
using Newtonsoft.Json;

namespace MapHive.Server.Core
{
    public partial class WebClientConfiguration
    {
        /// <summary>
        /// Reads web client confoguration - cookie names, header names, supported langs, app identifiers, xwindow origins and such
        /// </summary>
        /// <param name="dbCtx"></param>
        /// <remarks>See the comments on the locallisation property serialisation issues and reason the localisation is returned as JSON string</remarks>
        /// <returns></returns>
        public static async Task<Dictionary<string, object>> ReadAsync<T>(T dbCtx)
            where T: DbContext, ILocalised
        {
            var cfg = new Dictionary<string, object>();

            //output the app hash properties
            cfg[nameof(AppHashProperties)] = AppHashProperties;
            cfg[nameof(HashPropertyDelimiter)] = HashPropertyDelimiter;
            cfg[nameof(HashPropertyValueDelimiter)] = HashPropertyValueDelimiter;

            //do an 'auth preflight', so user gets better experience - app will prompt for authentication straight away.
            cfg["AuthRequiredAppIdentifiers"] = await Application.GetIdentifiersForAppsRequiringAuthAsync(dbCtx);

            //Allowed origins for the xwindow post message communication
            var xWinDbctx = dbCtx as IXWindow;
            if (xWinDbctx != null)
            {
                cfg["AllowedXWindowMsgBusOrigins"] = await XWindowOrigin.GetAllowedXWindowOriginsAsync(xWinDbctx);
            }
            
            
            cfg["SupportedLangCodes"] = (await Lang.GetSupportedLangsAsync(dbCtx)).Select(l => l.LangCode);
            var defaultLangCode = (await Lang.GetDefaultLangAsync(dbCtx))?.LangCode;
            cfg["DefaultLangCode"] = defaultLangCode;

            cfg[nameof(MapHiveCookieName)] = MapHiveCookieName;
            cfg[nameof(CookieValidSeconds)] = CookieValidSeconds;

            cfg[nameof(LangParam)] = LangParam;
            cfg[nameof(HeaderLang)] = HeaderLang;

            //some other xtra headers
            cfg[nameof(HeaderTotal)] = HeaderTotal;
            cfg[nameof(HeaderSource)] = HeaderSource;

            return cfg;
        }
    }
}
