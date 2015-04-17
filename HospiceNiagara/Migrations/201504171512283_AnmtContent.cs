namespace HospiceNiagara.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AnmtContent : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Announcement", "Content", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Announcement", "Content");
        }
    }
}
