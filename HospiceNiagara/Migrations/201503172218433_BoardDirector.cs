namespace HospiceNiagara.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BoardDirector : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contact", "IsBoardDirector", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Contact", "IsBoardDirector");
        }
    }
}
