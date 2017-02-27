using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MapHive.Server.Core.DataModel
{
    public static partial class ReadFilterValueExtensions
    {
        /// <summary>
        /// converts an enumerable to JArray as it would be deserialised when being sent from the clientside in JSON
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static JArray AsReadFilterList<T>(this IEnumerable<T> collection)
        {
            return JArray.Parse(JsonConvert.SerializeObject(collection.ToList()));
        }
    }
}
