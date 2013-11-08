namespace Validus.Console.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class R2_201309_3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RiskCodes",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 12),
                        Name = c.String(nullable: false, maxLength: 50),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                        CreatedBy = c.String(maxLength: 256),
                        ModifiedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "dbo.TeamRelatedRisks",
                c => new
                    {
                        Team_Id = c.Int(nullable: false),
                        RiskCode_Code = c.String(nullable: false, maxLength: 12),
                    })
                .PrimaryKey(t => new { t.Team_Id, t.RiskCode_Code })
                .ForeignKey("dbo.Teams", t => t.Team_Id, cascadeDelete: true)
                .ForeignKey("dbo.RiskCodes", t => t.RiskCode_Code, cascadeDelete: true)
                .Index(t => t.Team_Id)
                .Index(t => t.RiskCode_Code);
            
            CreateTable(
                "dbo.QuotesFI",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        RiskCodeId = c.String(maxLength: 12),
                        AmountOrOPL = c.String(),
                        AmountOrONP = c.String(),
                        LineSize = c.Decimal(precision: 18, scale: 2),
                        LineToStand = c.Boolean(nullable: false),
                        IsReinstatement = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Quotes", t => t.Id)
                .ForeignKey("dbo.RiskCodes", t => t.RiskCodeId)
                .Index(t => t.Id)
                .Index(t => t.RiskCodeId);
            
            CreateTable(
                "dbo.OptionsFI",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        RiskCodes = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Options", t => t.Id)
                .Index(t => t.Id);
            
            AlterColumn("dbo.OptionVersionsPV", "TSIPD", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.OptionVersionsPV", "TSIBI", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.OptionVersionsPV", "TSITotal", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropIndex("dbo.OptionsFI", new[] { "Id" });
            DropIndex("dbo.QuotesFI", new[] { "RiskCodeId" });
            DropIndex("dbo.QuotesFI", new[] { "Id" });
            DropIndex("dbo.TeamRelatedRisks", new[] { "RiskCode_Code" });
            DropIndex("dbo.TeamRelatedRisks", new[] { "Team_Id" });
            DropForeignKey("dbo.OptionsFI", "Id", "dbo.Options");
            DropForeignKey("dbo.QuotesFI", "RiskCodeId", "dbo.RiskCodes");
            DropForeignKey("dbo.QuotesFI", "Id", "dbo.Quotes");
            DropForeignKey("dbo.TeamRelatedRisks", "RiskCode_Code", "dbo.RiskCodes");
            DropForeignKey("dbo.TeamRelatedRisks", "Team_Id", "dbo.Teams");
            AlterColumn("dbo.OptionVersionsPV", "TSITotal", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.OptionVersionsPV", "TSIBI", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.OptionVersionsPV", "TSIPD", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropTable("dbo.OptionsFI");
            DropTable("dbo.QuotesFI");
            DropTable("dbo.TeamRelatedRisks");
            DropTable("dbo.RiskCodes");
        }
    }
}
