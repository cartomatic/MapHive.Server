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
using MapHive.Server.Core.DAL.DbContext;

namespace MapHive.Server.API.Controllers
{
    [RoutePrefix("applications")]
    public class ApplicationsController : BaseApiCrudController<Application, MapHiveDbContext>
    {
        //this customises the connection string the db context gets instantiated with
        public ApplicationsController()
            : base("MapHiveMeta")
        {
        }

        /// <summary>
        /// Gets a collection of Applications
        /// </summary>
        /// <param name="sort"></param>
        /// <param name="filter"></param>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [ResponseType(typeof(IEnumerable<Application>))]
        public async Task<IHttpActionResult> Get(string sort = null, string filter = null, int start = 0,
            int limit = 25)
        {
            return await GetAsync(sort, filter, start, limit);
        }

        /// <summary>
        /// Gets an Application by id
        /// </summary>
        /// <param name="uuid"></param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(Application))]
        [Route("{uuid}")]
        public async Task<IHttpActionResult> Get(Guid uuid)
        {
            return await GetAsync(uuid);
        }

        /// <summary>
        /// Updates an application
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="uuid"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{uuid}")]
        [ResponseType(typeof(Application))]
        public async Task<IHttpActionResult> Put(Application obj, Guid uuid)
        {
            return await PutAsync(obj, uuid);
        }

        /// <summary>
        /// Creates a new Application
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        [ResponseType(typeof(Application))]
        public async Task<IHttpActionResult> Post(Application obj)
        {
            return await PostAsync(obj);
        }

        /// <summary>
        /// Deletes an application
        /// </summary>
        /// <param name="uuid"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{uuid}")]
        [ResponseType(typeof(Application))]
        public async Task<IHttpActionResult> Delete(Guid uuid)
        {
            return await DeleteAsync(uuid);
        }

        /// <summary>
        /// Gets a list of identifiers of apps that do require authentication (uuids, short names, urls) for the apps 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("authidentifiers")]
        [ResponseType(typeof(IEnumerable<string>))]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetAppsWithAuthRequired()
        {
            try
            {
                return Ok(await Application.GetIdentifiersForAppsRequiringAuthAsync(_dbCtx));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }


        /// <summary>
        /// Gets x window origins for the xwindow msg bus
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("xwindoworigins")]
        [ResponseType(typeof(IEnumerable<string>))]
        public async Task<IHttpActionResult> Get()
        {
            try
            {
                return Ok(await Application.GetAllowedXWindowMsgBusOriginsAsync(_dbCtx as MapHiveDbContext));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}
