namespace MapHive.Server.Core.DAL.Migrations.MapHiveMetaConfiguration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "mh_meta.applications",
                c => new
                    {
                        uuid = c.Guid(nullable: false),
                        short_name = c.String(),
                        name = c.String(),
                        description = c.String(),
                        urls = c.String(),
                        use_splashscreen = c.Boolean(nullable: false),
                        requires_auth = c.Boolean(nullable: false),
                        is_common = c.Boolean(nullable: false),
                        is_default = c.Boolean(nullable: false),
                        is_hidden = c.Boolean(nullable: false),
                        created_by = c.Guid(),
                        last_modified_by = c.Guid(),
                        create_date_utc = c.DateTime(),
                        modify_date_utc = c.DateTime(),
                        end_date_utc = c.DateTime(),
                    })
                .PrimaryKey(t => t.uuid)
                .Index(t => t.short_name, unique: true, name: "uq_short_name")
                .Index(t => t.create_date_utc, name: "idx_create_date_application");
            
            CreateTable(
                "mh_localisation.email_templates",
                c => new
                    {
                        uuid = c.Guid(nullable: false),
                        name = c.String(),
                        description = c.String(),
                        application_name = c.String(),
                        identifier = c.String(),
                        is_body_html = c.Boolean(nullable: false),
                        translations = c.String(),
                        created_by = c.Guid(),
                        last_modified_by = c.Guid(),
                        create_date_utc = c.DateTime(),
                        modify_date_utc = c.DateTime(),
                        end_date_utc = c.DateTime(),
                    })
                .PrimaryKey(t => t.uuid)
                .Index(t => new { t.application_name, t.identifier }, unique: true, name: "uq_app_name_and_identifier")
                .Index(t => t.create_date_utc, name: "idx_create_date_lang");
            
            CreateTable(
                "mh_localisation.langs",
                c => new
                    {
                        uuid = c.Guid(nullable: false),
                        lang_code = c.String(),
                        name = c.String(),
                        description = c.String(),
                        is_default = c.Boolean(nullable: false),
                        created_by = c.Guid(),
                        last_modified_by = c.Guid(),
                        create_date_utc = c.DateTime(),
                        modify_date_utc = c.DateTime(),
                        end_date_utc = c.DateTime(),
                    })
                .PrimaryKey(t => t.uuid)
                .Index(t => t.create_date_utc, name: "idx_create_date_lang");
            
            CreateTable(
                "mh_meta.links",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        parent_uuid = c.Guid(nullable: false),
                        child_uuid = c.Guid(nullable: false),
                        parent_type_uuid = c.Guid(nullable: false),
                        child_type_uuid = c.Guid(nullable: false),
                        sort_order = c.Int(),
                        link_json_data = c.String(),
                    })
                .PrimaryKey(t => t.id)
                .Index(t => t.parent_uuid, name: "idx_parent_uuid")
                .Index(t => t.child_uuid, name: "idx_child_uuid")
                .Index(t => t.parent_type_uuid, name: "idx_parent_type_uuid")
                .Index(t => t.child_type_uuid, name: "idx_child_type_uuid");
            
            CreateTable(
                "mh_localisation.localisation_classes",
                c => new
                    {
                        uuid = c.Guid(nullable: false),
                        application_name = c.String(),
                        class_name = c.String(),
                        inherited_class_name = c.String(),
                        created_by = c.Guid(),
                        last_modified_by = c.Guid(),
                        create_date_utc = c.DateTime(),
                        modify_date_utc = c.DateTime(),
                        end_date_utc = c.DateTime(),
                    })
                .PrimaryKey(t => t.uuid)
                .Index(t => new { t.application_name, t.class_name }, unique: true, name: "uq_app_name_class_name")
                .Index(t => t.create_date_utc, name: "idx_create_date_localisationclass");
            
            CreateTable(
                "mh_localisation.translation_keys",
                c => new
                    {
                        uuid = c.Guid(nullable: false),
                        localisation_class_uuid = c.Guid(nullable: false),
                        key = c.String(),
                        translations = c.String(),
                        created_by = c.Guid(),
                        last_modified_by = c.Guid(),
                        create_date_utc = c.DateTime(),
                        modify_date_utc = c.DateTime(),
                        end_date_utc = c.DateTime(),
                    })
                .PrimaryKey(t => t.uuid)
                .Index(t => new { t.localisation_class_uuid, t.key }, unique: true, name: "uq_localisation_class_translation_key")
                .Index(t => t.create_date_utc, name: "idx_create_date_applocalisation");
            
            CreateTable(
                "mh_meta.users",
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
                        create_date_utc = c.DateTime(),
                        modify_date_utc = c.DateTime(),
                        end_date_utc = c.DateTime(),
                    })
                .PrimaryKey(t => t.uuid)
                .Index(t => t.email, name: "uq_email_t")
                .Index(t => t.create_date_utc, name: "idx_create_date_maphiveuser");
            
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
                .PrimaryKey(t => t.uuid)
                .Index(t => t.create_date_utc, name: "idx_create_date_xwindoworigin");
            
        }
        
        public override void Down()
        {
            DropIndex("mh_meta.xwindow_origins", "idx_create_date_xwindoworigin");
            DropIndex("mh_meta.users", "idx_create_date_maphiveuser");
            DropIndex("mh_meta.users", "uq_email_t");
            DropIndex("mh_localisation.translation_keys", "idx_create_date_applocalisation");
            DropIndex("mh_localisation.translation_keys", "uq_localisation_class_translation_key");
            DropIndex("mh_localisation.localisation_classes", "idx_create_date_localisationclass");
            DropIndex("mh_localisation.localisation_classes", "uq_app_name_class_name");
            DropIndex("mh_meta.links", "idx_child_type_uuid");
            DropIndex("mh_meta.links", "idx_parent_type_uuid");
            DropIndex("mh_meta.links", "idx_child_uuid");
            DropIndex("mh_meta.links", "idx_parent_uuid");
            DropIndex("mh_localisation.langs", "idx_create_date_lang");
            DropIndex("mh_localisation.email_templates", "idx_create_date_lang");
            DropIndex("mh_localisation.email_templates", "uq_app_name_and_identifier");
            DropIndex("mh_meta.applications", "idx_create_date_application");
            DropIndex("mh_meta.applications", "uq_short_name");
            DropTable("mh_meta.xwindow_origins");
            DropTable("mh_meta.users");
            DropTable("mh_localisation.translation_keys");
            DropTable("mh_localisation.localisation_classes");
            DropTable("mh_meta.links");
            DropTable("mh_localisation.langs");
            DropTable("mh_localisation.email_templates");
            DropTable("mh_meta.applications");
        }
    }
}
