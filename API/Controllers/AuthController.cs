using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.ModelBinding.Binders;
using MapHive.Identity.MembershipReboot;
using MapHive.Server.Core;
using MapHive.Server.Core.API;
using MapHive.Server.Core.DataModel;
using MapHive.Server.Core.DAL.DbContext;
using MapHive.Server.Core.Email;

namespace MapHive.Server.API.Controllers
{
    [RoutePrefix("auth")]
    public class AuthController : BaseApiController
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
            return Ok(await Auth.LetMeInAsync(email, pass));
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

            await Auth.LetMeOutOfHereAsync(accessToken);
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
            var tokenValidationOutput = await Auth.ValidateTokenAsync(accessToken);
            if (tokenValidationOutput.Success)
            {
                return Ok(tokenValidationOutput);
            }
            else
            {
                return BadRequest();
            }
        }


        /// <summary>
        /// DTO object needed to take data in put body
        /// </summary>
        public class AccountActivationInput
        {
            public string VerificationKey { get; set; }
            public string InitialPassword { get; set; }
        }

        /// <summary>
        /// account activation handler
        /// </summary>
        /// <param name="activationInput"></param>
        /// <returns></returns>
        [Route("accountactivation/{appCtx?}")]
        [HttpPut]
        [AllowAnonymous]
        [ResponseType(typeof(Auth.AccountActivationOutput))]
        public async Task<IHttpActionResult> ActivateAccount(AccountActivationInput activationInput, string appCtx = null)
        {
            try
            {
                var activationOutput =
                    await
                        Auth.ActivateAccountAsync(CustomUserAccountService.GetInstance("MapHiveMbr"),
                            activationInput.VerificationKey, activationInput.InitialPassword);


                //need to resend email with new verification key, as the previous one was stale
                if (activationOutput.VerificationKeyStale)
                {
                    var dbCtx = new MapHiveDbContext("MapHiveMeta");
                    var emailStuff = await GetEmailStuffAsync("activate_account_stale", appCtx, dbCtx);

                    //basically need to send an email the verification key has expired and send a new one
                    var user = await dbCtx.Users.Where(u => u.Email == activationOutput.Email).FirstOrDefaultAsync();

                    //since got an email off mbr, user should not be null, but just in a case...
                    if (user == null)
                    {
                        return BadRequest();
                    }

                    //prepare the email template tokens
                    var tokens = new Dictionary<string, object>
                    {
                        {"UserName", $"{user.GetFullUserName()} ({user.Email})"},
                        {"Email", user.Email},
                        {"RedirectUrl", this.GetRequestSource().Split('#')[0]},
                        {"VerificationKey", activationOutput.VerificationKey},
                        {"InitialPassword", ""}
                    };

                    //prepare and send the email
                    EmailSender.Send(emailStuff.Item1, emailStuff.Item2.Prepare(tokens), user.Email);
                }

                //mbr has not found a user, so bad, bad, bad request it was
                if (activationOutput.UnknownUser)
                {
                    return BadRequest();
                }

                //wipe out some potentially sensitive data
                activationOutput.Email = null;
                activationOutput.VerificationKey = null;

                return Ok(activationOutput);

            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// pass reset request input dto
        /// </summary>
        public class PassResetRequestInput
        {
            public string Email { get; set; }
        }

        /// <summary>
        /// pass reset request handler
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("passresetrequest/{appCtx?}")]
        [HttpPut]
        [AllowAnonymous]
        [ResponseType(typeof(Auth.AccountActivationOutput))]
        public async Task<IHttpActionResult> PassResetRequest(PassResetRequestInput input, string appCtx = null)
        {
            //Note: basically this is a pass reset request, so NO need to inform a potential attacker about exceptions - always return ok!

            try
            {
                var requestPassResetOutput =
                    await Auth.RequestPassResetAsync(CustomUserAccountService.GetInstance("MapHiveMbr"), input.Email);

                var dbCtx = new MapHiveDbContext("MapHiveMeta");
                var emailStuff = await GetEmailStuffAsync("pass_reset_request", appCtx, dbCtx);

                //basically need to send an email the verification key has expired and send a new one
                var user = await dbCtx.Users.Where(u => u.Email == input.Email).FirstOrDefaultAsync();

                //since got here and email off mbr, user should not be null, but just in a case...
                if (user == null)
                {
                    //return BadRequest();
                    return Ok();
                }

                //prepare the email template tokens
                var tokens = new Dictionary<string, object>
                    {
                        {"UserName", $"{user.GetFullUserName()} ({user.Email})"},
                        {"Email", user.Email},
                        {"RedirectUrl", this.GetRequestSource().Split('#')[0]},
                        {"VerificationKey", requestPassResetOutput.VerificationKey}
                    };

                //prepare and send the email
                EmailSender.Send(emailStuff.Item1, emailStuff.Item2.Prepare(tokens), user.Email);

                return Ok();
            }
            catch (Exception ex)
            {
                //return HandleException(ex);
                return Ok();
            }
        }

        /// <summary>
        /// pass reset input dto
        /// </summary>
        public class PassResetInput
        {
            public string NewPass { get; set; }
            public string VerificationKey { get; set; }
        }

        /// <summary>
        /// Resets user password
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("resetpass")]
        [HttpPut]
        [AllowAnonymous]
        public async Task<IHttpActionResult> ChangePasswordFromResetKey(PassResetInput input)
        {
            //Note: basically this is a pass reset request, so NO need to inform a potential attacker about exceptions - always return ok!

            try
            {
                var resetPassSuccess =
                    await Auth.ChangePasswordFromResetKeyAsync(CustomUserAccountService.GetInstance("MapHiveMbr"), input.NewPass, input.VerificationKey);

                //Note: if handle to get here, then pass should be reset

                return Ok(resetPassSuccess);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// pass change input dto
        /// </summary>
        public class ChangePassInput
        {
            public string OldPass { get; set; }
            public string NewPass { get; set; }
        }


        /// <summary>
        /// Changes user password
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("changepass")]
        [HttpPut]
        [ResponseType(typeof(Auth.ChangePassOutput))]
        public async Task<IHttpActionResult> ChangePassword(ChangePassInput input)
        {
            //Note: basically this is a pass reset request, so NO need to inform a potential attacker about exceptions - always return ok!

            try
            {
                var resetPassSuccess =
                    await Auth.ChangePasswordAsync(CustomUserAccountService.GetInstance("MapHiveMbr"), input.NewPass, input.OldPass);

                return Ok(resetPassSuccess);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}
