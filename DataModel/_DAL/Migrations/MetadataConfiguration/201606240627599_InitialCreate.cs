namespace MapHive.Server.DataModel.DAL.Migrations.MetadataConfiguration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "metadata.applications",
                c => new
                    {
                        uuid = c.Guid(nullable: false),
                        short_name = c.String(),
                        name = c.String(),
                        description = c.String(),
                        url = c.String(),
                        use_splashscreen = c.Boolean(nullable: false),
                        requires_auth = c.Boolean(nullable: false),
                        is_common = c.Boolean(nullable: false),
                        is_default = c.Boolean(nullable: false),
                        created_by = c.Guid(),
                        last_modified_by = c.Guid(),
                        create_date = c.DateTime(),
                        modify_date = c.DateTime(),
                        end_date = c.DateTime(),
                    })
                .PrimaryKey(t => t.uuid)
                .Index(t => t.short_name, unique: true, name: "uq_short_name");
            
            CreateTable(
                "metadata.app_localisations",
                c => new
                    {
                        uuid = c.Guid(nullable: false),
                        application_name = c.String(),
                        class_name = c.String(),
                        translation_key = c.String(),
                        translations = c.String(),
                        created_by = c.Guid(),
                        last_modified_by = c.Guid(),
                        create_date = c.DateTime(),
                        modify_date = c.DateTime(),
                        end_date = c.DateTime(),
                    })
                .PrimaryKey(t => t.uuid);
            
            CreateTable(
                "metadata.email_template_localisations",
                c => new
                    {
                        uuid = c.Guid(nullable: false),
                        name = c.String(),
                        description = c.String(),
                        application_name = c.String(),
                        identifier = c.String(),
                        is_email_html = c.Boolean(nullable: false),
                        translations = c.String(),
                        created_by = c.Guid(),
                        last_modified_by = c.Guid(),
                        create_date = c.DateTime(),
                        modify_date = c.DateTime(),
                        end_date = c.DateTime(),
                    })
                .PrimaryKey(t => t.uuid)
                .Index(t => new { t.application_name, t.identifier }, unique: true, name: "uq_app_name_and_identifier");
            
            CreateTable(
                "metadata.links",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        parent_uuid = c.Guid(nullable: false),
                        child_uuid = c.Guid(nullable: false),
                        parent_type_uuid = c.Guid(nullable: false),
                        child_type_uuid = c.Guid(nullable: false),
                        sort_order = c.Int(),
                        link_json_data = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.parent_uuid, unique: true, name: "idx_parent_uuid")
                .Index(t => t.child_uuid, unique: true, name: "idx_child_uuid")
                .Index(t => t.parent_type_uuid, unique: true, name: "idx_parent_type_uuid")
                .Index(t => t.child_type_uuid, unique: true, name: "idx_child_type_uuid");
            
            CreateTable(
                "metadata.users",
                c => new
                    {
                        uuid = c.Guid(nullable: false),
                        forename = c.String(),
                        surname = c.String(),
                        email = c.String(),
                        is_account_closed = c.Boolean(nullable: false),
                        is_account_verified = c.Boolean(nullable: false),
                        created_by = c.Guid(),
                        last_modified_by = c.Guid(),
                        create_date = c.DateTime(),
                        modify_date = c.DateTime(),
                        end_date = c.DateTime(),
                    })
                .PrimaryKey(t => t.uuid)
                .Index(t => t.email, unique: true, name: "uq_email");
            
        }
        
        public override void Down()
        {
            DropIndex("metadata.users", "uq_email");
            DropIndex("metadata.links", "idx_child_type_uuid");
            DropIndex("metadata.links", "idx_parent_type_uuid");
            DropIndex("metadata.links", "idx_child_uuid");
            DropIndex("metadata.links", "idx_parent_uuid");
            DropIndex("metadata.email_template_localisations", "uq_app_name_and_identifier");
            DropIndex("metadata.applications", "uq_short_name");
            DropTable("metadata.users");
            DropTable("metadata.links");
            DropTable("metadata.email_template_localisations");
            DropTable("metadata.app_localisations");
            DropTable("metadata.applications");
        }
    }
}
