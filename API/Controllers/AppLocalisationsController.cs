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

        /// <summary>
        /// Bulk localisations save input
        /// </summary>
        public class BulkSaveInput
        {
            public bool? Overwrite { get; set; }
            public string[] LangsToImport { get; set; }
            public LocalisationClass[] AppLocalisations { get; set; }
        }

        /// <summary>
        /// Saves app localisations in bulk
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
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
