using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using MapHive.Server.Core.API;
using MapHive.Server.Core.API.Serialisation;
using MapHive.Server.Core.DataModel;
using MapHive.Server.Core.DAL.DbContext;
using MapHive.Server.Core.DAL.Interface;

namespace MapHive.Server.API.Controllers
{
    [RoutePrefix("applocalisations")]
    public class AppLocalisationController : BaseApiController
    {
        /// <summary>
        /// Gets an app localisation - all the translations retrieved from a db, for a given app.
        /// </summary>
        /// <param name="langCode"></param>
        /// <param name="appNames"></param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(Dictionary<string, Dictionary<string, Dictionary<string, string>>>))]
        [Route("localiseit")]
        [AllowAnonymous]
        [UnmodifiedDictKeyCasingOutputMethod]
        public async Task<IHttpActionResult> GetAppLocalisation(string langCode, string appNames)
        {
            try
            {
                return
                    Ok(
                        await
                            AppLocalisation.GetAppLocalisationsAsync(new MapHiveDbContext("MapHiveMeta"), langCode,
                                string.IsNullOrWhiteSpace(appNames) ? new string[0] : appNames.Split(',')));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }


        /// <summary>
        /// Gets an app localisation - all the translations retrieved from a db, for a given app.
        /// </summary>
        /// <param name="langCodes"></param>
        /// <param name="appNames"></param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(Dictionary<string, Dictionary<string, Dictionary<string, string>>>))]
        [Route("localiseit")]
        [AllowAnonymous]
        [UnmodifiedDictKeyCasingOutputMethod]
        public async Task<IHttpActionResult> GetAppLocalisations(string langCodes, string appNames)
        {
            try
            {
                return Ok(await AppLocalisation.GetAppLocalisationsAsync(new MapHiveDbContext("MapHiveMeta"), (langCodes ?? string.Empty).Split(','), (appNames ?? string.Empty).Split(',')));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}
