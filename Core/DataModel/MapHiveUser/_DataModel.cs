using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using MapHive.Server.Core.DataModel;

namespace MapHive.Server.Core.DataModel
{
    /// <summary>
    /// Customised user
    /// </summary>
    public partial class MapHiveUser
    {
        public string Forename { get; set; }

        public string Surname { get; set; }
    }
}
