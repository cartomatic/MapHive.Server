using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapHive.Server.Core.DataModel;
using MapHive.Server.Core.DataModel.Interface;

namespace MapHive.Server.Core.DAL.TypeConfiguration
{
    public static class EntityTypeConfigurationExtensions
    {
        /// <summary>
        /// Takes care of setting up type configuration specific to the IBase model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="entityName">Name of the entity. used for automated index naming</param>
        /// <returns></returns>
        public static EntityTypeConfiguration<T> ApplyIBaseConfiguration<T>(this EntityTypeConfiguration<T> entity, string entityName) where T : class, IBase
        {
            entity.HasKey(t => t.Uuid);

            entity.Property(en => en.Uuid).HasColumnName("uuid");
            entity.Property(en => en.CreatedBy).HasColumnName("created_by");
            entity.Property(en => en.LastModifiedBy).HasColumnName("last_modified_by");
            entity.Property(en => en.CreateDateUtc).HasColumnName("create_date_utc");
            entity.Property(en => en.ModifyDateUtc).HasColumnName("modify_date_utc");
            entity.Property(en => en.EndDateUtc).HasColumnName("end_date_utc");

            entity.Ignore(p => p.TypeUuid);
            entity.Ignore(p => p.Links);

            entity.Property(en => en.CreateDateUtc)
                .HasColumnAnnotation(
                    "Index",
                    new IndexAnnotation(new IndexAttribute($"idx_create_date_{entityName.ToLower()}")));

            return entity;
        }

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
