using System;
using System.Collections.Generic;

namespace MapHive.Server.Core.DataModel.Interface
{
    public interface ILinksDiff
    {
        /// <summary>
        /// Link objects to be either inserted or updated
        /// </summary>
        List<Link> Upsert { get; set; }

        /// <summary>
        /// Link objects to be destroyed
        /// </summary>
        List<Guid> Destroy { get; set; } 
    }
}
