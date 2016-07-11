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
    public class EmailTemplateLocalisationConfiguration : EntityTypeConfiguration<EmailTemplateLocalisation>
    //Note:
    //Deriving from ILocalisationConfiguration<DataModel.AppLocalisation> does not work. EF needs a concrete type nd throws otherwise
    {
        public EmailTemplateLocalisationConfiguration()
        {
            ToTable("localisation_email_templates", "mh_meta");
            this.ApplyIBaseConfiguration(nameof(Lang));

            Property(en => en.ApplicationName).HasColumnName("application_name");
            Property(en => en.Name).HasColumnName("name");
            Property(en => en.Description).HasColumnName("description");
            Property(en => en.Identifier).HasColumnName("identifier");
            Property(en => en.IsBodyHtml).HasColumnName("is_body_html");

            //Stuff below would be true if the class derived from ILocalisationConfiguration; this does not seem to work though...
            //Note: Translations dobe via ILocalisationConfiguration
            Property(p => p.Translations.Serialised).HasColumnName("translations");


            Property(t => t.ApplicationName).HasColumnAnnotation(
                "Index", new IndexAnnotation(new IndexAttribute("idx_application")));

            //make a compond unique key 


            Property(t => t.ApplicationName)
                .HasColumnAnnotation(
                    "Index",
                    new IndexAnnotation(new IndexAttribute("uq_app_name_and_identifier") { IsUnique = true, Order = 1 }));
            Property(t => t.Identifier)
                .HasColumnAnnotation(
                    "Index",
                    new IndexAnnotation(new IndexAttribute("uq_app_name_and_identifier") { IsUnique = true, Order = 2 }));
        }
    }
}
