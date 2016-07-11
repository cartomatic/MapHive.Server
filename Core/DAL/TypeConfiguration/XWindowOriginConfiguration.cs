using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapHive.Server.Core.DataModel;
using MapHive.Server.Core.DataModel.Interface;

namespace MapHive.Server.Core.DAL.TypeConfiguration
{
    public class XWindowOriginConfiguration : EntityTypeConfiguration<XWindowOrigin> 
        //Note:
        //Deriving from ILocalisationConfiguration<DataModel.AppLocalisation> does not work. EF needs a concrete type nd throws otherwise
    {
        public XWindowOriginConfiguration()
        {
            ToTable("xwindow_origins", "mh_meta");
            this.ApplyIBaseConfiguration();

            Property(en => en.Origin).HasColumnName("origin");
            Property(en => en.Description).HasColumnName("description");
            Property(en => en.Custom).HasColumnName("custom");
        }
    }
}
