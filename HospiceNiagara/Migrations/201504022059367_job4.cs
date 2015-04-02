namespace HospiceNiagara.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class job4 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Contact", name: "Job_ID", newName: "JobDescription_ID");
            RenameIndex(table: "dbo.Contact", name: "IX_Job_ID", newName: "IX_JobDescription_ID");
            DropColumn("dbo.Contact", "Position");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Contact", "Position", c => c.String(nullable: false));
            RenameIndex(table: "dbo.Contact", name: "IX_JobDescription_ID", newName: "IX_Job_ID");
            RenameColumn(table: "dbo.Contact", name: "JobDescription_ID", newName: "Job_ID");
        }
    }
}
