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
    public partial class AppLocalisation : Base, ILocalisation
    {
        public AppLocalisation() : base(Guid.Parse("987ce604-4125-44e6-bd6d-8db0857756a4"))
        {
        }
    }
}
