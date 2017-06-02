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
        /// <summary>
        /// Peforms an equivalent of a standard crud Read controller but for organisation objects. such objects must be explicitly linked to an organisation object in order to be retrievable
        /// </summary>
        /// <param name="sort"></param>
        /// <param name="filter"></param>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public async Task<IHttpActionResult> GetAsync(string sort = null, string filter = null,
            int start = 0,
            int limit = 25)
        {
            try
            {
                var assets = await OrganisationContext.GetOrganisationAssets<T>(_dbCtx, sort, filter, start, limit);
                
                if (assets != null)
                {
                    AppendTotalHeader(assets.Item2);
                    return Ok(assets.Item1);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return this.HandleException(ex);
            }
        }

        /// <summary>
        /// Peforms an equivalent of a standard crud controller Read single but for organisation objects. such objects must be explicitly linked to an organisation object in order to be retrievable
        /// </summary>
        /// <param name="uuid"></param>
        /// <returns></returns>
        public async Task<IHttpActionResult> GetAsync(Guid uuid)
        {
            try
            {
                var asset = await OrganisationContext.GetOrganisationAsset<T>(_dbCtx, uuid);
                if (asset == null)
                    return NotFound();

                return Ok(asset);
            }
            catch (Exception ex)
            {
                return this.HandleException(ex);
            }
        }
    }
}
