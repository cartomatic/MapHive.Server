using MapHive.Server.Core.DAL.DbContext;

namespace MapHive.Server.Core.DAL.Migrations.MapHiveMetaConfiguration
{
    using System.Data.Entity.Migrations;

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
            Utils.Identity.ImpersonateGhostUser();

            DAL.Seed.MapHiveMeta.Seed.SeedAll(context);
        }
    }
}
