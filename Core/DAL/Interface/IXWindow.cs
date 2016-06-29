using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapHive.Server.Core.DataModel;

namespace MapHive.Server.Core.DAL.Interface
{
    public interface IXWindow
    {
        DbSet<XWindowOrigin> XWindowOrigins { get; set; }
    }
}
