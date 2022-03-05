namespace Jop_Offers_Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteRequiredFromProperty1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Jobs", "JobImage", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Jobs", "JobImage", c => c.String(nullable: false));
        }
    }
}
