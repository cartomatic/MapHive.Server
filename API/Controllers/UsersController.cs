using System;
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
using MapHive.Server.Core.DataModel;
using MapHive.Server.Core.DAL.Interface;
using MapHive.Server.Core.Email;
using MapHive.Server.DataModel;
using MapHive.Server.DataModel.DAL;

namespace MapHive.Server.API.Controllers
{
    [RoutePrefix("users")]
    public class UsersController : BaseApiController<User, MapHiveDbContext>
    {
        //this customises the connection string the db context gets instantiated with
        public UsersController()
            : base("MapHiveMeta")
        {
        }

        // GET: /users
        [HttpGet]
        [Route("")]
        [ResponseType(typeof(IEnumerable<User>))]
        public async Task<IHttpActionResult> Get(string sort = null, string filter = null, int start = 0,
            int limit = 25)
        {
            return await base.Get(sort, filter, start, limit);
        }

        // GET: /users/5
        [HttpGet]
        [ResponseType(typeof(User))]
        [Route("{uuid}")]
        public async Task<IHttpActionResult> Get(Guid uuid)
        {
            return await base.Get(uuid);
        }

        // PUT: /users/5
        [HttpPut]
        [Route("{uuid}")]
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> Put(User obj, Guid uuid)
        {
            try
            {
                var entity = await obj.Update<User, CustomUserAccount>(_dbCtx, CustomUserAccountService.GetInstance("MapHiveMbr"), uuid);

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
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> Post(User obj)
        {
            //Note: there are 2 options to send emails when creation a user account:
            //1. listen to UserCreated evt on the User object and then process email manually
            //2. grab the appropriate email template and email account, potentially adjust some email template tokens prior to creating a user and pass both sender account and email template to a user creation procedure

            //In this scenario a second approach is used

            try
            {
                var emailStuff = await GetEmailStuff("user_createdx", _dbCtx as ILocalised);

                //TODO - some email customisation. logon url and such. or maybe should obtain login url from a referrer or an xtra param??????


                var entity = await obj.Create(_dbCtx, CustomUserAccountService.GetInstance("MapHiveMbr"), emailStuff?.Item1, emailStuff?.Item2);

                if (entity != null)
                    return Ok(entity);

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
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> Delete(Guid uuid)
        {
            //all stuff is instance based, so need to obtain one first
            var obj = new User();

            try
            {
                obj = await obj.Destroy(_dbCtx, CustomUserAccountService.GetInstance("MapHiveMbr"), uuid);

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
    }
}
