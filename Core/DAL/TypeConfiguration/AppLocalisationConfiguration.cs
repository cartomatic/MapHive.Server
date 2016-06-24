using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapHive.Server.Core.DataModel;
using MapHive.Server.Core.DataModel.Interface;
using MapHive.Server.Core.Localisation;

namespace MapHive.Server.Core.DAL.TypeConfiguration
{
    public class AppLocalisationConfiguration : ILocalisationConfiguration<DataModel.AppLocalisation>
    {
        public AppLocalisationConfiguration()
        {
            this.ApplyIBaseConfiguration();

            Property(en => en.ApplicationName).HasColumnName("application_name");
            Property(en => en.ClassName).HasColumnName("class_name");
            Property(en => en.TranslationKey).HasColumnName("translation_key");

            //Note: Translations dobe via ILocalisationConfiguration
        }
        
    }
}
