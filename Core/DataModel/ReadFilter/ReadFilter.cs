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
    }
}
