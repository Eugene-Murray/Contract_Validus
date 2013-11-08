namespace Validus.Console.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class R3_201310_10 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Submissions", "LeaderNo", c => c.String(maxLength: 4));
            AddColumn("dbo.QuotesFI", "LineSizePctgAmt", c => c.String(maxLength: 10));
            AlterColumn("dbo.QuoteTemplates", "Name", c => c.String(maxLength: 30));
            DropColumn("dbo.QuotesPV", "LineToStand");
        }
        
        public override void Down()
        {
            AddColumn("dbo.QuotesPV", "LineToStand", c => c.Boolean(nullable: false));
            AlterColumn("dbo.QuoteTemplates", "Name", c => c.String(maxLength: 20));
            DropColumn("dbo.QuotesFI", "LineSizePctgAmt");
            DropColumn("dbo.Submissions", "LeaderNo");
        }
    }
}
