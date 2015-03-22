namespace HospiceNiagara.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class undo : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.FileStore", "Category");
            DropColumn("dbo.FileStore", "Month");
            DropColumn("dbo.FileStore", "Year");
            DropColumn("dbo.FileStore", "Discriminator");
        }
        
        public override void Down()
        {
            AddColumn("dbo.FileStore", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.FileStore", "Year", c => c.Int());
            AddColumn("dbo.FileStore", "Month", c => c.String());
            AddColumn("dbo.FileStore", "Category", c => c.String());
        }
    }
}
