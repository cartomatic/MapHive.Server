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
using MapHive.Server.Core.Localisation;

namespace MapHive.Server.Core.DAL.TypeConfiguration
{
    public class EmailTemplateLocalisationConfiguration : ILocalisationConfiguration<DataModel.EmailTemplateLocalisation>
    {
        public EmailTemplateLocalisationConfiguration()
        {
            this.ApplyIBaseConfiguration();

            Property(en => en.ApplicationName).HasColumnName("application_name");
            Property(en => en.Name).HasColumnName("name");
            Property(en => en.Desription).HasColumnName("description");
            Property(en => en.Identifier).HasColumnName("identifier");
            Property(en => en.IsEmailHtml).HasColumnName("is_email_html");

            //Note: Translations dobe via ILocalisationConfiguration

            Property(t => t.ApplicationName).HasColumnAnnotation(
                "Index", new IndexAnnotation(new IndexAttribute("application")));

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
