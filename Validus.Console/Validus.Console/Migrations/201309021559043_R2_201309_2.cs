namespace Validus.Console.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class R2_201309_2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SubmissionsEnergy", "Id", "dbo.Submissions");
            DropForeignKey("dbo.QuotesEnergy", "Id", "dbo.Quotes");
            DropIndex("dbo.SubmissionsEnergy", new[] { "Id" });
            DropIndex("dbo.QuotesEnergy", new[] { "Id" });
            CreateTable(
                "dbo.AuditTrails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Source = c.String(maxLength: 20),
                        Reference = c.String(maxLength: 20),
                        Title = c.String(maxLength: 256),
                        Description = c.String(),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                        CreatedBy = c.String(maxLength: 256),
                        ModifiedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SubmissionsEN",
                c => new
                    {
                        Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Submissions", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.QuotesEN",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        AmountOrOPL = c.String(),
                        AmountOrONP = c.String(),
                        QuoteComments = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Quotes", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.QuotesPV",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        PDPctgAmt = c.String(maxLength: 10),
                        PDExcessCurrency = c.String(maxLength: 10),
                        PDExcessAmount = c.Decimal(precision: 18, scale: 2),
                        BIPctgAmtDays = c.String(maxLength: 10),
                        BIExcessCurrency = c.String(maxLength: 10),
                        BIExcessAmount = c.Decimal(precision: 18, scale: 2),
                        LineSize = c.Decimal(precision: 18, scale: 2),
                        LineToStand = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Quotes", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.OptionVersionsPV",
                c => new
                    {
                        OptionId = c.Int(nullable: false),
                        VersionNumber = c.Int(nullable: false),
                        TSICurrency = c.String(maxLength: 10),
                        TSIPD = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TSIBI = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TSITotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => new { t.OptionId, t.VersionNumber })
                .ForeignKey("dbo.OptionVersions", t => new { t.OptionId, t.VersionNumber })
                .Index(t => new { t.OptionId, t.VersionNumber });
            
            AddColumn("dbo.Submissions", "Industry", c => c.String(maxLength: 20));
            AddColumn("dbo.Submissions", "Situation", c => c.String(maxLength: 20));
            AddColumn("dbo.Submissions", "Order", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Submissions", "EstSignPctg", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Addresses", "Phone", c => c.String(maxLength: 256));
            AddColumn("dbo.Addresses", "Fax", c => c.String(maxLength: 256));
            AddColumn("dbo.Addresses", "Url", c => c.String(maxLength: 256));
            AddColumn("dbo.TermsNConditionWordingSettings", "CreatedOn", c => c.DateTime());
            AddColumn("dbo.TermsNConditionWordingSettings", "ModifiedOn", c => c.DateTime());
            AddColumn("dbo.TermsNConditionWordingSettings", "CreatedBy", c => c.String(maxLength: 256));
            AddColumn("dbo.TermsNConditionWordingSettings", "ModifiedBy", c => c.String(maxLength: 256));
            AddColumn("dbo.SubjectToClauseWordingSettings", "CreatedOn", c => c.DateTime());
            AddColumn("dbo.SubjectToClauseWordingSettings", "ModifiedOn", c => c.DateTime());
            AddColumn("dbo.SubjectToClauseWordingSettings", "CreatedBy", c => c.String(maxLength: 256));
            AddColumn("dbo.SubjectToClauseWordingSettings", "ModifiedBy", c => c.String(maxLength: 256));
            DropColumn("dbo.SubmissionsPV", "ExtraProperty1");
            DropColumn("dbo.SubmissionsPV", "ExtraProperty2");
            DropColumn("dbo.SubmissionsPV", "ExtraProperty3");
            DropColumn("dbo.SubmissionsPV", "ExtraProperty4");
            DropTable("dbo.SubmissionsEnergy");
            DropTable("dbo.QuotesEnergy");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.QuotesEnergy",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        QuoteExtraProperty1 = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SubmissionsEnergy",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        ExtraProperty1 = c.String(),
                        ExtraProperty2 = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.SubmissionsPV", "ExtraProperty4", c => c.String(nullable: false));
            AddColumn("dbo.SubmissionsPV", "ExtraProperty3", c => c.String(nullable: false));
            AddColumn("dbo.SubmissionsPV", "ExtraProperty2", c => c.String(nullable: false));
            AddColumn("dbo.SubmissionsPV", "ExtraProperty1", c => c.String(nullable: false));
            DropIndex("dbo.OptionVersionsPV", new[] { "OptionId", "VersionNumber" });
            DropIndex("dbo.QuotesPV", new[] { "Id" });
            DropIndex("dbo.QuotesEN", new[] { "Id" });
            DropIndex("dbo.SubmissionsEN", new[] { "Id" });
            DropForeignKey("dbo.OptionVersionsPV", new[] { "OptionId", "VersionNumber" }, "dbo.OptionVersions");
            DropForeignKey("dbo.QuotesPV", "Id", "dbo.Quotes");
            DropForeignKey("dbo.QuotesEN", "Id", "dbo.Quotes");
            DropForeignKey("dbo.SubmissionsEN", "Id", "dbo.Submissions");
            DropColumn("dbo.SubjectToClauseWordingSettings", "ModifiedBy");
            DropColumn("dbo.SubjectToClauseWordingSettings", "CreatedBy");
            DropColumn("dbo.SubjectToClauseWordingSettings", "ModifiedOn");
            DropColumn("dbo.SubjectToClauseWordingSettings", "CreatedOn");
            DropColumn("dbo.TermsNConditionWordingSettings", "ModifiedBy");
            DropColumn("dbo.TermsNConditionWordingSettings", "CreatedBy");
            DropColumn("dbo.TermsNConditionWordingSettings", "ModifiedOn");
            DropColumn("dbo.TermsNConditionWordingSettings", "CreatedOn");
            DropColumn("dbo.Addresses", "Url");
            DropColumn("dbo.Addresses", "Fax");
            DropColumn("dbo.Addresses", "Phone");
            DropColumn("dbo.Submissions", "EstSignPctg");
            DropColumn("dbo.Submissions", "Order");
            DropColumn("dbo.Submissions", "Situation");
            DropColumn("dbo.Submissions", "Industry");
            DropTable("dbo.OptionVersionsPV");
            DropTable("dbo.QuotesPV");
            DropTable("dbo.QuotesEN");
            DropTable("dbo.SubmissionsEN");
            DropTable("dbo.AuditTrails");
            CreateIndex("dbo.QuotesEnergy", "Id");
            CreateIndex("dbo.SubmissionsEnergy", "Id");
            AddForeignKey("dbo.QuotesEnergy", "Id", "dbo.Quotes", "Id");
            AddForeignKey("dbo.SubmissionsEnergy", "Id", "dbo.Submissions", "Id");
        }
    }
}
