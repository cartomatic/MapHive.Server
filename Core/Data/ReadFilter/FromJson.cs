using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MapHive.Server.Core.Data
{
    public static partial class ReadFilterExtensions
    {
        public static List<ReadFilter> ExtJsJsonFiltersToReadFilters(this string json)
        {
            return string.IsNullOrEmpty(json) ?
                new List<ReadFilter>() :
                JsonConvert.DeserializeObject<List<ReadFilter>>(json);
        }
    }
}
