namespace HospiceNiagara.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Check : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BoardMember",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        Position = c.String(nullable: false),
                        EmailAddress = c.String(nullable: false),
                        HomeAddress = c.String(nullable: false),
                        BusinessAddress = c.String(nullable: false),
                        HomePhone = c.String(nullable: false, maxLength: 10),
                        BusinessPhone = c.String(nullable: false, maxLength: 10),
                        Fax = c.String(nullable: false, maxLength: 10),
                        PartnerName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.DeathNotice", "URL", c => c.String(maxLength: 2048));
            AddColumn("dbo.Schedule", "IsActiveSchedule", c => c.Boolean(nullable: false));
            AddColumn("dbo.Schedule", "DataAdded", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Announcement", "Content", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Announcement", "Content", c => c.String());
            DropColumn("dbo.Schedule", "DataAdded");
            DropColumn("dbo.Schedule", "IsActiveSchedule");
            DropColumn("dbo.DeathNotice", "URL");
            DropTable("dbo.BoardMember");
        }
    }
}
