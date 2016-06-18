using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapHive.Server.Core.Interfaces;

namespace MapHive.Server.Core.Data
{
    /// <summary>
    /// Decribes changes in the object links. Changes must be explicit in order to modify the links
    /// </summary>
    public class LinksDiff : ILinksDiff
    {
        /// <summary>
        /// Link objects to be either inserted or updated
        /// </summary>
        public IEnumerable<ILink> Upsert { get; set; } = new List<Link>(); 

        /// <summary>
        /// Link objects to be destroyed
        /// </summary>
        public IEnumerable<Guid> Destroy { get; set; } = new List<Guid>();
    }
}
