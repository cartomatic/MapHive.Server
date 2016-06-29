using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapHive.Server.Core.DataModel
{
    public partial class XWindowOrigin
    {
        public string Origin { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// Whether or not this is a custom XWindowOriginHost
        /// Custom means only available in a relation to a user and his network
        /// </summary>
        public bool Custom { get; set; }
    }
}
