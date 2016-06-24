using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MapHive.Server.Core.Localisation
{
    /// <summary>
    /// Provides a standardised translation object with automated serialisation
    /// In order to use it with EF, a non-generic concrete type must be used
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial class TranslationsGeneric<T> : Dictionary<string, T>, ITranslations
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
                Dictionary<string, T> incomingStringData = null;
                try
                {
                    incomingStringData = JsonConvert.DeserializeObject<Dictionary<string, T>>(value, JsonSerializerSettings);
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
