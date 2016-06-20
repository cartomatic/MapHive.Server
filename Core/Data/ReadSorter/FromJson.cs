using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MapHive.Server.Core.Data
{
    public static partial class ReadSorterExtensions
    {
        public static List<ReadSorter> ExtJsJsonSortersToReadSorters(this string json)
        {
            return string.IsNullOrEmpty(json) ?
                new List<ReadSorter>() :
                JsonConvert.DeserializeObject<List<ReadSorter>>(json);
        }
    }
}
