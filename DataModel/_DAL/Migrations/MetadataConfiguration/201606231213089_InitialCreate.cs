namespace MapHive.Server.DataModel.DAL.Migrations.MetadataConfiguration
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
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
                        short_name = c.String(
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "idx",
                                    new AnnotationValues(oldValue: null, newValue: "IndexAnnotation: { Name: uq_short_name, IsUnique: True }")
                                },
                            }),
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
                .PrimaryKey(t => t.uuid);
            
            CreateTable(
                "metadata.links",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        parent_uuid = c.Guid(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "idx",
                                    new AnnotationValues(oldValue: null, newValue: "IndexAnnotation: { Name: parent_uuid, IsUnique: True }")
                                },
                            }),
                        child_uuid = c.Guid(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "idx",
                                    new AnnotationValues(oldValue: null, newValue: "IndexAnnotation: { Name: child_uuid, IsUnique: True }")
                                },
                            }),
                        parent_type_uuid = c.Guid(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "idx",
                                    new AnnotationValues(oldValue: null, newValue: "IndexAnnotation: { Name: parent_type_uuid, IsUnique: True }")
                                },
                            }),
                        child_type_uuid = c.Guid(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "idx",
                                    new AnnotationValues(oldValue: null, newValue: "IndexAnnotation: { Name: child_type_uuid, IsUnique: True }")
                                },
                            }),
                        sort_order = c.Int(),
                        link_json_data = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "metadata.users",
                c => new
                    {
                        uuid = c.Guid(nullable: false),
                        forename = c.String(),
                        surname = c.String(),
                        email = c.String(
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "idx",
                                    new AnnotationValues(oldValue: null, newValue: "IndexAnnotation: { Name: uq_email, IsUnique: True }")
                                },
                            }),
                        is_account_closed = c.Boolean(nullable: false),
                        is_account_verified = c.Boolean(nullable: false),
                        created_by = c.Guid(),
                        last_modified_by = c.Guid(),
                        create_date = c.DateTime(),
                        modify_date = c.DateTime(),
                        end_date = c.DateTime(),
                    })
                .PrimaryKey(t => t.uuid);
            
        }
        
        public override void Down()
        {
            DropTable("metadata.users",
                removedColumnAnnotations: new Dictionary<string, IDictionary<string, object>>
                {
                    {
                        "email",
                        new Dictionary<string, object>
                        {
                            { "idx", "IndexAnnotation: { Name: uq_email, IsUnique: True }" },
                        }
                    },
                });
            DropTable("metadata.links",
                removedColumnAnnotations: new Dictionary<string, IDictionary<string, object>>
                {
                    {
                        "child_type_uuid",
                        new Dictionary<string, object>
                        {
                            { "idx", "IndexAnnotation: { Name: child_type_uuid, IsUnique: True }" },
                        }
                    },
                    {
                        "child_uuid",
                        new Dictionary<string, object>
                        {
                            { "idx", "IndexAnnotation: { Name: child_uuid, IsUnique: True }" },
                        }
                    },
                    {
                        "parent_type_uuid",
                        new Dictionary<string, object>
                        {
                            { "idx", "IndexAnnotation: { Name: parent_type_uuid, IsUnique: True }" },
                        }
                    },
                    {
                        "parent_uuid",
                        new Dictionary<string, object>
                        {
                            { "idx", "IndexAnnotation: { Name: parent_uuid, IsUnique: True }" },
                        }
                    },
                });
            DropTable("metadata.applications",
                removedColumnAnnotations: new Dictionary<string, IDictionary<string, object>>
                {
                    {
                        "short_name",
                        new Dictionary<string, object>
                        {
                            { "idx", "IndexAnnotation: { Name: uq_short_name, IsUnique: True }" },
                        }
                    },
                });
        }
    }
}
