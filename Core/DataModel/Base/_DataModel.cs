using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapHive.Server.Core.DataModel.Interface;

namespace MapHive.Server.Core.DataModel
{
    public abstract partial class Base
    {
        /// <summary>
        /// Unique object identifier; generated automatically upon saving; See DAL MapHiveDatabaseContextBase for more details
        /// </summary>
        public Guid Uuid { get; set; }


        //Note: automated data updates relating to IBase: hooked into OnSaving. See DAL BaseDbContext for more details
        
        /// <summary>
        /// Object creator - updated automatically
        /// </summary>
        public Guid? CreatedBy { get; set; }

        /// <summary>
        /// Object last modifued by - updated automatically
        /// </summary>
        public Guid? LastModifiedBy { get; set; }

        
        /// <summary>
        /// Create date - updated automatically
        /// </summary>
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// Modify date - updated automatically
        /// </summary>
        public DateTime? ModifyDate { get; set; }

        /// <summary>
        /// End date - updated automatically
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Object relations defined as set of links; this object is ignored when object is saved and is used only to provide a DIFF of links that should be applied to the db representation
        /// </summary>
        public ILinksDiff Links { get; set; } = new LinksDiff();
    }
}
