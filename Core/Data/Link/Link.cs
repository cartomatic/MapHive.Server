using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapHive.Server.Core.Interfaces;

namespace MapHive.Server.Core.Data
{
    /// <summary>
    /// Link object used to express relations between objects
    /// </summary>
    public class Link : ILink
    {
        /// <summary>
        /// Link identifier (primary key in relationships table)
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Unique identifier for object that contains links; direction of a link is from parent to child, although obviously the dataset as such can bq querried the other way round too
        /// </summary>
        public Guid ParentUuid { get; set; }

        /// <summary>
        /// Unique identifier for object that is linked to parent object
        /// </summary>
        public Guid ChildUuid { get; set; }

        /// <summary>
        /// Parent's type unique identifier
        /// </summary>
        public Guid ParentTypeUuid { get; set; }

        /// <summary>
        /// Child's type unique identifier
        /// </summary>
        public Guid ChildTypeUuid { get; set; }

        /// <summary>
        /// Extra JSON serialised data
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// Sort order if any
        /// </summary>
        public int? SortOrder { get; set; }
    }
}
