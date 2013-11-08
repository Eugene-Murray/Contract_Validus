namespace Validus.Console.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class R2_201309_5 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.QuotesPV", "PDExcessCurrency");
            DropColumn("dbo.QuotesPV", "BIExcessCurrency");
        }
        
        public override void Down()
        {
            AddColumn("dbo.QuotesPV", "BIExcessCurrency", c => c.String(maxLength: 10));
            AddColumn("dbo.QuotesPV", "PDExcessCurrency", c => c.String(maxLength: 10));
        }
    }
}
