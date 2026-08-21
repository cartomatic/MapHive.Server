using System.Data.Entity;
using MapHive.Server.Core.DataModel;

namespace MapHive.Server.Core.DAL.Interface
{
    /// <summary>
    /// Whether or not a dbctx has access to the maphive Applications
    /// </summary>
    public interface IMapHiveApps
    { 
        DbSet<Application> Applications { get; set; } 
    }
}