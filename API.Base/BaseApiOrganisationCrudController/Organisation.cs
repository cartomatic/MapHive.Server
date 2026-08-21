using System;
using System.Data.Entity;
using MapHive.Server.Core.API.Filters;
using MapHive.Server.Core.DataModel;

namespace MapHive.Server.Core.API
{
    public abstract partial class BaseApiOrganisatinCrudController<T, TDbCtx> : BaseApiCrudController<T, TDbCtx>
        where T : Base
        where TDbCtx : DbContext, new()
    {
        /// <summary>
        /// Gets organisation identifier
        /// </summary>
        public Guid OrganisationId
        {
            get
            {
                var orgId = (Guid?)Request.Properties[OrganisationContextAttribute.OrgIdPropertyName];
                if(!orgId.HasValue)
                    throw new InvalidOperationException("Organisation id is not known for this request.");

                return orgId.Value;
            }
        }

        /// <summary>
        /// Returns organisation context (organisation object) for the request
        /// </summary>
        public Organisation OrganisationContext
        {
            get
            {
                var orgCtx = (Organisation) Request.Properties[OrganisationContextAttribute.OrgCtxPropertyName];
                if (orgCtx == null)
                    throw new InvalidOperationException("Organisation context is not known for this request.");

                return orgCtx;
            }
        }
    }
}
