using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MapHive.Server.Core.DAL.DbContext;

namespace MapHive.Server.Core.DAL.Seed.MapHiveMeta
{
    public partial class Seed
    {
        public static void SeedAll(MapHiveDbContext context)
        {
            var mi = typeof(Seed).GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);

            foreach (var m in mi)
            {
                if (m.Name != nameof(SeedAll) && m.Name.StartsWith("Seed"))
                {
                    //assume there is one param for the time being
                    //may have to change it later
                    m.Invoke(null, new object[] { context });
                }
            }
        }

    }
}
