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
    public class AppLocalisationsController : BaseApiController
    {
        /// <summary>
        /// Gets an app localisation - all the translations retrieved from a db, for a given app.
        /// </summary>
        /// <param name="langCode"></param>
        /// <param name="appIdentifiers"></param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(Dictionary<string, Dictionary<string, Dictionary<string, string>>>))]
        [Route("localiseit")]
        [AllowAnonymous]
        [UnmodifiedDictKeyCasingOutputMethod]
        public async Task<IHttpActionResult> GetAppLocalisation(string langCode, string appIdentifiers)
        {
            try
            {
                return
                    Ok(
                        await
                            AppLocalisation.GetAppLocalisationsAsync(new MapHiveDbContext("MapHiveMeta"), langCode,
                                string.IsNullOrWhiteSpace(appIdentifiers) ? new string[0] : appIdentifiers.Split(',')));
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
        /// <param name="appIdentifiers"></param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(Dictionary<string, Dictionary<string, Dictionary<string, string>>>))]
        [Route("localiseit")]
        [AllowAnonymous]
        [UnmodifiedDictKeyCasingOutputMethod]
        public async Task<IHttpActionResult> GetAppLocalisations(string langCodes, string appIdentifiers)
        {
            try
            {
                return Ok(await AppLocalisation.GetAppLocalisationsAsync(new MapHiveDbContext("MapHiveMeta"), (langCodes ?? string.Empty).Split(','), (appIdentifiers ?? string.Empty).Split(',')));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        public class BulkSaveInput
        {
            public bool? Overwrite { get; set; }
            public string[] LangsToImport { get; set; }
            public LocalisationClass[] AppLocalisations { get; set; }
        }

        [HttpPost]
        [Route("bulksave")]
        [UnmodifiedDictKeyCasingOutputMethod]
        public async Task<IHttpActionResult> BulkSaveAppLocalisations(BulkSaveInput data)
        {
            try
            {
                await
                    AppLocalisation.SaveLocalisations(new MapHiveDbContext("MapHiveMeta"), data.AppLocalisations,
                        data.Overwrite, data.LangsToImport);

                return Ok();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}
