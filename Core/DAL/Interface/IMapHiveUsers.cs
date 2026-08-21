using System.Data.Entity;
using MapHive.Server.Core.DataModel;

namespace MapHive.Server.Core.DAL.Interface
{
    /// <summary>
    /// Whether or not a dbctx has access to the maphive users set
    /// </summary>
    public interface IMapHiveUsers <T>
        where T: MapHiveUserBase
    { 
        DbSet<T> Users { get; set; } 
    }
}