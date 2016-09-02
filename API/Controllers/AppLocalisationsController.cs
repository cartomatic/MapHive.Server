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
using MapHive.Server.Core.DAL.Interface;
using MapHive.Server.DataModel;
using MapHive.Server.DataModel.DAL;

namespace MapHive.Server.API.Controllers
{
    [RoutePrefix("applocalisations")]
    public class AppLocalisationController : BaseApiCrudController<AppLocalisation, MapHiveDbContext>
    {
        //this customises the connection string the db context gets instantiated with
        public AppLocalisationController()
            : base("MapHiveMeta")
        {
        }

        // GET: /applocalisations
        [HttpGet]
        [Route("")]
        [ResponseType(typeof(IEnumerable<AppLocalisation>))]
        public async Task<IHttpActionResult> Get(string sort = null, string filter = null, int start = 0,
            int limit = 25)
        {
            return await base.GetAsync(sort, filter, start, limit);
        }

        // GET: /applocalisations/5
        [HttpGet]
        [ResponseType(typeof(AppLocalisation))]
        [Route("{uuid}")]
        public async Task<IHttpActionResult> Get(Guid uuid)
        {
            return await base.GetAsync(uuid);
        }

        // PUT: /applocalisations/5
        [HttpPut]
        [Route("{uuid}")]
        [ResponseType(typeof(AppLocalisation))]
        public async Task<IHttpActionResult> Put(AppLocalisation obj, Guid uuid)
        {
            return await base.PutAsync(obj, uuid);
        }

        // POST: /applocalisations
        [HttpPost]
        [Route("")]
        [ResponseType(typeof(AppLocalisation))]
        public async Task<IHttpActionResult> Post(AppLocalisation obj)
        {
            return await base.PostAsync(obj);
        }

        // DELETE: /applocalisations/5
        [HttpDelete]
        [Route("{uuid}")]
        [ResponseType(typeof(AppLocalisation))]
        public async Task<IHttpActionResult> Delete(Guid uuid)
        {
            return await base.DeleteAsync(uuid);
        }

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
                            AppLocalisation.GetAppLocalisationsAsync(_dbCtx as MapHiveDbContext, langCode,
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
                return Ok(await AppLocalisation.GetAppLocalisationsAsync(_dbCtx as MapHiveDbContext, (langCodes ?? string.Empty).Split(','), (appNames ?? string.Empty).Split(',')));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}
