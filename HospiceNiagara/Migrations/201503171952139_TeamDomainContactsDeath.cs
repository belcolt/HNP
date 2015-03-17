namespace HospiceNiagara.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TeamDomainContactsDeath : DbMigration
    {
        public override void Up()
        {
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
            
            AddColumn("dbo.Contact", "TeamDomainID", c => c.Int(nullable: false));
            
            CreateIndex("dbo.Contact", "TeamDomainID");
            AddForeignKey("dbo.Contact", "TeamDomainID", "dbo.TeamDomain", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Contact", "TeamDomainID", "dbo.TeamDomain");
            DropIndex("dbo.TeamDomain", "IX_TeamDomain_Unique");
            DropIndex("dbo.Contact", new[] { "TeamDomainID" });
            DropIndex("dbo.ResourceType", "IX_ResourceType_Desc");
            DropColumn("dbo.Contact", "TeamDomainID");
            DropTable("dbo.DeathNotice");
            DropTable("dbo.TeamDomain");
        }
    }
}
