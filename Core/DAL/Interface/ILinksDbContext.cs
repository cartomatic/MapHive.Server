using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapHive.Server.Core.DataModel.Interface;

namespace MapHive.Server.Core.DAL.Interface
{
    /// <summary>
    /// Db context that can save object links
    /// </summary>
    public interface ILinksDbContext
    {
        DbSet<ILink> Links { get; }
    }
}
