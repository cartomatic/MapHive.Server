using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapHive.Server.Core.Interfaces
{
    interface ILinksDiff
    {
        /// <summary>
        /// Link objects to be either inserted or updated
        /// </summary>
        IEnumerable<ILink> Upsert { get; set; }

        /// <summary>
        /// Link objects to be destroyed
        /// </summary>
        IEnumerable<Guid> Destroy { get; set; } 
    }
}
