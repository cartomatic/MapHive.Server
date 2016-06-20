using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapHive.Server.Core.DataModel.Interface;

namespace MapHive.Server.Core.DAL.Interface
{
    public interface ILinksDbContext
    {
        DbSet<ILink> Links { get; }
    }
}
