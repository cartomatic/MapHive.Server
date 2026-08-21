using System.Data.Entity.ModelConfiguration;
using MapHive.Server.Core.DataModel;

namespace MapHive.Server.Core.DAL.TypeConfiguration
{
    public class LangConfiguration : EntityTypeConfiguration<Lang> 
        //Note:
        //Deriving from ILocalisationConfiguration<DataModel.AppLocalisation> does not work. EF needs a concrete type nd throws otherwise
    {
        public LangConfiguration()
        {
            ToTable("langs", "mh_localisation");
            this.ApplyIBaseConfiguration(nameof(Lang));

            Property(en => en.LangCode).HasColumnName("lang_code");
            Property(en => en.Name).HasColumnName("name");
            Property(en => en.Description).HasColumnName("description");
            Property(en => en.IsDefault).HasColumnName("is_default");
        }
    }
}
