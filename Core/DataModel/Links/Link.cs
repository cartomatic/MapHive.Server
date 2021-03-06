﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapHive.Server.Core.DataModel.Interface;
using Newtonsoft.Json;

namespace MapHive.Server.Core.DataModel
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
        /// Sort order if any
        /// </summary>
        public int? SortOrder { get; set; }

        /// <summary>
        /// Extra data to be saved with the link; can store data for different applications within the same link
        /// </summary>
        public LinkData LinkData { get; set; } = new LinkData();

        /// <summary>
        /// LinkData setter; used so can set data into a collection of specific type, while still maintaining an interface input
        /// </summary>
        /// <param name="linkData"></param>
        public void SetLinkData(ILinkData linkData)
        {
            //Note: complex type cannot be null, otherwise EF will throw!
            LinkData = (LinkData)linkData ?? new LinkData();
        }

        /// <summary>
        /// explicit interface implementation - lets the Base keep the ILinkData interface while it implements a concrete ILinkData type, so EF can handle the data mapping
        /// </summary>
        ILinkData ILink.LinkData
        {
            get { return LinkData; }
            set { LinkData = (LinkData) value; }
        }
    }
}
