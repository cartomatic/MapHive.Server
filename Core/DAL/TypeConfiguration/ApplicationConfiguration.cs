﻿using System;
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
    public class ApplicationConfiguration : EntityTypeConfiguration<Application>
    {
        public ApplicationConfiguration()
        {
            ToTable("applications", "mh_meta");
            this.ApplyIBaseConfiguration(nameof(Application));

            Property(en => en.ShortName).HasColumnName("short_name");
            Property(en => en.Name).HasColumnName("name");
            Property(en => en.Description).HasColumnName("description");
            Property(en => en.Urls).HasColumnName("urls");
            Property(en => en.UseSplashscreen).HasColumnName("use_splashscreen");
            Property(en => en.RequiresAuth).HasColumnName("requires_auth");
            Property(en => en.IsCommon).HasColumnName("is_common");
            Property(en => en.IsDefault).HasColumnName("is_default");
            Property(en => en.IsHome).HasColumnName("is_home");
            Property(en => en.IsHive).HasColumnName("is_hive");
            Property(en => en.ProviderId).HasColumnName("provider_id");

            Property(t => t.ShortName)
                .HasColumnAnnotation(
                    "Index",
                    new IndexAnnotation(new IndexAttribute("uq_short_name") { IsUnique = true }));
        }
    }
}
