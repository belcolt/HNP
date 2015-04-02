namespace HospiceNiagara.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class job5 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Contact", "JobDescription_ID", "dbo.JobDescription");
            DropIndex("dbo.Contact", new[] { "JobDescription_ID" });
            RenameColumn(table: "dbo.Contact", name: "JobDescription_ID", newName: "JobDescriptionID");
            AlterColumn("dbo.Contact", "JobDescriptionID", c => c.Int(nullable: false));
            CreateIndex("dbo.Contact", "JobDescriptionID");
            AddForeignKey("dbo.Contact", "JobDescriptionID", "dbo.JobDescription", "ID", cascadeDelete: true);
            DropColumn("dbo.Contact", "PositionID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Contact", "PositionID", c => c.Int(nullable: false));
            DropForeignKey("dbo.Contact", "JobDescriptionID", "dbo.JobDescription");
            DropIndex("dbo.Contact", new[] { "JobDescriptionID" });
            AlterColumn("dbo.Contact", "JobDescriptionID", c => c.Int());
            RenameColumn(table: "dbo.Contact", name: "JobDescriptionID", newName: "JobDescription_ID");
            CreateIndex("dbo.Contact", "JobDescription_ID");
            AddForeignKey("dbo.Contact", "JobDescription_ID", "dbo.JobDescription", "ID");
        }
    }
}
