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
    public class TranslationKeyConfiguration : EntityTypeConfiguration<TranslationKey> 
        //Note:
        //Deriving from ILocalisationConfiguration<DataModel.AppLocalisation> does not work. EF needs a concrete type nd throws otherwise
    {
        public TranslationKeyConfiguration()
        {
            ToTable("translation_keys", "mh_localisation");
            this.ApplyIBaseConfiguration(nameof(AppLocalisation));

            Property(en => en.LocalisationClassUuid).HasColumnName("localisation_class_uuid");
            Property(en => en.Key).HasColumnName("key");

            //Stuff below would be true if the class derived from ILocalisationConfiguration; this does not seem to work though...
            //and need to set the mapping explicitly. Looks like EF is not always happy with the interfaces.
            //Note: Translations dobe via ILocalisationConfiguration
            Property(p => p.Translations.Serialised).HasColumnName("translations");

            //indexes
            Property(en => en.LocalisationClassUuid)
                .HasColumnAnnotation(
                    "Index",
                    new IndexAnnotation(new IndexAttribute("uq_localisation_class_translation_key") { IsUnique = true, Order = 1 }));
            Property(en => en.Key)
                .HasColumnAnnotation(
                    "Index",
                    new IndexAnnotation(new IndexAttribute("uq_localisation_class_translation_key") { IsUnique = true, Order = 2 }));
        }
        
    }
}
