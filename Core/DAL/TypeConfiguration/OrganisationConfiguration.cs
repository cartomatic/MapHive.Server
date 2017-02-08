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
    public class OrganisationConfiguration : EntityTypeConfiguration<Organisation>
    {
        public OrganisationConfiguration()
        {
            ToTable("organisations", "mh_meta");
            this.ApplyIBaseConfiguration(nameof(Organisation));

            Property(p => p.Slug).HasColumnName("slug");
            Property(p => p.DisplayName).HasColumnName("display_name");
            Property(p => p.Description).HasColumnName("description");
            Property(p => p.Location).HasColumnName("location");
            Property(p => p.Url).HasColumnName("url");
            Property(p => p.Email).HasColumnName("email");
            Property(p => p.GravatarEmail).HasColumnName("gravatar_email");
            Ignore(p => p.ProfilePicture);
            Property(p => p.ProfilePictureId).HasColumnName("profile_picture_id");
            Property(p => p.BillingEmail).HasColumnName("billing_email");
            Property(p => p.BillingAddress).HasColumnName("billing_address");
            Property(p => p.BillingExtraInfo.Serialised).HasColumnName("billing_extra_info");

            Property(en => en.Slug)
                .HasColumnAnnotation(
                    "Index",
                    new IndexAnnotation(new IndexAttribute($"idx_slug_{nameof(Organisation).ToLower()}") { IsUnique = true }));
        }
    }
}
