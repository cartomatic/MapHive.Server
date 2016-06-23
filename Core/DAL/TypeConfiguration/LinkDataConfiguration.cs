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
    public class LinkDataConfiguration : ComplexTypeConfiguration<LinkData>
    {
        public LinkDataConfiguration()
        {
            Property(p => p.Serialised).HasColumnName("link_json_data");

            //Q: will this work??? dunno... need to find out.

            //modelBuilder.ComplexType<Type>()
            //.Property(p => p.Serialized)
            //.HasColumnName("link_json_data");
        }
    }
}
