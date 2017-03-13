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
    [RoutePrefix("organisations/{" + OrganisationContextAttribute.OrgIdPropertyName + "}/applications")]
    public class ApplicationsOrgController : BaseApiOrganisatinCrudController<Application, MapHiveDbContext>
    {
        //this customises the connection string the db context gets instantiated with
        public ApplicationsOrgController()
            : base("MapHiveMeta")
        {
        }

        /// <summary>
        /// Reads applications accessible for an organisation
        /// </summary>
        /// <param name="organisationId"></param>
        /// <param name="sort"></param>
        /// <param name="filter"></param>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("linkable")]
        [ResponseType(typeof(IEnumerable<Team>))]
        public async Task<IHttpActionResult> Get(Guid organisationId, string sort = null, string filter = null,
            int start = 0,
            int limit = 25)
        {
            try
            {
                var apps = await OrganisationContext.GetOrganisationLinkableApps(_dbCtx, sort, filter, start, limit);

                if (apps != null)
                {
                    AppendTotalHeader(apps.Item2);
                    return Ok(apps.Item1);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return this.HandleException(ex);
            }
        }

        
    }
}