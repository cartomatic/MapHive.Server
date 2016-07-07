namespace MapHive.Server.DataModel.DAL.Migrations.MetadataConfiguration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmailTemplateLocalisationModelChange : DbMigration
    {
        public override void Up()
        {
            AddColumn("metadata.localisation_email_templates", "is_body_html", c => c.Boolean(nullable: false));
            DropColumn("metadata.localisation_email_templates", "is_email_html");
        }
        
        public override void Down()
        {
            AddColumn("metadata.localisation_email_templates", "is_email_html", c => c.Boolean(nullable: false));
            DropColumn("metadata.localisation_email_templates", "is_body_html");
        }
    }
}
