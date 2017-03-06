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

namespace MapHive.Server.API.OrgControllers
{
    /// <summary>
    /// Organisation users controller - allows for reading users scoped within an organisation
    /// </summary>
    [RoutePrefix("organisations/{" + OrganisationContextAttribute.OrgIdPropertyName + "}/users")]
    public class UsersOrgController : BaseApiOrganisatinCrudController<MapHiveUser, MapHiveDbContext>
    {
        //this customises the connection string the db context gets instantiated with
        public UsersOrgController()
            : base("MapHiveMeta")
        {
        }

        /// <summary>
        /// returns a list of organisation users
        /// </summary>
        /// <param name="organisationId"></param>
        /// <param name="sort"></param>
        /// <param name="filter"></param>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [ResponseType(typeof(IEnumerable<MapHiveUser>))]
        public async Task<IHttpActionResult> Get(Guid organisationId, string sort = null, string filter = null, int start = 0,
            int limit = 25)
        {
            try
            {
                var users = await OrganisationContext.GetOrganisationAssets<MapHiveUser>(_dbCtx, sort, filter, start, limit);
                if (users == null)
                    return NotFound();

                var roles2users = await OrganisationContext.GetOrgRoles2UsersMap(_dbCtx);

                foreach (var user in users.Item1)
                {
                    user.OrganisationRole = OrganisationContext.GetUserOrgRole(roles2users, user.Uuid);
                }

                AppendTotalHeader(users.Item2);
                return Ok(users.Item1);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
            
        }

        /// <summary>
        /// gets an Organisation User by id
        /// </summary>
        /// <param name="organisationId"></param>
        /// <param name="uuid"></param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(MapHiveUser))]
        [Route("{uuid}")]
        public async Task<IHttpActionResult> Get(Guid organisationId, Guid uuid)
        {
            return await GetAsync(uuid);
        }


        /// <summary>
        /// Creates an OrgUser for the organisation. Links the user to the org with the default org member role
        /// </summary>
        /// <param name="organisationId"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        [ResponseType(typeof(MapHiveUser))]
        public async Task<IHttpActionResult> Post(Guid organisationId, MapHiveUser user)
        {
            //this is an org user, so needs to be flagged as such!
            user.IsOrgUser = true;
            user.ParentOrganisationId = organisationId;

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
        /// <param name="organisationId"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("link")]
        [ResponseType(typeof(MapHiveUser))]
        public async Task<IHttpActionResult> Link(Guid organisationId, MapHiveUser user)
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


        /// <summary>
        /// Updates a user
        /// </summary>
        /// <param name="organisationId"></param>
        /// <param name="user"></param>
        /// <param name="uuid"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{uuid}")]
        [ResponseType(typeof(MapHiveUser))]
        public async Task<IHttpActionResult> Put(Guid organisationId, MapHiveUser user, Guid uuid)
        {
            try
            {
                var entity = await user.UpdateAsync<MapHiveUser, CustomUserAccount>(_dbCtx, CustomUserAccountService.GetInstance("MapHiveMbr"), uuid);

                if (entity != null)
                {
                    //once the user has been updated, need to update its role within an org too
                    await this.OrganisationContext.ChangeOrganisationUserRole(_dbCtx, user, CustomUserAccountService.GetInstance("MapHiveMbr"));

                    return Ok(entity);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return this.HandleException(ex);
            }
        }

        /// <summary>
        /// Removes and external user link from an Organisation
        /// </summary>
        /// <param name="organisationId"></param>
        /// <param name="uuid"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("link/{uuid}")]
        [ResponseType(typeof(MapHiveUser))]
        public async Task<IHttpActionResult> UnLink(Guid organisationId, Guid uuid)
        {
            try
            {
                //get a user an make sure he is not an org user!
                var user = await new MapHiveUser().ReadAsync(_dbCtx, uuid);
                if (
                    user == null || (user.IsOrgUser && user.ParentOrganisationId == organisationId)
                    || user.UserOrgId == organisationId //also avoid removing own org of a user!
                )
                    throw MapHive.Server.Core.DataModel.Validation.Utils.GenerateValidationFailedException(nameof(MapHiveUser), MapHive.Server.Core.DataModel.Validation.ValidationErrors.OrgOwnerDestroyError);
                        


                //not providing a user role will effectively wipe out user assignment
                user.OrganisationRole = null;
                await this.OrganisationContext.ChangeOrganisationUserRole(_dbCtx, user, CustomUserAccountService.GetInstance("MapHiveMbr"));

                OrganisationContext.RemoveLink(user);
                await OrganisationContext.UpdateAsync(_dbCtx);

                return Ok();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}
