using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapHive.Server.Core.DataModel;
using MapHive.Server.Core.DataModel.Interface;

namespace MapHive.Server.Core.DAL.Interface
{
    /// <summary>
    /// Db context that can save object links
    /// </summary>
    public interface ILinksDbContext
    {
        /// <summary>
        /// A collection of link objects that define object relations
        /// </summary>
        DbSet<Link> Links { get; set; }
    }
}
