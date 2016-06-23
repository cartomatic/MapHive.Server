using System.Collections.Generic;
using MapHive.Server.Core.DataModel;
using MapHive.Server.Core.Utils;

namespace MapHive.Server.DataModel.DAL.Migrations.MetadataConfiguration
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MapHiveDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "MapHive.Server.DataModel.DAL.MapHiveDbContext";
            MigrationsDirectory = @"_DAL\Migrations\MetadataConfiguration";
        }

        protected override void Seed(MapHiveDbContext context)
        {
            Identity.ImpersonateGhostUser();

            TestLinkSeed(context);
        }

        private void TestLinkSeed(MapHiveDbContext context)
        {
            var l = new Link
            {
                ParentTypeUuid = default(Guid),
                ChildTypeUuid = default(Guid),
                ParentUuid = default(Guid),
                ChildUuid = default(Guid)
            };

            l.LinkData.Add("some_link_data_consumer", new Dictionary<string, object>
            {
                { "prop1", "Some textual property" },
                { "prop2", 123 },
                { "prop3", DateTime.Now }
            });

            context.Links.AddOrUpdate(l);
        }
    }
}
