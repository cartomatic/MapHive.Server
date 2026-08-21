using System;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Http;
using MapHive.Server.Core.DataModel;

namespace MapHive.Server.Core.API
{
    public abstract partial class BaseApiOrganisatinCrudController<T, TDbCtx> : BaseApiCrudController<T, TDbCtx>
        where T : Base
        where TDbCtx : DbContext, new()
    {
        public async Task<IHttpActionResult> PutAsync(T obj, Guid uuid)
        {
            if (await OrganisationContext.IsOrganisationAsset(_dbCtx, uuid))
            {
                return await base.PutAsync(obj, uuid);
            }
            return BadRequest();
        }
    }
}
