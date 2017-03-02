using System;
using System.Collections;
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
    /// <summary>
    /// Organisation users controller - allows for reading users scoped within an organisation
    /// </summary>
    [RoutePrefix("organisations/{" + OrganisationContextAttribute.OrgIdPropertyName + "}/users")]
    public class OrgUsersController : BaseApiOrganisatinCrudController<MapHiveUser, MapHiveDbContext>
    {
        //this customises the connection string the db context gets instantiated with
        public OrgUsersController()
            : base("MapHiveMeta")
        {
        }

        /// <summary>
        /// returns a list of organisation users
        /// </summary>
        /// <param name="OrganisationId"></param>
        /// <param name="sort"></param>
        /// <param name="filter"></param>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [ResponseType(typeof(IEnumerable<MapHiveUser>))]
        public async Task<IHttpActionResult> Get(Guid OrganisationId, string sort = null, string filter = null, int start = 0,
            int limit = 25)
        {
            return await GetOrganisationAssets<MapHiveUser>(sort, filter, start, limit);
        }

        /// <summary>
        /// gets a user by id
        /// </summary>
        /// <param name="OrganisationId"></param>
        /// <param name="uuid"></param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(MapHiveUser))]
        [Route("{uuid}")]
        public async Task<IHttpActionResult> Get(Guid OrganisationId, Guid uuid)
        {
            return await GetOrganisationAsset<MapHiveUser>(uuid);
        }


        /// <summary>
        /// Creates an OrgUser for the organisation. Links the user to the org with the default org member role
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        [ResponseType(typeof(MapHiveUser))]
        public async Task<IHttpActionResult> Post(Guid OrganisationId, MapHiveUser user)
        {
            //this is an org user, so needs to be flagged as such!
            user.IsOrgUser = true;
            user.ParentOrganisationId = OrganisationId;

            try
            {
                var createdUser = await Utils.User.CreateUser(_dbCtx, user,
                    await GetEmailStuffAsync("user_created", _dbCtx as ILocalised), GetRequestSource().Split('#')[0]);

                if (createdUser != null)
                {
                    //user has been created, so now need to add it to the org with an appropriate role
                    await this.OrganisationContext.AddOrganisationUser(_dbCtx, createdUser, CustomUserAccountService.GetInstance("MapHiveMbr"));

                    return Ok(createdUser);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return this.HandleException(ex);
            }
        }


        /// <summary>
        /// Links a user to an organisation
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("link")]
        [ResponseType(typeof(MapHiveUser))]
        public async Task<IHttpActionResult> Link(Guid OrganisationId, MapHiveUser user)
        {
            try
            {
                await this.OrganisationContext.AddOrganisationUser(_dbCtx, user, CustomUserAccountService.GetInstance("MapHiveMbr"));

                return Ok();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}
