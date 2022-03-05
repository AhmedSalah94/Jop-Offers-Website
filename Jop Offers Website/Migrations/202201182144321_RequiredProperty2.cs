namespace Jop_Offers_Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequiredProperty2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Jobs", "JobContent", c => c.String());
            AlterColumn("dbo.Jobs", "JobImage", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Jobs", "JobImage", c => c.String());
            AlterColumn("dbo.Jobs", "JobContent", c => c.String(nullable: false));
        }
    }
}
