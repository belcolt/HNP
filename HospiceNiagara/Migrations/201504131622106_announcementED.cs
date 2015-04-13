namespace HospiceNiagara.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class announcementED : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Announcement", "ExpiryDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Announcement", "ExpiryDate");
        }
    }
}
