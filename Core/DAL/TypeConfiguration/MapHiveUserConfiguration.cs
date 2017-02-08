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
            Property(p => p.Slug).HasColumnName("slug");
            Property(p => p.Bio).HasColumnName("bio");
            Property(p => p.Company).HasColumnName("company");
            Property(p => p.Department).HasColumnName("department");
            Property(p => p.Location).HasColumnName("location");
            Property(p => p.GravatarEmail).HasColumnName("gravatar_email");
            Ignore(p => p.ProfilePicture);
            Property(p => p.ProfilePictureId).HasColumnName("profile_picture_id");
            Property(p => p.IsOrgUser).HasColumnName("is_org_user");

            Property(en => en.Slug)
                .HasColumnAnnotation(
                    "Index",
                    new IndexAnnotation(new IndexAttribute($"idx_slug_{nameof(MapHiveUser).ToLower()}") {IsUnique = true}));

        }
    }
}
