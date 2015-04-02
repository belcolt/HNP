namespace HospiceNiagara.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class job3 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Contact", name: "Jobs_ID", newName: "Job_ID");
            RenameIndex(table: "dbo.Contact", name: "IX_Jobs_ID", newName: "IX_Job_ID");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Contact", name: "IX_Job_ID", newName: "IX_Jobs_ID");
            RenameColumn(table: "dbo.Contact", name: "Job_ID", newName: "Jobs_ID");
        }
    }
}
