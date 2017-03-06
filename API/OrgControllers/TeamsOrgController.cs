using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using MapHive.Server.Core.API;
using MapHive.Server.Core.API.Filters;
using MapHive.Server.Core.DataModel;
using MapHive.Server.Core.DAL.DbContext;

namespace MapHive.Server.API.OrgControllers
{
    [RoutePrefix("organisations/{" + OrganisationContextAttribute.OrgIdPropertyName + "}/teams")]
    public class TeamsOrgController : BaseApiOrganisatinCrudController<Team, MapHiveDbContext>
    {
        //this customises the connection string the db context gets instantiated with
        public TeamsOrgController()
            : base("MapHiveMeta")
        {
        }

        /// <summary>
        /// Reads organisation Teams
        /// </summary>
        /// <param name="organisationId"></param>
        /// <param name="sort"></param>
        /// <param name="filter"></param>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [ResponseType(typeof(IEnumerable<Team>))]
        public async Task<IHttpActionResult> Get(Guid organisationId, string sort = null, string filter = null,
            int start = 0,
            int limit = 25)
        {
            return await GetAsync(sort, filter, start, limit);
        }

        /// <summary>
        /// Gets an organisation team by id
        /// </summary>
        /// <param name="organisationId"></param>
        /// <param name="uuid"></param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(Team))]
        [Route("{uuid}")]
        public async Task<IHttpActionResult> Get(Guid organisationId, Guid uuid)
        {
            return await GetAsync(uuid);
        }

        /// <summary>
        /// Updates an Organisation's Team
        /// </summary>
        /// <param name="organisationId"></param>
        /// <param name="obj"></param>
        /// <param name="uuid"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{uuid}")]
        [ResponseType(typeof(Team))]
        public async Task<IHttpActionResult> Put(Guid organisationId, Team obj, Guid uuid)
        {
            return await PutAsync(obj, uuid);
        }

        /// <summary>
        /// Creates a new Team within an Organisation
        /// </summary>
        /// <param name="organisationId"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        [ResponseType(typeof(Team))]
        public async Task<IHttpActionResult> Post(Guid organisationId, Team obj)
        {
            return await PostAsync(obj);
        }

        /// <summary>
        /// Deletes a Team
        /// </summary>
        /// <param name="organisationId"></param>
        /// <param name="uuid"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{uuid}")]
        [ResponseType(typeof(Team))]
        public async Task<IHttpActionResult> Delete(Guid organisationId, Guid uuid)
        {
            return await DeleteAsync(uuid);
        }
    }
}