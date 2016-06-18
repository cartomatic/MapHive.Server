using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapHive.Server.Core.Interfaces
{
    public interface ILinksDbContext
    {
        DbSet<ILink> Links { get; }
    }
}
