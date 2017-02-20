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

        // GET: /organisations
        [HttpGet]
        [Route("")]
        [ResponseType(typeof(IEnumerable<Organisation>))]
        public async Task<IHttpActionResult> Get(string sort = null, string filter = null, int start = 0,
            int limit = 25)
        {
            return await base.GetAsync(sort, filter, start, limit);
        }

        // GET: /organisations/5
        [HttpGet]
        [ResponseType(typeof(Organisation))]
        [Route("{uuid}")]
        public async Task<IHttpActionResult> Get(Guid uuid)
        {
            return await base.GetAsync(uuid);
        }

        // PUT: /organisations/5
        [HttpPut]
        [Route("{uuid}")]
        [ResponseType(typeof(Organisation))]
        public async Task<IHttpActionResult> Put(Organisation obj, Guid uuid)
        {
            return await base.PutAsync(obj, uuid);
        }

        // POST: /organisations
        [HttpPost]
        [Route("")]
        [ResponseType(typeof(Organisation))]
        public async Task<IHttpActionResult> Post(Organisation obj)
        {
            return await base.PostAsync(obj);
        }

        // DELETE: /organisations/5
        [HttpDelete]
        [Route("{uuid}")]
        [ResponseType(typeof(Organisation))]
        public async Task<IHttpActionResult> Delete(Guid uuid)
        {
            return await base.DeleteAsync(uuid);
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
