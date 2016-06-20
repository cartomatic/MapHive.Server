using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MapHive.Server.Core.DataModel
{
    public abstract partial class Base
    {
        /// <summary>
        /// Reads a collection of objects; note - the public read 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbCtx"></param>
        /// <param name="sorters"></param>
        /// <param name="filters"></param>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <param name="detached"></param>
        /// <returns></returns>
        protected internal virtual async Task<IEnumerable<T>> Read<T>(DbContext dbCtx, IEnumerable<ReadSorter> sorters,
            IEnumerable<ReadFilter> filters, int start = 0, int limit = 25, bool detached = true) where T : Base
        {
            var dbSet = dbCtx.Set<T>();

            IQueryable<T> query;

            if (detached)
            {
                query = dbSet.AsNoTracking();
            }
            else
            {
                query = dbSet;
            }

            return await query.ApplyReadFilters(filters).ApplyReadSorters(sorters).Skip(start).Take(limit).ToListAsync();
        }

        /// <summary>
        /// Counts a set of data for a specified filterset
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbCtx"></param>
        /// <param name="filters"></param>
        /// <returns></returns>
        protected internal virtual async Task<int> ReadCount<T>(DbContext dbCtx, IEnumerable<ReadFilter> filters)
            where T : Base
        {
            var dbSet = dbCtx.Set<T>();

            var query = dbSet.ApplyReadFilters(filters);

            return await query.CountAsync();
        }

        /// <summary>
        /// Reads a single object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbCtx"></param>
        /// <param name="uuid"></param>
        /// <param name="detached"></param>
        /// <returns></returns>
        protected internal virtual async Task<T> Read<T>(DbContext dbCtx, Guid uuid, bool detached = true) where T : Base
        {
            T result = null;

            var dbSet = dbCtx.Set<T>();

            if (detached)
            {
                result = await dbSet.AsNoTracking().Where(o => o.Uuid == uuid).FirstOrDefaultAsync();
            }
            else
            {
                result = await dbSet.FindAsync(uuid);
            }

            return result;
        }

        /// <summary>
        /// Reads list of objects by their uuids
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbCtx"></param>
        /// <param name="uuids">List of object uuids to read; their sort order determines the order of a returned list</param>
        /// <param name="detached"></param>
        /// <returns></returns>
        protected internal virtual async Task<IEnumerable<T>> Read<T>(DbContext dbCtx, IEnumerable<Guid> uuids, bool detached = true)
            where T : Base
        {
            if (uuids == null)
                return new List<T>();

            var dbSet = dbCtx.Set<T>();

            List<T> data = null;

            if (detached)
            {
                data = await dbSet.AsNoTracking().Where(x => uuids.Contains(x.Uuid))
                    .ToListAsync();
            }
            else
            {
                data = await dbSet.Where(x => uuids.Contains(x.Uuid))
                            .ToListAsync();
            }

            var sorted = data.OrderBy((x => uuids.ToList().IndexOf(x.Uuid))).ToList();

            return sorted;

            //alternative way of sorting by another list;
            //var d = items.ToDictionary(x => x.Uuid);
            //var ordered = uuids.Select(i => d[i]).ToList();
        }



    }

}
