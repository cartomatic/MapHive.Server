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
        public static void SeedXWindowOrigins(MapHiveDbContext context)
        {
            //obtain the origins first
            var origins = new List<string>();

            //Note: context may have not seeded the apps yet, so need to use a bit less dynamic resource here ;)
            //foreach (var app in context.Applications)
            foreach (var app in GetApps())
            {
                var urls = app.Urls.Split('|');

                foreach (var url in urls)
                {
                    var uri = new Uri(url);
                    if (!origins.Contains(uri.Host))
                    {
                        origins.Add(uri.Host);
                    }
                }
            }

            foreach (var origin in origins)
            {
                if (!context.XWindowOrigins.Any(xwo => xwo.Origin == origin))
                {
                    context.XWindowOrigins.AddOrUpdate(
                        new XWindowOrigin
                        {
                            Uuid = Guid.NewGuid(),
                            Origin = origin
                        }
                    );
                }
            }
        }
    }
}
