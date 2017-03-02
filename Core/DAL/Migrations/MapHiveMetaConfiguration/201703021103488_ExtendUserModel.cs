namespace MapHive.Server.Core.DAL.Migrations.MapHiveMetaConfiguration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExtendUserModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("mh_meta.users", "VisibleInCatalogue", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("mh_meta.users", "VisibleInCatalogue");
        }
    }
}
