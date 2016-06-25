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
    //Note:
    //this does not want to worl. EF has some difficulties when too much generics are in game

    public class ILocalisationConfiguration<T> : EntityTypeConfiguration<T>
        where T: class, ILocalisation
    {
        protected ILocalisationConfiguration()
        {
            Property(p => p.Translations.Serialised).HasColumnName("translations");
        }
    }
}
