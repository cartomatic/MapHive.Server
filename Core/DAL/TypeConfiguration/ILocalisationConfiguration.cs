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
    public class ILocalisationConfiguration<T> : EntityTypeConfiguration<T>
        where T: class, ILocalisation
    {
        protected ILocalisationConfiguration()
        {
            Property(p => p.Translations.Serialised).HasColumnName("translations");
        }
    }
}
