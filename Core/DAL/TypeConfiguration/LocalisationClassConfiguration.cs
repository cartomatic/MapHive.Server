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
    public class LocalisationClassConfiguration : EntityTypeConfiguration<LocalisationClass> 
        //Note:
        //Deriving from ILocalisationConfiguration<DataModel.AppLocalisation> does not work. EF needs a concrete type nd throws otherwise
    {
        public LocalisationClassConfiguration()
        {
            ToTable("localisation_classes", "mh_localisation");
            this.ApplyIBaseConfiguration(nameof(LocalisationClass));

            Property(en => en.ApplicationName).HasColumnName("application_name");
            Property(en => en.ClassName).HasColumnName("class_name");
            Property(en => en.InheritedClassName).HasColumnName("inherited_class_name");

            Ignore(p => p.TranslationKeys);

            //indexes
            Property(en => en.ApplicationName)
                .HasColumnAnnotation(
                    "Index",
                    new IndexAnnotation(new IndexAttribute("uq_app_name_class_name") { IsUnique = true, Order = 1 }));
            Property(en => en.ClassName)
                .HasColumnAnnotation(
                    "Index",
                    new IndexAnnotation(new IndexAttribute("uq_app_name_class_name") { IsUnique = true, Order = 2 }));
        }
        
    }
}
