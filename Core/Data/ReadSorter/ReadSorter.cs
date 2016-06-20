using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapHive.Server.Core.Data
{
    public partial class ReadSorter
    {
        /// <summary>
        /// Object property to sort on
        /// </summary>
        public string Property { get; set; }

        /// <summary>
        /// ummm.... well... sort direction ;)
        /// </summary>
        public string Direction { get; set; }
    }
}
