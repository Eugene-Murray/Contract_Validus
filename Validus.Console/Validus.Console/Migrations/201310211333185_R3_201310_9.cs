namespace Validus.Console.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class R3_201310_9 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Options", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.OptionVersions", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Quotes", "IsDeleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Quotes", "IsDeleted");
            DropColumn("dbo.OptionVersions", "IsDeleted");
            DropColumn("dbo.Options", "IsDeleted");
        }
    }
}
