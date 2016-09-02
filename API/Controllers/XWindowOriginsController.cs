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
    [RoutePrefix("xwindoworigins")]
    public class XWindowOriginsController : BaseApiCrudController<XWindowOrigin, MapHiveDbContext>
    {
        //this customises the connection string the db context gets instantiated with
        public XWindowOriginsController()
            : base("MapHiveMeta")
        {
        }

        // GET: /xwindoworigins
        [HttpGet]
        [Route("")]
        [ResponseType(typeof(IEnumerable<XWindowOrigin>))]
        public async Task<IHttpActionResult> Get(string sort = null, string filter = null, int start = 0,
            int limit = 25)
        {
            return await base.GetAsync(sort, filter, start, limit);
        }

        // GET: /xwindoworigins/5
        [HttpGet]
        [ResponseType(typeof(XWindowOrigin))]
        [Route("{uuid}")]
        public async Task<IHttpActionResult> Get(Guid uuid)
        {
            return await base.GetAsync(uuid);
        }

        // PUT: /xwindoworigins/5
        [HttpPut]
        [Route("{uuid}")]
        [ResponseType(typeof(XWindowOrigin))]
        public async Task<IHttpActionResult> Put(XWindowOrigin obj, Guid uuid)
        {
            return await base.PutAsync(obj, uuid);
        }

        // POST: /xwindoworigins
        [HttpPost]
        [Route("")]
        [ResponseType(typeof(XWindowOrigin))]
        public async Task<IHttpActionResult> Post(XWindowOrigin obj)
        {
            return await base.PostAsync(obj);
        }

        // DELETE: /xwindoworigins/5
        [HttpDelete]
        [Route("{uuid}")]
        [ResponseType(typeof(XWindowOrigin))]
        public async Task<IHttpActionResult> Delete(Guid uuid)
        {
            return await base.DeleteAsync(uuid);
        }

        /// <summary>
        /// Gets supported langs
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(IEnumerable<XWindowOrigin>))]
        [Route("allowed")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetSupportedLangs()
        {
            try
            {
                return Ok(await XWindowOrigin.GetAllowedXWindowOriginsAsync(_dbCtx as MapHiveDbContext));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}
