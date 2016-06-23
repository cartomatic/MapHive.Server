﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using MapHive.Server.Core.API;
using MapHive.Server.DataModel;
using MapHive.Server.DataModel.DAL;

namespace MapHive.Server.API.Controllers
{
    [RoutePrefix("applications")]
    public class ApplicationsController : BaseApiController<Application, MapHiveDbContext>
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
            return await base.Get(sort, filter, start, limit);
        }

        // GET: /applications/5
        [HttpGet]
        [ResponseType(typeof(Application))]
        [Route("{uuid}")]
        public async Task<IHttpActionResult> Get(Guid uuid)
        {
            return await base.Get(uuid);
        }

        // PUT: /applications/5
        [HttpPut]
        [Route("{uuid}")]
        [ResponseType(typeof(Application))]
        public async Task<IHttpActionResult> Put(Application obj, Guid uuid)
        {
            return await base.Put(obj, uuid);
        }

        // POST: /applications
        [HttpPost]
        [Route("")]
        [ResponseType(typeof(Application))]
        public async Task<IHttpActionResult> Post(Application obj)
        {
            return await base.Post(obj);
        }

        // DELETE: /applications/5
        [HttpDelete]
        [Route("{uuid}")]
        [ResponseType(typeof(Application))]
        public async Task<IHttpActionResult> Delete(Guid uuid)
        {
            return await base.Delete(uuid);
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
                return Ok(await Application.GetIdentifiersForAppsRequiringAuth(_dbCtx));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }


        [HttpGet]
        [Route("userapps")]
        [ResponseType(typeof(IEnumerable<Application>))]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetUserApps()
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    //TODO - this will require testing for the actual read access based in a role
                    return await base.Get();
                }
                else
                {
                    //get just the common apps a user can see
                    return Ok(await Application.GetCommonApps(_dbCtx));
                }
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}
