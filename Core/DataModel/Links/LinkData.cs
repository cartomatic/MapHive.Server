using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapHive.Server.Core.DataModel.Interface;
using Newtonsoft.Json;

namespace MapHive.Server.Core.DataModel
{
    public class LinkData : Dictionary<string, Dictionary<string,object>>, ILinkData
    {
        [JsonIgnore]
        public string Serialised
        {
            get
            {
                return JsonConvert.SerializeObject(this);
            }

            set
            {
                //deserialise...
                Dictionary<string, Dictionary<string, object>> incomingStringData = null;
                try
                {
                    incomingStringData = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, object>>>(value);
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
