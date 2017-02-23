using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using MapHive.Server.Core.DAL.DbContext;

namespace MapHive.Server.Core.API.Filters
{
    public class OrganisationContextAttribute : ActionFilterAttribute
    {
        public const string OrgCtxPropertyName = "OrganisationContext";

        public const string OrgIdPropertyName = "OrganisationId";


        /// <summary>
        /// Provides organisation context for a request
        /// </summary>
        /// <param name="actionContext"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            var orgId = (Guid) actionContext.ActionArguments["organisationId"];
            actionContext.Request.Properties.Add(OrgIdPropertyName, orgId);

            using (var dbCtx = new MapHiveDbContext())
            {
                actionContext.Request.Properties.Add(OrgCtxPropertyName, await dbCtx.Organisations.FirstOrDefaultAsync(o=>o.Uuid == orgId, cancellationToken));
            }

            await base.OnActionExecutingAsync(actionContext, cancellationToken);
        }
    }
}
