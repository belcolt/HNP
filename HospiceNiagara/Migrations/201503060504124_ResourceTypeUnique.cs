namespace HospiceNiagara.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ResourceTypeUnique : DbMigration
    {
        public override void Up()
        {
        }
        
        public override void Down()
        {
            DropIndex("dbo.ResourceType", "IX_ResourceType_Desc");
        }
    }
}
