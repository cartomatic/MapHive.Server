using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapHive.Server.Core.DataModel;

namespace MapHive.Server.DataModel
{
    public partial class Application : Base
    {
        public Application() : base(Guid.Parse("a980c990-656f-47ca-8969-100853866d7b"))
        {
        }
    }
}
