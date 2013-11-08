namespace Validus.Console.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class R2_201312_4 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Wordings", "WordingRefNumber", c => c.String(maxLength: 256));
            AlterColumn("dbo.AppAccelerators", "HomepageUrl", c => c.String(nullable: false, maxLength: 256));
            AlterColumn("dbo.AppAccelerators", "DisplayName", c => c.String(nullable: false, maxLength: 256));
            AlterColumn("dbo.AppAccelerators", "ActivityCategory", c => c.String(nullable: false, maxLength: 256));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AppAccelerators", "ActivityCategory", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.AppAccelerators", "DisplayName", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.AppAccelerators", "HomepageUrl", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.Wordings", "WordingRefNumber", c => c.String(maxLength: 12));
        }
    }
}
