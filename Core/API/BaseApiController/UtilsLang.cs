using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Cartomatic.Utils.Web;
using MapHive.Server.Core.DataModel;
using MapHive.Server.Core.DAL.Interface;

namespace MapHive.Server.Core.API
{
    public abstract partial class BaseApiController<T, TDbCtx>
    {
        /// <summary>
        /// Tries to obtain a language of a request
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetRequestLangCode()
        {
            var request = HttpContext.Current.Request;
            var lng = string.Empty;

            //url test
            foreach (string param in request.Params)
            {
                if (param == WebClientConfiguration.LangParam)
                {
                    lng = request.Params[param];

                    break;
                }
            }

            //now it's the cookie time
            if (string.IsNullOrEmpty(lng))
            {
                lng = request.GetCookieValue(WebClientConfiguration.MapHiveCookieName, WebClientConfiguration.LangParam);
            }

            //uhuh, no lang detected so far. inspect the request and the client langs
            if (string.IsNullOrEmpty(lng))
            {
                if (request.UserLanguages?.Length > 0)
                {
                    lng = request.UserLanguages[0].Substring(0, 2).ToLower();
                }
            }

            return lng;
        }

        /// <summary>
        /// grabs a default language
        /// </summary>
        /// <param name="dbCtx"></param>
        /// <returns></returns>
        public async Task<string> GetDefaultLang(ILocalised dbCtx)
        {
            return (await dbCtx.Langs.FirstOrDefaultAsync(l => l.IsDefault))?.LangCode;
        }
    }
}
