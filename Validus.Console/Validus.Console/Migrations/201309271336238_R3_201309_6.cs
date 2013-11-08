namespace Validus.Console.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class R3_201309_6 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.QuotesME",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        AmountOrOPL = c.String(),
                        AmountOrONP = c.String(),
                        LineSize = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Quotes", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.QuotesHM",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        AmountOrOPL = c.String(),
                        AmountOrONP = c.String(),
                        VesselTopLimitCurrency = c.String(maxLength: 10),
                        VesselTopLimitAmount = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Quotes", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.QuotesCA",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        AmountOrOPL = c.String(),
                        AmountOrONP = c.String(),
                        LineSize = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Quotes", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.OptionVersionsHM",
                c => new
                    {
                        OptionId = c.Int(nullable: false),
                        VersionNumber = c.Int(nullable: false),
                        TSICurrency = c.String(maxLength: 10),
                        TSIAmount = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => new { t.OptionId, t.VersionNumber })
                .ForeignKey("dbo.OptionVersions", t => new { t.OptionId, t.VersionNumber })
                .Index(t => new { t.OptionId, t.VersionNumber });
            
            CreateTable(
                "dbo.OptionVersionsCA",
                c => new
                    {
                        OptionId = c.Int(nullable: false),
                        VersionNumber = c.Int(nullable: false),
                        TSICurrency = c.String(maxLength: 10),
                        TSIAmount = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => new { t.OptionId, t.VersionNumber })
                .ForeignKey("dbo.OptionVersions", t => new { t.OptionId, t.VersionNumber })
                .Index(t => new { t.OptionId, t.VersionNumber });
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.OptionVersionsCA", new[] { "OptionId", "VersionNumber" });
            DropIndex("dbo.OptionVersionsHM", new[] { "OptionId", "VersionNumber" });
            DropIndex("dbo.QuotesCA", new[] { "Id" });
            DropIndex("dbo.QuotesHM", new[] { "Id" });
            DropIndex("dbo.QuotesME", new[] { "Id" });
            DropForeignKey("dbo.OptionVersionsCA", new[] { "OptionId", "VersionNumber" }, "dbo.OptionVersions");
            DropForeignKey("dbo.OptionVersionsHM", new[] { "OptionId", "VersionNumber" }, "dbo.OptionVersions");
            DropForeignKey("dbo.QuotesCA", "Id", "dbo.Quotes");
            DropForeignKey("dbo.QuotesHM", "Id", "dbo.Quotes");
            DropForeignKey("dbo.QuotesME", "Id", "dbo.Quotes");
            DropTable("dbo.OptionVersionsCA");
            DropTable("dbo.OptionVersionsHM");
            DropTable("dbo.QuotesCA");
            DropTable("dbo.QuotesHM");
            DropTable("dbo.QuotesME");
        }
    }
}
