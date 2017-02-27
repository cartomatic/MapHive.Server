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
        /// Gets links expressing organisation objects
        /// </summary>
        /// <typeparam name="TChild"></typeparam>
        /// <param name="dbCtx"></param>
        /// <returns></returns>
        protected async Task<IEnumerable<Link>> GetOrganisationLinksAsync<TChild>()
            where TChild : Base
        {
            return await OrganisationContext.GetChildLinksAsync<Organisation, TChild>(_dbCtx);
        }

        /// <summary>
        /// gets a list of organisations object ids
        /// </summary>
        /// <returns></returns>
        protected async Task<IEnumerable<Guid>> GetOrganisationObjectIdsAsync<TChild>()
            where TChild : Base
        {
            return (await GetOrganisationLinksAsync<TChild>()).Select(l => l.ChildUuid);
        }

        /// <summary>
        /// Peforms an equivalent of a standard crud Read controller but for organisation objects. such objects must be explicitly linked to an organisation object in order to be retrievable
        /// </summary>
        /// <typeparam name="TChild"></typeparam>
        /// <param name="sort"></param>
        /// <param name="filter"></param>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public async Task<IHttpActionResult> GetOrganisationAssets<TChild>(string sort = null, string filter = null,
            int start = 0,
            int limit = 25)
            where TChild : Base
        {
            try
            {
                //first need to get objects for an organisation, and then add an extra filter with object guids
                var orgObjIds = await GetOrganisationObjectIdsAsync<TChild>();
                var filters = filter.ExtJsJsonFiltersToReadFilters();

                filters.Add(
                    new ReadFilter
                    {
                        Property = "Uuid",
                        Value = orgObjIds.AsReadFilterList(),
                        Operator = "in",
                        ExactMatch = true
                    }
                );

                var obj = (T) Activator.CreateInstance(typeof(T));
                var objects = await obj.ReadAsync(_dbCtx, sort.ExtJsJsonSortersToReadSorters(), filters, start, limit);

                if (objects.Any())
                {
                    AppendTotalHeader(await obj.ReadCountAsync(_dbCtx, filters));
                    return Ok(objects);
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
        /// <typeparam name="TChild"></typeparam>
        /// <param name="uuid"></param>
        /// <returns></returns>
        public async Task<IHttpActionResult> GetOrganisationAsset<TChild>(Guid uuid)
            where TChild : Base
        {
            try
            {
                var orgObjIds = await GetOrganisationObjectIdsAsync<TChild>();
                if (!orgObjIds.Contains(uuid))
                    return BadRequest();

                return await base.GetAsync(uuid);
            }
            catch (Exception ex)
            {
                return this.HandleException(ex);
            }
        }
    }
}
