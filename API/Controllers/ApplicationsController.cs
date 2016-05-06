using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace MapHive.Server.API.Controllers
{
    [Route("applications")]
    public class ApplicationsController : ApiController
    {
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            //TODO - first check if user is known, so can customise output - public apps vs apps per user!
            
            //TODO - for unauthenticated users, when getting the apps to display in the app switcher only return the apps that are public, common or something

            return Ok(MapHive.Server.Core.Application.Read());
        }
    }
}
