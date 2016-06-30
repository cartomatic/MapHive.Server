using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.ModelBinding.Binders;
using MapHive.Server.Core;

namespace MapHive.Server.API.Controllers
{
    [RoutePrefix("auth")]
    public class AuthController : ApiController
    {
        /// <summary>
        /// Authenticates user; output returned, if successful contains access and refresh tokens
        /// </summary>
        /// <param name="email"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("letmein")]
        [AllowAnonymous]
        [ResponseType(typeof (Auth.AuthOutput))]
        public async Task<IHttpActionResult> LetMeIn(
            [FromUri(BinderType = typeof (TypeConverterModelBinder))] string email,
            [FromUri(BinderType = typeof (TypeConverterModelBinder))] string pass
            )
        {
            return Ok(await Auth.LetMeIn(email, pass));
        }

        /// <summary>
        /// Finalises user session on the idsrv
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("letmeoutofhere")]
        [ResponseType(typeof (Auth.AuthOutput))]
        public async Task<IHttpActionResult> LetMeOutOfHere()
        {
            //extract access token off the request
            var accessToken = Request.Headers.Authorization.Parameter.Replace("Bearer ", "");

            await Auth.LetMeOutOfHere(accessToken);
            return Ok();
        }

        /// <summary>
        /// Validates access token
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        [Route("tokenvalidation")]
        [HttpGet]
        [AllowAnonymous]
        [ResponseType(typeof (Auth.AuthOutput))]
        public async Task<IHttpActionResult> ValidateToken(string accessToken)
        {
            var tokenValidationOutput = await Auth.ValidateToken(accessToken);
            if (tokenValidationOutput.Success)
            {
                return Ok(tokenValidationOutput);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
