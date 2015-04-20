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
                        Title = c.String(nullable: false),
                        Content = c.String(),
                        ExpiryDate = c.DateTime(nullable: false),
                        PostDate = c.DateTime(nullable: false),
                        IsNew = c.Boolean(nullable: false),
                        ResourceID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Resource", t => t.ResourceID, cascadeDelete: true)
                .Index(t => t.ResourceID);
            
            CreateTable(
                "dbo.Resource",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FileDesc = c.String(nullable: false),
                        DateAdded = c.DateTime(nullable: false),
                        DownloadCount = c.Int(nullable: false),
                        ResourceCategoryID = c.Int(nullable: false),
                        ResourceSubCategoryID = c.Int(),
                        FileStoreID = c.Int(nullable: false),
                        Panel = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.FileStore", t => t.FileStoreID, cascadeDelete: true)
                .ForeignKey("dbo.ResourceCategory", t => t.ResourceCategoryID, cascadeDelete: false)
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
                        Panel = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.TeamDomain", t => t.TeamDomainID, cascadeDelete: false)
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
            
            CreateTable(
                "dbo.Contact",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        Phone = c.String(nullable: false, maxLength: 10),
                        Email = c.String(),
                        DateHired = c.DateTime(nullable: false),
                        TeamDomainID = c.Int(nullable: false),
                        JobDescriptionID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.JobDescription", t => t.JobDescriptionID, cascadeDelete: true)
                .ForeignKey("dbo.TeamDomain", t => t.TeamDomainID, cascadeDelete: true)
                .Index(t => t.TeamDomainID)
                .Index(t => t.JobDescriptionID);
            
            CreateTable(
                "dbo.Invitation",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        RSVP = c.Boolean(nullable: false),
                        HasResponded = c.Boolean(nullable: false),
                        ContactID = c.Int(nullable: false),
                        EventMeetingID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Contact", t => t.ContactID, cascadeDelete: true)
                .ForeignKey("dbo.HospiceDate", t => t.EventMeetingID, cascadeDelete: true)
                .Index(t => t.ContactID)
                .Index(t => t.EventMeetingID);
            
            CreateTable(
                "dbo.HospiceDate",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        StartDateTime = c.DateTime(nullable: false),
                        EndDateTime = c.DateTime(nullable: false),
                        Location = c.String(nullable: false),
                        Notes = c.String(),
                        StaffLead = c.String(),
                        VolunteersNeeded = c.Boolean(),
                        BrochureId = c.Int(),
                        AgendaID = c.Int(),
                        MinutesID = c.Int(),
                        AttendanceID = c.Int(),
                        Requirements = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.MeetingResource", t => t.AgendaID)
                .ForeignKey("dbo.MeetingResource", t => t.AttendanceID)
                .ForeignKey("dbo.MeetingResource", t => t.MinutesID)
                .ForeignKey("dbo.MeetingResource", t => t.BrochureId)
                .Index(t => t.BrochureId)
                .Index(t => t.AgendaID)
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
                .ForeignKey("dbo.HospiceDate", t => t.MeetingID, cascadeDelete: true)
                .ForeignKey("dbo.Resource", t => t.ResourceID, cascadeDelete: true)
                .Index(t => t.MeetingID)
                .Index(t => t.ResourceID);
            
            CreateTable(
                "dbo.JobDescription",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        JobName = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
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
                        URL = c.String(maxLength: 2048),
                        ExpiryDate = c.DateTime(nullable: false),
                        PostDate = c.DateTime(nullable: false),
                        IsNew = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        TeamDomainID = c.Int(),
                        NonClient = c.Boolean(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TeamDomain", t => t.TeamDomainID, cascadeDelete: true)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex")
                .Index(t => t.TeamDomainID);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Schedule",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Category = c.String(),
                        Month = c.String(),
                        Year = c.Int(nullable: false),
                        IsActiveSchedule = c.Boolean(nullable: false),
                        DataAdded = c.DateTime(nullable: false),
                        ResourceID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Resource", t => t.ResourceID, cascadeDelete: true)
                .Index(t => t.ResourceID);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        LastLoggedIn = c.DateTime(),
                        LoggedIn = c.DateTime(),
                        ContactID = c.Int(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Contact", t => t.ContactID, cascadeDelete: false)
                .Index(t => t.ContactID)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "ContactID", "dbo.Contact");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Schedule", "ResourceID", "dbo.Resource");
            DropForeignKey("dbo.AspNetRoles", "TeamDomainID", "dbo.TeamDomain");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Contact", "TeamDomainID", "dbo.TeamDomain");
            DropForeignKey("dbo.Contact", "JobDescriptionID", "dbo.JobDescription");
            DropForeignKey("dbo.HospiceDate", "BrochureId", "dbo.MeetingResource");
            DropForeignKey("dbo.MeetingResource", "ResourceID", "dbo.Resource");
            DropForeignKey("dbo.MeetingResource", "MeetingID", "dbo.HospiceDate");
            DropForeignKey("dbo.HospiceDate", "MinutesID", "dbo.MeetingResource");
            DropForeignKey("dbo.HospiceDate", "AttendanceID", "dbo.MeetingResource");
            DropForeignKey("dbo.HospiceDate", "AgendaID", "dbo.MeetingResource");
            DropForeignKey("dbo.Invitation", "EventMeetingID", "dbo.HospiceDate");
            DropForeignKey("dbo.Invitation", "ContactID", "dbo.Contact");
            DropForeignKey("dbo.Announcement", "ResourceID", "dbo.Resource");
            DropForeignKey("dbo.Resource", "ResourceSubCategoryID", "dbo.ResourceSubCategory");
            DropForeignKey("dbo.ResourceSubCategory", "ResourceCategoryID", "dbo.ResourceCategory");
            DropForeignKey("dbo.Resource", "ResourceCategoryID", "dbo.ResourceCategory");
            DropForeignKey("dbo.ResourceCategory", "TeamDomainID", "dbo.TeamDomain");
            DropForeignKey("dbo.Resource", "FileStoreID", "dbo.FileStore");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUsers", new[] { "ContactID" });
            DropIndex("dbo.Schedule", new[] { "ResourceID" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", new[] { "TeamDomainID" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.MeetingResource", new[] { "ResourceID" });
            DropIndex("dbo.MeetingResource", new[] { "MeetingID" });
            DropIndex("dbo.HospiceDate", new[] { "AttendanceID" });
            DropIndex("dbo.HospiceDate", new[] { "MinutesID" });
            DropIndex("dbo.HospiceDate", new[] { "AgendaID" });
            DropIndex("dbo.HospiceDate", new[] { "BrochureId" });
            DropIndex("dbo.Invitation", new[] { "EventMeetingID" });
            DropIndex("dbo.Invitation", new[] { "ContactID" });
            DropIndex("dbo.Contact", new[] { "JobDescriptionID" });
            DropIndex("dbo.Contact", new[] { "TeamDomainID" });
            DropIndex("dbo.ResourceSubCategory", new[] { "ResourceCategoryID" });
            DropIndex("dbo.ResourceSubCategory", "IX_ResouceSubCat_Unique");
            DropIndex("dbo.TeamDomain", "IX_TeamDomain_Unique");
            DropIndex("dbo.ResourceCategory", "IX_ResouceDomCat_Unique");
            DropIndex("dbo.Resource", new[] { "FileStoreID" });
            DropIndex("dbo.Resource", new[] { "ResourceSubCategoryID" });
            DropIndex("dbo.Resource", new[] { "ResourceCategoryID" });
            DropIndex("dbo.Announcement", new[] { "ResourceID" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Schedule");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.DeathNotice");
            DropTable("dbo.JobDescription");
            DropTable("dbo.MeetingResource");
            DropTable("dbo.HospiceDate");
            DropTable("dbo.Invitation");
            DropTable("dbo.Contact");
            DropTable("dbo.BoardMember");
            DropTable("dbo.ResourceSubCategory");
            DropTable("dbo.TeamDomain");
            DropTable("dbo.ResourceCategory");
            DropTable("dbo.FileStore");
            DropTable("dbo.Resource");
            DropTable("dbo.Announcement");
        }
    }
}
