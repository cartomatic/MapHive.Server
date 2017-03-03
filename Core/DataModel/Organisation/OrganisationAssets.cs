using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapHive.Server.Core.DataModel;

namespace MapHive.Server.Core.DataModel
{
    public partial class Organisation
    {
        /// <summary>
        /// Gets links expressing organisation objects
        /// </summary>
        /// <typeparam name="TChild"></typeparam>
        /// <param name="dbCtx"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Link>> GetOrganisationLinksAsync<TChild>(DbContext dbCtx)
            where TChild : Base
        {
            return await this.GetChildLinksAsync<Organisation, TChild>(dbCtx);
        }

        /// <summary>
        /// gets a list of organisations object ids
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Guid>> GetOrganisationObjectIdsAsync<TChild>(DbContext dbCtx)
            where TChild : Base
        {
            return (await GetOrganisationLinksAsync<TChild>(dbCtx)).Select(l => l.ChildUuid);
        }


        /// <summary>
        /// Reads org assets of given type; an org asset is an object the org has links to; meant to simplify paged org assets reading
        /// </summary>
        /// <typeparam name="TChild"></typeparam>
        /// <param name="dbCtx"></param>
        /// <param name="sort"></param>
        /// <param name="filter"></param>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public async Task<Tuple<IEnumerable<TChild>, int>> GetOrganisationAssets<TChild>(DbContext dbCtx, string sort = null, string filter = null,
            int start = 0,
            int limit = 25)
            where TChild : Base
        {
            //first need to get objects for an organisation, and then add an extra filter with object guids
            var orgObjIds = await GetOrganisationObjectIdsAsync<TChild>(dbCtx);

            //do make sure there is something to read!
            if (!orgObjIds.Any())
                return null;

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

            var obj = (TChild)Activator.CreateInstance(typeof(TChild));
            var objects = await obj.ReadAsync(dbCtx, sort.ExtJsJsonSortersToReadSorters(), filters, start, limit);

            if (!objects.Any()) return null;

            var count = await objects.First().ReadCountAsync(dbCtx, filters);

            return new Tuple<IEnumerable<TChild>, int>(objects, count);
        }

        /// <summary>
        /// Reads an organisation asset. Ensures the asset is actually linked to organisation before reading it
        /// </summary>
        /// <typeparam name="TChild"></typeparam>
        /// <param name="dbCtx"></param>
        /// <param name="uuid"></param>
        /// <returns></returns>
        public async Task<TChild> GetOrganisationAsset<TChild>(DbContext dbCtx, Guid uuid)
            where TChild : Base
        {
            var orgObjIds = await GetOrganisationObjectIdsAsync<TChild>(dbCtx);
            if (!orgObjIds.Contains(uuid))
                return null;

            var obj = (TChild)Activator.CreateInstance(typeof(TChild));
            return await obj.ReadAsync(dbCtx, uuid);
        }

        /// <summary>
        /// Checks whether or not an obj is an org asset - is linked to an org
        /// </summary>
        /// <typeparam name="TChild"></typeparam>
        /// <param name="dbCtx"></param>
        /// <param name="child"></param>
        /// <returns></returns>
        public async Task<bool> IsOrganisationAsset<TChild>(DbContext dbCtx, TChild child)
            where TChild : Base
        {
            return (await this.GetChildLinkAsync(dbCtx, child)) != null;
        }

        /// <summary>
        /// whether or not an org has a link of given uuid
        /// </summary>
        /// <param name="dbCtx"></param>
        /// <param name="childId"></param>
        /// <returns></returns>
        public async Task<bool> IsOrganisationAsset(DbContext dbCtx, Guid childId)
        {
            return (await this.GetChildLinkAsync(dbCtx, childId)) != null;
        }
    }
}
