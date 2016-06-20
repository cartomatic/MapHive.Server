using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MapHive.Server.Core.DataModel
{
    public static partial class ReadSorterExtensions
    {
        private const string Asc = "ASC";

        /// <summary>
        /// Applies sorting to IQueryable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="sorters"></param>
        /// <param name="defaultSortProperty"></param>
        /// <param name="defaultSortOrder"></param>
        /// <returns></returns>
        public static IQueryable<T> ApplyReadSorters<T>(this IQueryable<T> query, IEnumerable<ReadSorter> sorters, string defaultSortProperty = "createdate", string defaultSortOrder = Asc)
        {
            // If sorters not provided or empty, add a default one - sorting is needed when using take / skip later on
            if (sorters == null || !sorters.Any())
            {
                sorters = new List<ReadSorter>()
                {
                    new ReadSorter() { Property = defaultSortProperty, Direction = defaultSortOrder }
                };
            }

            // Add linq sort expressions for each sort criteria item
            foreach (var sorter in sorters)
                query = query.OrderBy(sorter.Property, sorter.Direction == Asc, sorter.Equals(sorters.First()));

            return query;
        }


        /// <summary>
        /// Helper method, sorting by reflections
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="propertyd"></param>
        /// <param name="ascending"></param>
        /// <param name="firstProperty"></param>
        /// <returns></returns>
        private static IQueryable<T> OrderBy<T>(this IQueryable<T> query, string property, bool ascending, bool firstProperty)
        {
            //first expression param type as in: p in the p => 
            var expType = Expression.Parameter(typeof(T), "p");

            //next property as in p => p.PropertyName
            var expProp = Expression.Property(expType, property);

            //expression that specifies property on a type as in p => p.Property
            var exp = Expression.Lambda(expProp, expType);

            //work out which sorting method should be used
            var method = ascending ? "OrderBy" : "OrderByDescending";
            if (!firstProperty)
                method = ascending ? "ThenBy" : "ThenByDescending";


            var types = new Type[] { query.ElementType, exp.Body.Type };

            //OrderBy / ThenBy are extension methods, so just need to specify the iqueryable as the first param, and then the expression that specifies the propery to filter on
            var methodCallExperssion = Expression.Call(typeof(Queryable), method, types, query.Expression, exp);

            //finally assemble a query to return
            return query.Provider.CreateQuery<T>(methodCallExperssion);
        }
    }
}
