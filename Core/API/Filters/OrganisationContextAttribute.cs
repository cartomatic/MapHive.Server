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

        //lower case here, so method params do not get underlined by r#
        public const string OrgIdPropertyName = "organisationId";


        /// <summary>
        /// Provides organisation context for a request
        /// </summary>
        /// <param name="actionContext"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            //extracting data only if key present in the action params will let controllers flagged with org ctx attribute to work without the org ctx if needed
            //org ctx getters will trhow though if one tries to obtain the ctx in such scenario
            if (actionContext.ActionArguments.ContainsKey(OrgIdPropertyName))
            {
                var orgId = (Guid)actionContext.ActionArguments[OrgIdPropertyName];
                actionContext.Request.Properties.Add(OrgIdPropertyName, orgId);

                using (var dbCtx = new MapHiveDbContext())
                {
                    actionContext.Request.Properties.Add(OrgCtxPropertyName, await dbCtx.Organisations.FirstOrDefaultAsync(o => o.Uuid == orgId, cancellationToken));
                }

                await base.OnActionExecutingAsync(actionContext, cancellationToken);
            }
        }
    }
}
