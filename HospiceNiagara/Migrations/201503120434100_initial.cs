namespace HospiceNiagara.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
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
                        FileDesc = c.String(),
                        ResourceTypeID = c.Int(nullable: false),
                        FileStoreID = c.Int(nullable: false),
                        DateAdded = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.FileStore", t => t.FileStoreID, cascadeDelete: true)
                .ForeignKey("dbo.ResourceType", t => t.ResourceTypeID, cascadeDelete: true)
                .Index(t => t.ResourceTypeID)
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
                "dbo.ResourceType",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Description = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.Description, unique: true, name: "IX_ResourceType_Desc");
            
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
                    })
                .PrimaryKey(t => t.ID);
            
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
                .ForeignKey("dbo.Resource", t => t.BrochureID)
                .ForeignKey("dbo.MeetingResource", t => t.AgendaId)
                .ForeignKey("dbo.MeetingResource", t => t.AttendanceID)
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Event", "MinutesID", "dbo.MeetingResource");
            DropForeignKey("dbo.Event", "AttendanceID", "dbo.MeetingResource");
            DropForeignKey("dbo.Event", "AgendaId", "dbo.MeetingResource");
            DropForeignKey("dbo.MeetingResource", "ResourceID", "dbo.Resource");
            DropForeignKey("dbo.MeetingResource", "MeetingID", "dbo.Event");
            DropForeignKey("dbo.Invitation", "EventID", "dbo.Event");
            DropForeignKey("dbo.Event", "BrochureID", "dbo.Resource");
            DropForeignKey("dbo.Invitation", "ContactID", "dbo.Contact");
            DropForeignKey("dbo.Announcement", "Resource_ID", "dbo.Resource");
            DropForeignKey("dbo.Resource", "ResourceTypeID", "dbo.ResourceType");
            DropForeignKey("dbo.Resource", "FileStoreID", "dbo.FileStore");
            DropIndex("dbo.MeetingResource", new[] { "ResourceID" });
            DropIndex("dbo.MeetingResource", new[] { "MeetingID" });
            DropIndex("dbo.Event", new[] { "AttendanceID" });
            DropIndex("dbo.Event", new[] { "MinutesID" });
            DropIndex("dbo.Event", new[] { "AgendaId" });
            DropIndex("dbo.Event", new[] { "BrochureID" });
            DropIndex("dbo.Invitation", new[] { "EventID" });
            DropIndex("dbo.Invitation", new[] { "ContactID" });
            DropIndex("dbo.ResourceType", "IX_ResourceType_Desc");
            DropIndex("dbo.Resource", new[] { "FileStoreID" });
            DropIndex("dbo.Resource", new[] { "ResourceTypeID" });
            DropIndex("dbo.Announcement", new[] { "Resource_ID" });
            DropTable("dbo.DeathNotice");
            DropTable("dbo.MeetingResource");
            DropTable("dbo.Event");
            DropTable("dbo.Invitation");
            DropTable("dbo.Contact");
            DropTable("dbo.ResourceType");
            DropTable("dbo.FileStore");
            DropTable("dbo.Resource");
            DropTable("dbo.Announcement");
        }
    }
}
