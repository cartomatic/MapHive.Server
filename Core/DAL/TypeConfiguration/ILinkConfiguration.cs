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
    public class ILinkConfiguration : EntityTypeConfiguration<ILink>
    {
        public ILinkConfiguration(string table = "links")
        {
            ToTable(table);

            HasKey(p => p.Id);

            Property(en => en.ParentUuid).HasColumnName("parent_uuid");
            Property(en => en.ChildUuid).HasColumnName("child_uuid");
            Property(en => en.ParentTypeUuid).HasColumnName("parent_type_uuid");
            Property(en => en.ChildTypeUuid).HasColumnName("child_type_uuid");
            Property(en => en.SortOrder).HasColumnName("sort_order");

            Property(t => t.ParentUuid)
                .HasColumnAnnotation(
                    "idx",
                    new IndexAnnotation(new IndexAttribute("parent_uuid") {IsUnique = true}));

            Property(t => t.ChildUuid)
                .HasColumnAnnotation(
                    "idx",
                    new IndexAnnotation(new IndexAttribute("child_uuid") { IsUnique = true }));

            Property(t => t.ParentTypeUuid)
                .HasColumnAnnotation(
                    "idx",
                    new IndexAnnotation(new IndexAttribute("parent_type_uuid") { IsUnique = true }));

            Property(t => t.ChildTypeUuid)
                .HasColumnAnnotation(
                    "idx",
                    new IndexAnnotation(new IndexAttribute("child_type_uuid") { IsUnique = true }));
        }
    }
}
