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
        public async Task<IHttpActionResult> PostAsync(T obj)
        {
            try
            {
                //create an obj and then link it to an org
                var entity = await obj.CreateAsync(_dbCtx);

                OrganisationContext.AddLink(entity);
                await OrganisationContext.UpdateAsync(_dbCtx);

                return Ok(entity);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}
