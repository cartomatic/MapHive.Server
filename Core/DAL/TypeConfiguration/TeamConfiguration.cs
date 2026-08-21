using System.Data.Entity.ModelConfiguration;
using MapHive.Server.Core.DataModel;

namespace MapHive.Server.Core.DAL.TypeConfiguration
{
    public class TeamConfiguration : EntityTypeConfiguration<Team>
    {
        public TeamConfiguration()
        {
            ToTable("teams", "mh_meta");
            this.ApplyIBaseConfiguration(nameof(Role));

            Property(p => p.Name).HasColumnName("name");
            Property(p => p.Description).HasColumnName("description");
        }
    }
}
