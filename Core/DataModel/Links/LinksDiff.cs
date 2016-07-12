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
        ///// <summary>
        ///// Link objects to be either inserted or updated
        ///// </summary>
        public List<Link> Upsert { get; set; } = new List<Link>();

        ///// <summary>
        ///// Link objects to be destroyed
        ///// </summary>
        public List<Guid> Destroy { get; set; } = new List<Guid>();


        List<ILink> ILinksDiff.Upsert
        {
            get { return Upsert.Select(u => u as ILink).ToList(); }

            set { Upsert = value.Select(u => u as Link).ToList(); }
        }
    }
}
