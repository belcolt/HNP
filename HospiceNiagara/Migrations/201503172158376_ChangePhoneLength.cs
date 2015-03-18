namespace HospiceNiagara.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangePhoneLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Contact", "Phone", c => c.String(nullable: false, maxLength: 10));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Contact", "Phone", c => c.String(nullable: false, maxLength: 9));
        }
    }
}
