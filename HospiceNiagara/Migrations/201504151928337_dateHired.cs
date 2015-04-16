namespace HospiceNiagara.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dateHired : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contact", "DateHired", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Contact", "DateHired");
        }
    }
}
