using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapHive.Server.Core.DataModel;

namespace MapHive.Server.Core.DAL.TypeConfiguration
{
    public class MapHiveUserConfiguration : EntityTypeConfiguration<MapHiveUser>
    {
        public MapHiveUserConfiguration()
        {
            ToTable("users", "mh_meta");
            this.ApplyIBaseConfiguration(nameof(MapHiveUser));
            this.ApplyMapHiveUserBaseConfiguration();

            Property(p => p.Forename).HasColumnName("forename");
            Property(p => p.Surname).HasColumnName("surname");

            Property(t => t.Email)
                .HasColumnAnnotation(
                    "Index",
                    new IndexAnnotation(new IndexAttribute("") { IsUnique = true }));
        }
    }
}
