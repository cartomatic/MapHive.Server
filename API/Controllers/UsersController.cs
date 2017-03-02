﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using MapHive.Identity.MembershipReboot;
using MapHive.Server.Core.API;
using MapHive.Server.Core.API.Filters;
using MapHive.Server.Core.DataModel;
using MapHive.Server.Core.DAL.DbContext;
using MapHive.Server.Core.DAL.Interface;
using MapHive.Server.Core.Email;

namespace MapHive.Server.API.Controllers
{
    [RoutePrefix("users")]
    public class UsersController : BaseApiCrudController<MapHiveUser, MapHiveDbContext>
    {
        //this customises the connection string the db context gets instantiated with
        public UsersController()
            : base("MapHiveMeta")
        {
        }

        // GET: /users
        [HttpGet]
        [Route("")]
        [ResponseType(typeof(IEnumerable<MapHiveUser>))]
        public async Task<IHttpActionResult> Get(string sort = null, string filter = null, int start = 0,
            int limit = 25)
        {
            return await base.GetAsync(sort, filter, start, limit);
        }

        // GET: /users/5
        [HttpGet]
        [ResponseType(typeof(MapHiveUser))]
        [Route("{uuid}")]
        public async Task<IHttpActionResult> Get(Guid uuid)
        {
            return await base.GetAsync(uuid);
        }

        // PUT: /users/5
        [HttpPut]
        [Route("{uuid}")]
        [ResponseType(typeof(MapHiveUser))]
        public async Task<IHttpActionResult> Put(MapHiveUser obj, Guid uuid)
        {
            try
            {
                var entity = await obj.UpdateAsync<MapHiveUser, CustomUserAccount>(_dbCtx, CustomUserAccountService.GetInstance("MapHiveMbr"), uuid);

                if (entity != null)
                    return Ok(entity);

                return NotFound();

            }
            catch (Exception ex)
            {
                return this.HandleException(ex);
            }
        }

        // POST: /users
        [HttpPost]
        [Route("")]
        [ResponseType(typeof(MapHiveUser))]
        public async Task<IHttpActionResult> Post(MapHiveUser obj)
        {
            return await HandleUserCreate(obj);
        }

        // POST: /users
        [AllowAnonymous]
        [ImpersonateGhostUser]
        [HttpPost]
        [Route("account")]
        [ResponseType(typeof(MapHiveUser))]
        public async Task<IHttpActionResult> CreateAccount(MapHiveUser obj)
        {
            //Note: this is the same as a default post. the difference is it must be accessible anonymously
            //Ghost user is impersonated in the ImpersonateGhostUserAttribute
            return await HandleUserCreate(obj);
        }

        /// <summary>
        /// Handles user creation procedure
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task<IHttpActionResult> HandleUserCreate(MapHiveUser user)
        {
            try
            {
                var createdUser = await Utils.User.CreateUser(_dbCtx, user,
                    await GetEmailStuffAsync("user_created", _dbCtx as ILocalised), GetRequestSource().Split('#')[0]);

                if (createdUser != null)
                    return Ok(createdUser);

                return NotFound();
            }
            catch (Exception ex)
            {
                return this.HandleException(ex);
            }
        }

        // DELETE: /users/5
        [HttpDelete]
        [Route("{uuid}")]
        [ResponseType(typeof(MapHiveUser))]
        public async Task<IHttpActionResult> Delete(Guid uuid)
        {
            //all stuff is instance based, so need to obtain one first
            var obj = new MapHiveUser();

            try
            {
                obj = await obj.DestroyAsync(_dbCtx, CustomUserAccountService.GetInstance("MapHiveMbr"), uuid);

                if (obj != null)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return this.HandleException(ex);
            }
        }

        /// <summary>
        /// Returns details of an authenticated user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(MapHiveUser))]
        [Route("owndetails")]
        public async Task<IHttpActionResult> GetOwnDetails()
        {
            var uuid = MapHive.Server.Core.Utils.Identity.GetUserGuid();

            //this should not happen really as otherwise the user would not be authenticated
            if (!uuid.HasValue)
            {
                return BadRequest();
            }

            try
            {
                var user = await (_dbCtx).Set<MapHiveUser>().FirstOrDefaultAsync(u => u.Uuid == uuid.Value);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Returns a list of organisations a user has an access to
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(Organisation))]
        [Route("userorgs")]
        public async Task<IHttpActionResult> GetUserOrgs()
        {
            var uuid = MapHive.Server.Core.Utils.Identity.GetUserGuid();

            //this should not happen really as otherwise the user would not be authenticated
            if (!uuid.HasValue)
            {
                return BadRequest();
            }

            try
            {
                var orgs = await MapHiveUser.GetUserOrganisationsAsync(_dbCtx, uuid.Value);

                if (!orgs.Any())
                {
                    return NotFound();
                }

                return Ok(orgs);
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
                return Ok(await MapHiveUser.GetUserAppsAsync(_dbCtx as MapHiveDbContext, MapHive.Server.Core.Utils.Identity.GetUserGuid(), GetRequestOrgIdentifier()));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}
