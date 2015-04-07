namespace HospiceNiagara.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DOYOUREALLYWANTOHURTMEEEEEEE : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.DeathNotice", "URL", c => c.String(maxLength: 2048));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DeathNotice", "URL", c => c.String());
        }
    }
}
