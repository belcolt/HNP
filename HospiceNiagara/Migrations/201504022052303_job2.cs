namespace HospiceNiagara.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class job2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contact", "PositionID", c => c.Int(nullable: false));
            DropColumn("dbo.Contact", "JobID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Contact", "JobID", c => c.Int(nullable: false));
            DropColumn("dbo.Contact", "PositionID");
        }
    }
}
