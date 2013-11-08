namespace Validus.Console.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class R3_201310_10 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AdditionalInsureds",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InsuredId = c.Int(nullable: false),
                        InsuredName = c.String(),
                        InsuredType = c.String(nullable: false, maxLength: 50),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                        CreatedBy = c.String(maxLength: 256),
                        ModifiedBy = c.String(maxLength: 256),
                        Submission_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Submissions", t => t.Submission_Id)
                .Index(t => t.Submission_Id);
            
            AddColumn("dbo.Submissions", "LeaderNo", c => c.String(maxLength: 4));
            AddColumn("dbo.Quotes", "RenewalRate", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Quotes", "RenewalConditions", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Quotes", "RenewalDeductibles", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Quotes", "RenewalExposure", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Quotes", "RenewalBase", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Quotes", "RenewalFull", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.QuotesFI", "LineSizePctgAmt", c => c.String(maxLength: 10));
            AlterColumn("dbo.QuoteTemplates", "Name", c => c.String(maxLength: 30));
            DropColumn("dbo.QuotesPV", "LineToStand");
        }
        
        public override void Down()
        {
            AddColumn("dbo.QuotesPV", "LineToStand", c => c.Boolean(nullable: false));
            DropIndex("dbo.AdditionalInsureds", new[] { "Submission_Id" });
            DropForeignKey("dbo.AdditionalInsureds", "Submission_Id", "dbo.Submissions");
            AlterColumn("dbo.QuoteTemplates", "Name", c => c.String(maxLength: 20));
            DropColumn("dbo.QuotesFI", "LineSizePctgAmt");
            DropColumn("dbo.Quotes", "RenewalFull");
            DropColumn("dbo.Quotes", "RenewalBase");
            DropColumn("dbo.Quotes", "RenewalExposure");
            DropColumn("dbo.Quotes", "RenewalDeductibles");
            DropColumn("dbo.Quotes", "RenewalConditions");
            DropColumn("dbo.Quotes", "RenewalRate");
            DropColumn("dbo.Submissions", "LeaderNo");
            DropTable("dbo.AdditionalInsureds");
        }
    }
}
