namespace HospiceNiagara.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Announcement",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Content = c.String(),
                        Date = c.DateTime(nullable: false),
                        Resource_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Resource", t => t.Resource_ID)
                .Index(t => t.Resource_ID);
            
            CreateTable(
                "dbo.Resource",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FileDesc = c.String(nullable: false),
                        DateAdded = c.DateTime(nullable: false),
                        ResourceCategoryID = c.Int(nullable: false),
                        ResourceSubCategoryID = c.Int(),
                        FileStoreID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.FileStore", t => t.FileStoreID, cascadeDelete: true)
                .ForeignKey("dbo.ResourceCategory", t => t.ResourceCategoryID, cascadeDelete: true)
                .ForeignKey("dbo.ResourceSubCategory", t => t.ResourceSubCategoryID)
                .Index(t => t.ResourceCategoryID)
                .Index(t => t.ResourceSubCategoryID)
                .Index(t => t.FileStoreID);
            
            CreateTable(
                "dbo.FileStore",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FileContent = c.Binary(nullable: false),
                        MimeType = c.String(nullable: false, maxLength: 256),
                        FileName = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ResourceCategory",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        TeamDomainID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.TeamDomain", t => t.TeamDomainID, cascadeDelete: true)
                .Index(t => new { t.Name, t.TeamDomainID }, unique: true, name: "IX_ResouceDomCat_Unique");
            
            CreateTable(
                "dbo.TeamDomain",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.Description, unique: true, name: "IX_TeamDomain_Unique");
            
            CreateTable(
                "dbo.ResourceSubCategory",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        ResourceCategoryID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ResourceCategory", t => t.ResourceCategoryID, cascadeDelete: true)
                .Index(t => t.Name, unique: true, name: "IX_ResouceSubCat_Unique")
                .Index(t => t.ResourceCategoryID);
            
            CreateTable(
                "dbo.Contact",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        Position = c.String(nullable: false),
                        Phone = c.String(nullable: false, maxLength: 10),
                        Email = c.String(),
                        IsBoardDirector = c.Boolean(nullable: false),
                        TeamDomainID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.TeamDomain", t => t.TeamDomainID, cascadeDelete: true)
                .Index(t => t.TeamDomainID);
            
            CreateTable(
                "dbo.Invitation",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        RSVP = c.Boolean(nullable: false),
                        HasResponded = c.Boolean(nullable: false),
                        ContactID = c.Int(nullable: false),
                        EventID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Contact", t => t.ContactID, cascadeDelete: true)
                .ForeignKey("dbo.Event", t => t.EventID, cascadeDelete: true)
                .Index(t => t.ContactID)
                .Index(t => t.EventID);
            
            CreateTable(
                "dbo.Event",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        StartDateTime = c.DateTime(nullable: false),
                        EndDateTime = c.DateTime(nullable: false),
                        Location = c.String(nullable: false),
                        VolunteersNeeded = c.Boolean(nullable: false),
                        BrochureID = c.Int(),
                        Requirements = c.String(),
                        Notes = c.String(),
                        AgendaId = c.Int(),
                        MinutesID = c.Int(),
                        StaffLead = c.String(),
                        AttendanceID = c.Int(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Resource", t => t.AgendaId)
                .ForeignKey("dbo.MeetingResource", t => t.AttendanceID)
                .ForeignKey("dbo.Resource", t => t.BrochureID)
                .ForeignKey("dbo.MeetingResource", t => t.MinutesID)
                .Index(t => t.BrochureID)
                .Index(t => t.AgendaId)
                .Index(t => t.MinutesID)
                .Index(t => t.AttendanceID);
            
            CreateTable(
                "dbo.MeetingResource",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        MeetingID = c.Int(nullable: false),
                        ResourceID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Event", t => t.MeetingID, cascadeDelete: true)
                .ForeignKey("dbo.Resource", t => t.ResourceID, cascadeDelete: true)
                .Index(t => t.MeetingID)
                .Index(t => t.ResourceID);
            
            CreateTable(
                "dbo.DeathNotice",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 30),
                        MiddleName = c.String(maxLength: 30),
                        LastName = c.String(nullable: false, maxLength: 50),
                        Date = c.DateTime(nullable: false),
                        Location = c.String(maxLength: 50),
                        Notes = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Schedule",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Category = c.String(),
                        Month = c.String(),
                        Year = c.Int(nullable: false),
                        ResourceID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Resource", t => t.ResourceID, cascadeDelete: true)
                .Index(t => t.ResourceID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Schedule", "ResourceID", "dbo.Resource");
            DropForeignKey("dbo.Contact", "TeamDomainID", "dbo.TeamDomain");
            DropForeignKey("dbo.Event", "MinutesID", "dbo.MeetingResource");
            DropForeignKey("dbo.Invitation", "EventID", "dbo.Event");
            DropForeignKey("dbo.Event", "BrochureID", "dbo.Resource");
            DropForeignKey("dbo.Event", "AttendanceID", "dbo.MeetingResource");
            DropForeignKey("dbo.MeetingResource", "ResourceID", "dbo.Resource");
            DropForeignKey("dbo.MeetingResource", "MeetingID", "dbo.Event");
            DropForeignKey("dbo.Event", "AgendaId", "dbo.Resource");
            DropForeignKey("dbo.Invitation", "ContactID", "dbo.Contact");
            DropForeignKey("dbo.Announcement", "Resource_ID", "dbo.Resource");
            DropForeignKey("dbo.Resource", "ResourceSubCategoryID", "dbo.ResourceSubCategory");
            DropForeignKey("dbo.ResourceSubCategory", "ResourceCategoryID", "dbo.ResourceCategory");
            DropForeignKey("dbo.Resource", "ResourceCategoryID", "dbo.ResourceCategory");
            DropForeignKey("dbo.ResourceCategory", "TeamDomainID", "dbo.TeamDomain");
            DropForeignKey("dbo.Resource", "FileStoreID", "dbo.FileStore");
            DropIndex("dbo.Schedule", new[] { "ResourceID" });
            DropIndex("dbo.MeetingResource", new[] { "ResourceID" });
            DropIndex("dbo.MeetingResource", new[] { "MeetingID" });
            DropIndex("dbo.Event", new[] { "AttendanceID" });
            DropIndex("dbo.Event", new[] { "MinutesID" });
            DropIndex("dbo.Event", new[] { "AgendaId" });
            DropIndex("dbo.Event", new[] { "BrochureID" });
            DropIndex("dbo.Invitation", new[] { "EventID" });
            DropIndex("dbo.Invitation", new[] { "ContactID" });
            DropIndex("dbo.Contact", new[] { "TeamDomainID" });
            DropIndex("dbo.ResourceSubCategory", new[] { "ResourceCategoryID" });
            DropIndex("dbo.ResourceSubCategory", "IX_ResouceSubCat_Unique");
            DropIndex("dbo.TeamDomain", "IX_TeamDomain_Unique");
            DropIndex("dbo.ResourceCategory", "IX_ResouceDomCat_Unique");
            DropIndex("dbo.Resource", new[] { "FileStoreID" });
            DropIndex("dbo.Resource", new[] { "ResourceSubCategoryID" });
            DropIndex("dbo.Resource", new[] { "ResourceCategoryID" });
            DropIndex("dbo.Announcement", new[] { "Resource_ID" });
            DropTable("dbo.Schedule");
            DropTable("dbo.DeathNotice");
            DropTable("dbo.MeetingResource");
            DropTable("dbo.Event");
            DropTable("dbo.Invitation");
            DropTable("dbo.Contact");
            DropTable("dbo.ResourceSubCategory");
            DropTable("dbo.TeamDomain");
            DropTable("dbo.ResourceCategory");
            DropTable("dbo.FileStore");
            DropTable("dbo.Resource");
            DropTable("dbo.Announcement");
        }
    }
}
