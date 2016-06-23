using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapHive.Server.Core.DAL.TypeConfiguration;

namespace MapHive.Server.DataModel.DAL.TypeConfiguration
{
    public class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            ToTable("users");
            this.ApplyIBaseConfiguration();

            Property(p => p.Email).HasColumnName("email");
            Property(p => p.Forename).HasColumnName("forename");
            Property(p => p.Surname).HasColumnName("surname");
            Property(en => en.IsAccountClosed).HasColumnName("is_account_closed");
            Property(en => en.IsAccountVerified).HasColumnName("is_account_verified");

            Property(t => t.Email)
                .HasColumnAnnotation(
                    "idx",
                    new IndexAnnotation(new IndexAttribute("uq_email") { IsUnique = true }));
        }
    }
}
