namespace MapHive.Server.Core.DAL.Migrations.MapHiveMetaConfiguration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewModels_User_Org_Role_etc : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "mh_meta.organisations",
                c => new
                    {
                        uuid = c.Guid(nullable: false),
                        slug = c.String(),
                        display_name = c.String(),
                        description = c.String(),
                        location = c.String(),
                        url = c.String(),
                        email = c.String(),
                        gravatar_email = c.String(),
                        profile_picture_id = c.Guid(),
                        billing_email = c.String(),
                        billing_address = c.String(),
                        billing_extra_info = c.String(),
                        created_by = c.Guid(),
                        last_modified_by = c.Guid(),
                        create_date_utc = c.DateTime(),
                        modify_date_utc = c.DateTime(),
                        end_date_utc = c.DateTime(),
                    })
                .PrimaryKey(t => t.uuid)
                .Index(t => t.slug, unique: true, name: "idx_slug_organisation")
                .Index(t => t.create_date_utc, name: "idx_create_date_organisation");
            
            CreateTable(
                "mh_meta.roles",
                c => new
                    {
                        uuid = c.Guid(nullable: false),
                        identifier = c.String(),
                        name = c.String(),
                        description = c.String(),
                        created_by = c.Guid(),
                        last_modified_by = c.Guid(),
                        create_date_utc = c.DateTime(),
                        modify_date_utc = c.DateTime(),
                        end_date_utc = c.DateTime(),
                    })
                .PrimaryKey(t => t.uuid)
                .Index(t => t.create_date_utc, name: "idx_create_date_role");
            
            CreateTable(
                "mh_meta.teams",
                c => new
                    {
                        uuid = c.Guid(nullable: false),
                        name = c.String(),
                        description = c.String(),
                        created_by = c.Guid(),
                        last_modified_by = c.Guid(),
                        create_date_utc = c.DateTime(),
                        modify_date_utc = c.DateTime(),
                        end_date_utc = c.DateTime(),
                    })
                .PrimaryKey(t => t.uuid)
                .Index(t => t.create_date_utc, name: "idx_create_date_role");
            
            AddColumn("mh_meta.users", "slug", c => c.String());
            AddColumn("mh_meta.users", "bio", c => c.String());
            AddColumn("mh_meta.users", "company", c => c.String());
            AddColumn("mh_meta.users", "department", c => c.String());
            AddColumn("mh_meta.users", "location", c => c.String());
            AddColumn("mh_meta.users", "gravatar_email", c => c.String());
            AddColumn("mh_meta.users", "profile_picture_id", c => c.Guid());
            AddColumn("mh_meta.users", "is_org_user", c => c.Boolean(nullable: false));
            CreateIndex("mh_meta.users", "slug", unique: true, name: "idx_slug_maphiveuser");
        }
        
        public override void Down()
        {
            DropIndex("mh_meta.users", "idx_slug_maphiveuser");
            DropIndex("mh_meta.teams", "idx_create_date_role");
            DropIndex("mh_meta.roles", "idx_create_date_role");
            DropIndex("mh_meta.organisations", "idx_create_date_organisation");
            DropIndex("mh_meta.organisations", "idx_slug_organisation");
            DropColumn("mh_meta.users", "is_org_user");
            DropColumn("mh_meta.users", "profile_picture_id");
            DropColumn("mh_meta.users", "gravatar_email");
            DropColumn("mh_meta.users", "location");
            DropColumn("mh_meta.users", "department");
            DropColumn("mh_meta.users", "company");
            DropColumn("mh_meta.users", "bio");
            DropColumn("mh_meta.users", "slug");
            DropTable("mh_meta.teams");
            DropTable("mh_meta.roles");
            DropTable("mh_meta.organisations");
        }
    }
}
