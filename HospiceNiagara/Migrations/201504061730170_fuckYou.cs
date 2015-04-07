namespace HospiceNiagara.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fuckYou : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Announcement", "Resource_ID", "dbo.Resource");
            DropIndex("dbo.Announcement", new[] { "Resource_ID" });
            RenameColumn(table: "dbo.Announcement", name: "Resource_ID", newName: "ResourceID");
            AlterColumn("dbo.Announcement", "ResourceID", c => c.Int(nullable: false));
            CreateIndex("dbo.Announcement", "ResourceID");
            AddForeignKey("dbo.Announcement", "ResourceID", "dbo.Resource", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Announcement", "ResourceID", "dbo.Resource");
            DropIndex("dbo.Announcement", new[] { "ResourceID" });
            AlterColumn("dbo.Announcement", "ResourceID", c => c.Int());
            RenameColumn(table: "dbo.Announcement", name: "ResourceID", newName: "Resource_ID");
            CreateIndex("dbo.Announcement", "Resource_ID");
            AddForeignKey("dbo.Announcement", "Resource_ID", "dbo.Resource", "ID");
        }
    }
}
