using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MapHive.Server.Core.DataModel
{
    public partial class ReadFilter
    {
        public string Operator { get; set; }

        public dynamic Value { get; set; }

        public string Property { get; set; }

        /// <summary>
        /// Whether or not this should be a resultset limiting filter (joined with an AND / AndAlso condition after all the other filters have been assembled):
        /// When set to true, will be added to the Expression Tree as below:
        /// (X AND / OR Y AND / OR Z) AND XX AND YY
        /// </summary>
        public bool ExactMatch { get; set; }


        /// <summary>
        /// Support for SINGLE param db function to be called when filtering the data. the name of the function should be as defined in the System.Data.Entity.DbFunctions
        /// </summary>
        public string DbFn { get; set; }


        /// <summary>
        /// by default nested filters are joined using OR operator. When set to true this becomes AND
        /// </summary>
        public bool AndJoin { get; set; }

        /// <summary>
        /// nested filters that form one condtintion; because the default filter behavior is just joining all the filters in the greedy way (OR), using nested filters
        /// makes sense when used with ExactMatch. this way it is possible to provide filters such as (some filter AND (some other filter OR another one) AND so on...) AND other filters
        /// </summary>
        public List<ReadFilter> NestedFilters { get; set; }
    }
}
