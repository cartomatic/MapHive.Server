using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapHive.Server.Core.DataModel.Interface;

namespace MapHive.Server.Core.DataModel
{
    /// <summary>
    /// Decribes changes in the object links. Changes must be explicit in order to modify the links
    /// </summary>
    public class LinksDiff : ILinksDiff
    {
        /// <summary>
        /// Link objects to be either inserted or updated
        /// </summary>
        public IList<ILink> Upsert { get; set; } = new List<ILink>(); 

        /// <summary>
        /// Link objects to be destroyed
        /// </summary>
        public IList<Guid> Destroy { get; set; } = new List<Guid>();
    }
}
