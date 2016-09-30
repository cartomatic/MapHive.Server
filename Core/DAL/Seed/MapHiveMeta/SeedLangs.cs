using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MapHive.Server.Core.DataModel;
using MapHive.Server.Core.DAL.DbContext;

namespace MapHive.Server.Core.DAL.Seed.MapHiveMeta
{
    public partial class Seed
    {
        public static void SeedLangs(MapHiveDbContext context)
        {
            context.Langs.AddOrUpdate(new Lang
            {
                Uuid = Guid.Parse("ece753c3-f079-4772-8aa2-0960aeabc94d"),
                LangCode = "pl",
                Name = "Polski"
            },
            new Lang
            {
                Uuid = Guid.Parse("8323d1bb-e6f5-49d3-a441-837017d6e97e"),
                LangCode = "en",
                Name = "English",
                IsDefault = true
            });
        }
    }
}
