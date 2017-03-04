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
using MapHive.Server.Core.Email;
using MapHive.Server.Core.UserConfiguration;
using MapHive.Server.Core.DataModel;
using MapHive.Server.Core.DAL.DbContext;

namespace MapHive.Server.API.Controllers
{
    [RoutePrefix("configuration")]
    public class ConfigurationController : BaseApiController
    {
        /// <summary>
        /// Gets a base maphive configuration for an authenticated user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("user")]
        [ResponseType(typeof(IDictionary<string, object>))]
        public async Task<IHttpActionResult> GetUserConfiguration()
        {
            try
            {
                return Ok(
                    await UserConfigurationReader.ReadAsync(
                        GetAppUrl(),
                        new MapHiveBasicUserConfiguration<MapHiveDbContext, MapHiveUser>()
                    )
                );
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Gets a webclient configuration for maphive apps. returns stuff like url param name, cookie names, header param names and such
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("webclient")]
        [AllowAnonymous]
        [ResponseType(typeof(IDictionary<string, object>))]
        public async Task<IHttpActionResult> GetWebClientConfiguration()
        {
            try
            {
                return Ok(await WebClientConfiguration.ReadAsync(new MapHiveDbContext("MapHiveMeta")));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

    }
}
