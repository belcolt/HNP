namespace HospiceNiagara.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class job : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Contact", "IsBoardDirector");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Contact", "IsBoardDirector", c => c.Boolean(nullable: false));
        }
    }
}
