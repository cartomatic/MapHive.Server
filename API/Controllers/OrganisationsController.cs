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
    [RoutePrefix("organisations")]
    public class OrganisationsController : BaseApiCrudController<Organisation, MapHiveDbContext>
    {
        //this customises the connection string the db context gets instantiated with
        public OrganisationsController()
            : base("MapHiveMeta")
        {
        }

        /// <summary>
        /// Gets a collection of Organisations
        /// </summary>
        /// <param name="sort"></param>
        /// <param name="filter"></param>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [ResponseType(typeof(IEnumerable<Organisation>))]
        public async Task<IHttpActionResult> Get(string sort = null, string filter = null, int start = 0,
            int limit = 25)
        {
            return await GetAsync(sort, filter, start, limit);
        }

        /// <summary>
        /// Gets an Organisation key by id
        /// </summary>
        /// <param name="uuid"></param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(Organisation))]
        [Route("{uuid}")]
        public async Task<IHttpActionResult> Get(Guid uuid)
        {
            return await GetAsync(uuid);
        }

        /// <summary>
        /// Updates an Organisation
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="uuid"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{uuid}")]
        [ResponseType(typeof(Organisation))]
        public async Task<IHttpActionResult> Put(Organisation obj, Guid uuid)
        {
            return await PutAsync(obj, uuid);
        }

        /// <summary>
        /// Creates an Organisation
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        [ResponseType(typeof(Organisation))]
        public async Task<IHttpActionResult> Post(Organisation obj)
        {
            return await PostAsync(obj);
        }

        /// <summary>
        /// Deletes an organisation
        /// </summary>
        /// <param name="uuid"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{uuid}")]
        [ResponseType(typeof(Organisation))]
        public async Task<IHttpActionResult> Delete(Guid uuid)
        {
            return await DeleteAsync(uuid);
        }

        

        /// <summary>
        /// Gets x window origins for the xwindow msg bus
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("{orgId}/allowsapplication/{appId}")]
        [ResponseType(typeof(bool))]
        public async Task<IHttpActionResult> Get(Guid orgId, Guid appId)
        {
            try
            {
                return Ok(await Organisation.CanUseApp(_dbCtx as MapHiveDbContext, orgId, appId));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}
