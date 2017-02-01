namespace MapHive.Server.Core.DAL.Migrations.MapHiveMetaConfiguration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveXWindow_UpdateApplicationModel : DbMigration
    {
        public override void Up()
        {
            DropIndex("mh_meta.xwindow_origins", "idx_create_date_xwindoworigin");
            AddColumn("mh_meta.applications", "is_home", c => c.Boolean(nullable: false));
            AddColumn("mh_meta.applications", "is_hive", c => c.Boolean(nullable: false));
            AddColumn("mh_meta.applications", "provider_id", c => c.Guid());
            DropColumn("mh_meta.applications", "is_hidden");
            DropTable("mh_meta.xwindow_origins");
        }
        
        public override void Down()
        {
            CreateTable(
                "mh_meta.xwindow_origins",
                c => new
                    {
                        uuid = c.Guid(nullable: false),
                        origin = c.String(),
                        description = c.String(),
                        custom = c.Boolean(nullable: false),
                        created_by = c.Guid(),
                        last_modified_by = c.Guid(),
                        create_date_utc = c.DateTime(),
                        modify_date_utc = c.DateTime(),
                        end_date_utc = c.DateTime(),
                    })
                .PrimaryKey(t => t.uuid);
            
            AddColumn("mh_meta.applications", "is_hidden", c => c.Boolean(nullable: false));
            DropColumn("mh_meta.applications", "provider_id");
            DropColumn("mh_meta.applications", "is_hive");
            DropColumn("mh_meta.applications", "is_home");
            CreateIndex("mh_meta.xwindow_origins", "create_date_utc", name: "idx_create_date_xwindoworigin");
        }
    }
}
