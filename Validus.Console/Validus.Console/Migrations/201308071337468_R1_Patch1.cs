namespace Validus.Console.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class R1_Patch1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Teams", "DefaultPolicyType", c => c.String(nullable: false, maxLength: 100, defaultValue:"MARINE"));
            AlterColumn("dbo.Submissions", "BrokerContact", c => c.String(maxLength: 50));
            AlterColumn("dbo.Submissions", "Leader", c => c.String(maxLength: 10));
            AlterColumn("dbo.Submissions", "Brokerage", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.Quotes", "TechnicalPricingMethod", c => c.String(maxLength: 10));
            AlterColumn("dbo.Quotes", "Currency", c => c.String(maxLength: 10));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Quotes", "Currency", c => c.String(nullable: false, maxLength: 10));
            AlterColumn("dbo.Quotes", "TechnicalPricingMethod", c => c.String(nullable: false, maxLength: 10));
            AlterColumn("dbo.Submissions", "Brokerage", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Submissions", "Leader", c => c.String(nullable: false, maxLength: 10));
            AlterColumn("dbo.Submissions", "BrokerContact", c => c.String(nullable: false, maxLength: 50));
            DropColumn("dbo.Teams", "DefaultPolicyType");
        }
    }
}
