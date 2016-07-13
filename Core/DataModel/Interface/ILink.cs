using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapHive.Server.Core.DataModel.Interface
{
    /// <summary>
    /// ILink describes the Link object that is used to express a relation between objects
    /// </summary>
    public interface ILink
    {
        /// <summary>
        /// Link identifier (primary key in relationships table)
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// Unique identifier for object that contains links; direction of a link is from parent to child, although obviously the dataset as such can bq querried the other way round too
        /// </summary>
        Guid ParentUuid { get; set; }

        /// <summary>
        /// Unique identifier for object that is linked to parent object
        /// </summary>
        Guid ChildUuid { get; set; }

        /// <summary>
        /// Parent's type unique identifier
        /// </summary>
        Guid ParentTypeUuid { get; set; }

        /// <summary>
        /// Child's type unique identifier
        /// </summary>
        Guid ChildTypeUuid { get; set; }

        /// <summary>
        /// Sort order if any
        /// </summary>
        int? SortOrder { get; set; }

        /// <summary>
        /// Extra data to be saved with the link; can store data for different applications within the same link
        /// </summary>
        ILinkData LinkData { get; set; }

        /// <summary>
        /// LinkData setter; used so can set data into a collection of specific type, while still maintaining an interface input
        /// </summary>
        /// <param name="linkData"></param>
        void SetLinkData(ILinkData linkData);
    }
}
