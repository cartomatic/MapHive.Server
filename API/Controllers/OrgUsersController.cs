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

        // GET: /users
        [HttpGet]
        [Route("")]
        [ResponseType(typeof(IEnumerable<MapHiveUser>))]
        public async Task<IHttpActionResult> Get(string sort = null, string filter = null, int start = 0,
            int limit = 25)
        {
            return await GetOrganisationAssets<MapHiveUser>(sort, filter, start, limit);
        }

        // GET: /users/5
        [HttpGet]
        [ResponseType(typeof(MapHiveUser))]
        [Route("{uuid}")]
        public async Task<IHttpActionResult> Get(Guid uuid)
        {
            return await GetOrganisationAsset<MapHiveUser>(uuid);
        }
    }
}
