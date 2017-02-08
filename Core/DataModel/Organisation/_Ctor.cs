using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapHive.Server.Core.DataModel;

namespace MapHive.Server.Core.DataModel
{
    public partial class Organisation : Base
    {
        public Organisation() : base(Guid.Parse("0bc1402a-ec54-4e50-8e04-eb22a7625b91"))
        {
        }
    }
}
