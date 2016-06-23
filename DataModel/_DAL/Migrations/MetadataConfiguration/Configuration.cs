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

            //SeedWhatever(context);
        }

        //private void SeedWhatever(MapHiveDbContext context)
        //{
        //    context.SomeType.AddOrUpdate(obj);
        //}
    }
}
