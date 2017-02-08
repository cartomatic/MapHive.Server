using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapHive.Server.Core.DataModel;

namespace MapHive.Server.Core.DataModel
{
    public partial class Team : Base
    {
        public Team() : base(Guid.Parse("907ef53f-9c2e-4463-bb52-3b6e97bc21ab"))
        {
        }
    }
}
