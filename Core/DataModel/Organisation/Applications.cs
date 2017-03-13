using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MapHive.Server.Core.DataModel;
using MapHive.Server.Core.DAL.DbContext;

namespace MapHive.Server.Core.DataModel
{
    
    public partial class Organisation
    {
        /// <summary>
        /// checks if an org can access an application
        /// </summary>
        /// <param name="dbCtx"></param>
        /// <param name="orgId"></param>
        /// <param name="appId"></param>
        /// <returns></returns>
        public static async Task<bool> CanUseApp(MapHiveDbContext dbCtx, Guid orgId, Guid appId)
        {
            var app = await dbCtx.Applications.FirstOrDefaultAsync(a => a.Uuid == appId);
            if (app == null)
                return false;

            //if an ap is common then every organisation can access it
            if (app.IsCommon)
                return true;

            //app is not common, so need to see if an app is assigned to this very org
            return await app.HasParentLinkAsync(dbCtx, orgId);
        }

        /// <summary>
        /// Determines whether or not an organisation has an access to an application
        /// </summary>
        /// <param name="dbCtx"></param>
        /// <param name="app"></param>
        /// <returns></returns>
        public async Task<bool> CanUseApp(DbContext dbCtx, Application app)
        {
            return app.IsCommon || await this.HasChildLinkAsync(dbCtx, app);
        }


        /// <summary>
        /// Reads applications that are visible to an organisation and can be linked to it - returns apps that require auth and apps that are non-public (specific to that very org)
        /// </summary>
        /// <param name="dbCtx"></param>
        /// <param name="sort"></param>
        /// <param name="filter"></param>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public async Task<Tuple<IEnumerable<Application>, int>> GetOrganisationLinkableApps(DbContext dbCtx, string sort = null, string filter = null,
            int start = 0,
            int limit = 25)
        {
            //first need to get objects for an organisation, and then add an extra filter with object guids
            var orgObjIds = await GetOrganisationObjectIdsAsync<Application>(dbCtx);

            //do make sure there is something to read!
            var filters = filter.ExtJsJsonFiltersToReadFilters();


            //return all the apps 
            var preFilter = new ReadFilter
            {
                Property = nameof(Application.RequiresAuth),
                Value = true,
                Operator = "==",
                ExactMatch = true, //make the whole filter exact
                AndJoin = true,
                NestedFilters = new List<ReadFilter>
                {
                    //need common apps
                    new ReadFilter
                    {
                        Property = nameof(Application.IsCommon),
                        Value = true
                    },
                    //but not the dashboard. users always can access the dashboard!
                    new ReadFilter
                    {
                        Property = nameof(Application.IsDefault),
                        Value = false
                    }
                }
            };

            //if there are org specific apps always return them in addition to the ones filtered by default
            if (orgObjIds.Any())
            {
                preFilter = new ReadFilter
                {
                    Property = "Uuid",
                    Value = orgObjIds.AsReadFilterList(),
                    Operator = "in",
                    ExactMatch = true,
                    AndJoin = false,
                    NestedFilters = new List<ReadFilter>
                    {
                        preFilter
                    }
                };
            }

            filters.Add(preFilter);


            var app = new Application();
            var apps = await app.ReadAsync(dbCtx, sort.ExtJsJsonSortersToReadSorters(), filters, start, limit);

            if (!apps.Any()) return null;

            var count = await apps.First().ReadCountAsync(dbCtx, filters);

            return new Tuple<IEnumerable<Application>, int>(apps, count);
        }
    }
}
