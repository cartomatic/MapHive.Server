using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using MapHive.Server.Core.DataModel;

namespace MapHive.Server.Core.DAL.TypeConfiguration
{
    public static partial class EntityTypeConfigurationExtensions
    {
        public static EntityTypeConfiguration<T> ApplyMapHiveUserBaseConfiguration<T>(this EntityTypeConfiguration<T> entity) where T : MapHiveUserBase
        {
            entity.Property(en => en.Email).HasColumnName("email");
            entity.Property(en => en.IsAccountClosed).HasColumnName("is_account_closed");
            entity.Property(en => en.IsAccountVerified).HasColumnName("is_account_verified");

            entity.Property(en => en.Email)
                .HasColumnAnnotation(
                    "Index",
                    new IndexAnnotation(new IndexAttribute($"uq_email_{nameof(T).ToLower()}")));

            return entity;
        }
    }
}
