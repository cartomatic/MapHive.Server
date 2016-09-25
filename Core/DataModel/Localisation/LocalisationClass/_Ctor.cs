using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapHive.Server.Core.DataModel.Interface;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MapHive.Server.Core.DataModel
{
    public partial class LocalisationClass : Base
    {
        public LocalisationClass() : base(Guid.Parse("03ad4b67-7801-4cf1-90dd-fe65674fc1e6"))
        {
        }
    }
}
