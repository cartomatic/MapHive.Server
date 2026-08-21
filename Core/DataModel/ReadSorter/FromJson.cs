using System.Collections.Generic;
using Newtonsoft.Json;

namespace MapHive.Server.Core.DataModel
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
