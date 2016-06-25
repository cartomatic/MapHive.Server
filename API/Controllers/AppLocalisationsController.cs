using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using MapHive.Server.Core.API;
using MapHive.Server.Core.DataModel;
using MapHive.Server.Core.DAL.Interface;
using MapHive.Server.DataModel;
using MapHive.Server.DataModel.DAL;

namespace MapHive.Server.API.Controllers
{
    [RoutePrefix("applocalisations")]
    public class AppLocalisationController : BaseApiController<AppLocalisation, MapHiveDbContext>
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
            return await base.Get(sort, filter, start, limit);
        }

        // GET: /applocalisations/5
        [HttpGet]
        [ResponseType(typeof(AppLocalisation))]
        [Route("{uuid}")]
        public async Task<IHttpActionResult> Get(Guid uuid)
        {
            return await base.Get(uuid);
        }

        // PUT: /applocalisations/5
        [HttpPut]
        [Route("{uuid}")]
        [ResponseType(typeof(AppLocalisation))]
        public async Task<IHttpActionResult> Put(AppLocalisation obj, Guid uuid)
        {
            return await base.Put(obj, uuid);
        }

        // POST: /applocalisations
        [HttpPost]
        [Route("")]
        [ResponseType(typeof(AppLocalisation))]
        public async Task<IHttpActionResult> Post(AppLocalisation obj)
        {
            return await base.Post(obj);
        }

        // DELETE: /applocalisations/5
        [HttpDelete]
        [Route("{uuid}")]
        [ResponseType(typeof(AppLocalisation))]
        public async Task<IHttpActionResult> Delete(Guid uuid)
        {
            return await base.Delete(uuid);
        }

        /// <summary>
        /// Gets a list of applocalisations 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(Dictionary<string, Dictionary<string, Dictionary<string, string>>>))]
        [Route("localiseit")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetAppsWithAuthRequired(string langCode, string appNames)
        {
            try
            {
                return Ok(await AppLocalisation.GetAppLocalisations(_dbCtx as MapHiveDbContext, langCode, (appNames ?? string.Empty).Split(',')));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}
