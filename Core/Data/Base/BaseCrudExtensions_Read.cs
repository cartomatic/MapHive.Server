using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapHive.Server.Core.Data
{
    public static partial class BaseCrudExtensions
    {
        //Note:
        //Crud APIs exposed as extension methods, so the type of the object is actually worked out without having to specify it explicitly;
        //the actual Crud methods are protected


        /// <summary>
        /// Reads a list of objects
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="dbCtx"></param>
        /// <param name="sorters"></param>
        /// <param name="filters"></param>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <param name="detached"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<T>> Read<T>(this T obj, DbContext dbCtx, List<ReadSorter> sorters,
            List<ReadFilter> filters, int start = 0, int limit = 25, bool detached = true) where T : Base
        {
            return await obj.Read<T>(dbCtx, sorters, filters, start, limit, detached);
        }

        /// <summary>
        /// Returns a count of records for a given filters set
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="dbCtx"></param>
        /// <param name="filters"></param>
        /// <param name="detached"></param>
        /// <returns></returns>
        public static async Task<int> ReadCount<T>(this T obj, DbContext dbCtx, List<ReadFilter> filters)
            where T : Base
        {
            return await obj.ReadCount<T>(dbCtx, filters);
        }

        /// <summary>
        /// Reads a single object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="dbCtx"></param>
        /// <param name="uuid"></param>
        /// <param name="detached"></param>
        /// <returns></returns>
        public static async Task<T> Read<T>(this T obj, DbContext dbCtx, Guid uuid, bool detached = true) where T : Base
        {
            return await obj.Read<T>(dbCtx, uuid, detached);
        }
    }
}
