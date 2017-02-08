using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapHive.Server.Core.DataModel.Interface;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MapHive.Server.Core.DataModel.SerialisableDict
{
    /// <summary>
    /// Provides a base for serialisable dicts; In order to use it with EF, dict must be made a concrete type, so new non-generic type needs to be derived
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class SerialisableDictBase<TKey, TValue> : Dictionary<TKey, TValue>, IJsonSerialisableType
    {
        private static JsonSerializerSettings JsonSerializerSettings => new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() };

        [JsonIgnore]
        public string Serialised
        {
            get
            {
                return JsonConvert.SerializeObject(this, Formatting.None, JsonSerializerSettings);
            }

            set
            {
                //deserialise...
                Dictionary<TKey, TValue> incomingStringData = null;
                try
                {
                    incomingStringData = JsonConvert.DeserializeObject<Dictionary<TKey, TValue>>(value, JsonSerializerSettings);
                }
                catch
                {
                    //ignore - silently fail
                }

                //do not modify self if this was invalid json...
                if (incomingStringData == null) return;

                //clear self, so can pump in the data
                this.Clear();

                foreach (var kv in incomingStringData)
                {
                    this.Add(kv.Key, kv.Value);
                }
            }
        }
    }
}
