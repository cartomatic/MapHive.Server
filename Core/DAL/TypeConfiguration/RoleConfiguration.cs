using System.Data.Entity.ModelConfiguration;
using MapHive.Server.Core.DataModel;

namespace MapHive.Server.Core.DAL.TypeConfiguration
{
    public class RoleConfiguration : EntityTypeConfiguration<Role>
    {
        public RoleConfiguration()
        {
            ToTable("roles", "mh_meta");
            this.ApplyIBaseConfiguration(nameof(Role));

            Property(p => p.Identifier).HasColumnName("identifier");
            Property(p => p.Name).HasColumnName("name");
            Property(p => p.Description).HasColumnName("description");
        }
    }
}
