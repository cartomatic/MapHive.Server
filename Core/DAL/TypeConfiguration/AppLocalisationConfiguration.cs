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
    public class AppLocalisationConfiguration : EntityTypeConfiguration<AppLocalisation> 
        //Note:
        //Deriving from ILocalisationConfiguration<DataModel.AppLocalisation> does not work. EF needs a concrete type nd throws otherwise
    {
        public AppLocalisationConfiguration()
        {
            ToTable("localisation_app_translations");
            this.ApplyIBaseConfiguration();

            Property(en => en.ApplicationName).HasColumnName("application_name");
            Property(en => en.ClassName).HasColumnName("class_name");
            Property(en => en.TranslationKey).HasColumnName("translation_key");

            //Stuff below would be true if the class derived from ILocalisationConfiguration; this does not seem to work though...
            //Note: Translations dobe via ILocalisationConfiguration
            Property(p => p.Translations.Serialised).HasColumnName("translations");

            //indexes
            Property(en => en.ApplicationName)
                .HasColumnAnnotation(
                    "Index",
                    new IndexAnnotation(new IndexAttribute("uq_app_name_class_name_translation_key") { IsUnique = true, Order = 1 }));
            Property(en => en.ClassName)
                .HasColumnAnnotation(
                    "Index",
                    new IndexAnnotation(new IndexAttribute("uq_app_name_class_name_translation_key") { IsUnique = true, Order = 2 }));
            Property(en => en.TranslationKey)
                .HasColumnAnnotation(
                    "Index",
                    new IndexAnnotation(new IndexAttribute("uq_app_name_class_name_translation_key") { IsUnique = true, Order = 3 }));
        }
        
    }
}
