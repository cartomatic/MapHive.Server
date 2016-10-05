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

        // GET: /applications
        [HttpGet]
        [Route("")]
        [ResponseType(typeof(IEnumerable<Application>))]
        public async Task<IHttpActionResult> Get(string sort = null, string filter = null, int start = 0,
            int limit = 25)
        {
            return await base.GetAsync(sort, filter, start, limit);
        }

        // GET: /applications/5
        [HttpGet]
        [ResponseType(typeof(Application))]
        [Route("{uuid}")]
        public async Task<IHttpActionResult> Get(Guid uuid)
        {
            return await base.GetAsync(uuid);
        }

        // PUT: /applications/5
        [HttpPut]
        [Route("{uuid}")]
        [ResponseType(typeof(Application))]
        public async Task<IHttpActionResult> Put(Application obj, Guid uuid)
        {
            return await base.PutAsync(obj, uuid);
        }

        // POST: /applications
        [HttpPost]
        [Route("")]
        [ResponseType(typeof(Application))]
        public async Task<IHttpActionResult> Post(Application obj)
        {
            return await base.PostAsync(obj);
        }

        // DELETE: /applications/5
        [HttpDelete]
        [Route("{uuid}")]
        [ResponseType(typeof(Application))]
        public async Task<IHttpActionResult> Delete(Guid uuid)
        {
            return await base.DeleteAsync(uuid);
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
        /// Returns applications available to the current user; does not require auth, and for guests return a list of common apps.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("userapps")]
        [ResponseType(typeof(IEnumerable<Application>))]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetUserApps()
        {
            try
            {
                return Ok(await Application.GetCommonAppsAsync(_dbCtx));


                //TODO - decide what should be returned. need more thinking on the platform shape first though!

                //if (User.Identity.IsAuthenticated)
                //{
                //    //TODO - this will require testing for the actual read access based in a role
                //    return await base.GetAsync();
                //}
                //else
                //{
                //    //get just the common apps a user can see
                //    return Ok(await Application.GetCommonAppsAsync(_dbCtx));
                //}
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}
