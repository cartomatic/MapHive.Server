namespace MapHive.Server.Core.DAL.Migrations.MapHiveMetaConfiguration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class orgIdentifierAddedToUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("mh_meta.users", "user_org_id", c => c.Guid());
        }
        
        public override void Down()
        {
            DropColumn("mh_meta.users", "user_org_id");
        }
    }
}
