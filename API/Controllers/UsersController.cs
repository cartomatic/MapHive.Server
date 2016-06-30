using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using MapHive.Identity.MembershipReboot;
using MapHive.Server.Core.API;
using MapHive.Server.Core.DataModel;
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
            //TODO - plug in email template read in order to send emails!

            try
            {
                //TODO - wire up an evt listener, so can react to user created evt and send a confirmation email
                //TODO - or maybe make it possible to send emails in the user object???

                var entity = await obj.Create(_dbCtx, CustomUserAccountService.GetInstance("MapHiveMbr"));

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
