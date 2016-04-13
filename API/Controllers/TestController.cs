using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace MapHive.Server.API.Controllers
{
    public class TestController : ApiController
    {
        /// <summary>
        /// No auth access test
        /// </summary>
        /// <returns></returns>
        [Route("test/noauth")]
        [HttpGet]
        public string AllowNonAuthorised()
        {
            return "Non-authorised access allowed";
        }

        /// <summary>
        /// auth access test
        /// </summary>
        /// <returns></returns>
        [Route("test/auth")]
        [HttpGet]
        [Authorize]
        public string PreventNonAuthorised()
        {
            var claimsPrincipal = User as ClaimsPrincipal;

            return "Only authorised access allowed";
        }
    }
}
