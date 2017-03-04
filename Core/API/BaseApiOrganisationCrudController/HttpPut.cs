using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using MapHive.Server.Core.DataModel;
using MapHive.Server.Core.DataModel.Interface;

namespace MapHive.Server.Core.API
{
    public abstract partial class BaseApiOrganisatinCrudController<T, TDbCtx> : BaseApiCrudController<T, TDbCtx>
        where T : Base
        where TDbCtx : DbContext, new()
    {
        public async Task<IHttpActionResult> PutAsync(T obj, Guid uuid)
        {
            if (await OrganisationContext.IsOrganisationAsset(_dbCtx, obj))
            {
                return await base.PostAsync(obj);
            }
            return BadRequest();
        }
    }
}
