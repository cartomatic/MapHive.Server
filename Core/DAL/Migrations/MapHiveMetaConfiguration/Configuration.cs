using System.CodeDom;
using System.Collections.Generic;
using MapHive.Server.Core.DataModel;
using MapHive.Server.Core.DAL.DbContext;
using MapHive.Server.Core.DAL.Interface;
using MapHive.Server.Core.Utils;

namespace MapHive.Server.Core.DAL.Migrations.MapHiveMetaConfiguration
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<MapHiveDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "MapHive.Server.Core.DAL.DbContext.MapHiveDbContext";
            MigrationsDirectory = @"DAL\Migrations\MapHiveMetaConfiguration";
        }

        protected override void Seed(MapHiveDbContext context)
        {
            Identity.ImpersonateGhostUser();

            DAL.Seed.MapHiveMeta.Seed.SeedAll(context);
        }
    }
}
