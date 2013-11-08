namespace Validus.Console.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class R2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.QuoteTemplates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 20),
                        RdlPath = c.String(maxLength: 256),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                        CreatedBy = c.String(maxLength: 256),
                        ModifiedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TeamOfficeSettings",
                c => new
                    {
                        TeamId = c.Int(nullable: false),
                        OfficeId = c.String(nullable: false, maxLength: 256),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                        CreatedBy = c.String(maxLength: 256),
                        ModifiedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => new { t.TeamId, t.OfficeId })
                .ForeignKey("dbo.Teams", t => t.TeamId, cascadeDelete: true)
                .ForeignKey("dbo.Offices", t => t.OfficeId, cascadeDelete: true)
                .Index(t => t.TeamId)
                .Index(t => t.OfficeId);
            
            CreateTable(
                "dbo.MarketWordingSettings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DisplayOrder = c.Int(nullable: false),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                        CreatedBy = c.String(maxLength: 256),
                        ModifiedBy = c.String(maxLength: 256),
                        MarketWording_Id = c.Int(),
                        TeamOfficeSetting_TeamId = c.Int(),
                        TeamOfficeSetting_OfficeId = c.String(maxLength: 256),
                        Submission_Id = c.Int(),
                        Submission_Id1 = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Wordings", t => t.MarketWording_Id)
                .ForeignKey("dbo.TeamOfficeSettings", t => new { t.TeamOfficeSetting_TeamId, t.TeamOfficeSetting_OfficeId })
                .ForeignKey("dbo.Submissions", t => t.Submission_Id)
                .ForeignKey("dbo.Submissions", t => t.Submission_Id1)
                .Index(t => t.MarketWording_Id)
                .Index(t => new { t.TeamOfficeSetting_TeamId, t.TeamOfficeSetting_OfficeId })
                .Index(t => t.Submission_Id)
                .Index(t => t.Submission_Id1);
            
            CreateTable(
                "dbo.Wordings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WordingRefNumber = c.String(maxLength: 12),
                        WordingType = c.String(maxLength: 15),
                        Title = c.String(nullable: false, maxLength: 256),
                        VersionNo = c.Int(nullable: false),
                        Key = c.Guid(nullable: false),
                        IsObsolete = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                        CreatedBy = c.String(maxLength: 256),
                        ModifiedBy = c.String(maxLength: 256),
                        WordingDesc = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TermsNConditionWordingSettings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DisplayOrder = c.Int(nullable: false),
                        IsStrikeThrough = c.Boolean(nullable: false),
                        TermsNConditionWording_Id = c.Int(),
                        TeamOfficeSetting_TeamId = c.Int(),
                        TeamOfficeSetting_OfficeId = c.String(maxLength: 256),
                        Submission_Id = c.Int(),
                        Submission_Id1 = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Wordings", t => t.TermsNConditionWording_Id)
                .ForeignKey("dbo.TeamOfficeSettings", t => new { t.TeamOfficeSetting_TeamId, t.TeamOfficeSetting_OfficeId })
                .ForeignKey("dbo.Submissions", t => t.Submission_Id)
                .ForeignKey("dbo.Submissions", t => t.Submission_Id1)
                .Index(t => t.TermsNConditionWording_Id)
                .Index(t => new { t.TeamOfficeSetting_TeamId, t.TeamOfficeSetting_OfficeId })
                .Index(t => t.Submission_Id)
                .Index(t => t.Submission_Id1);
            
            CreateTable(
                "dbo.SubjectToClauseWordingSettings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DisplayOrder = c.Int(nullable: false),
                        IsStrikeThrough = c.Boolean(nullable: false),
                        SubjectToClauseWording_Id = c.Int(),
                        TeamOfficeSetting_TeamId = c.Int(),
                        TeamOfficeSetting_OfficeId = c.String(maxLength: 256),
                        Submission_Id = c.Int(),
                        Submission_Id1 = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Wordings", t => t.SubjectToClauseWording_Id)
                .ForeignKey("dbo.TeamOfficeSettings", t => new { t.TeamOfficeSetting_TeamId, t.TeamOfficeSetting_OfficeId })
                .ForeignKey("dbo.Submissions", t => t.Submission_Id)
                .ForeignKey("dbo.Submissions", t => t.Submission_Id1)
                .Index(t => t.SubjectToClauseWording_Id)
                .Index(t => new { t.TeamOfficeSetting_TeamId, t.TeamOfficeSetting_OfficeId })
                .Index(t => t.Submission_Id)
                .Index(t => t.Submission_Id1);
            
            CreateTable(
                "dbo.AppAccelerators",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 20),
                        HomepageUrl = c.String(nullable: false, maxLength: 20),
                        DisplayName = c.String(nullable: false, maxLength: 20),
                        DisplayIcon = c.String(nullable: false, maxLength: 256),
                        ActivityCategory = c.String(nullable: false, maxLength: 20),
                        ActivityActionPreview = c.String(nullable: false, maxLength: 256),
                        ActivityActionExecute = c.String(nullable: false, maxLength: 256),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                        CreatedBy = c.String(maxLength: 256),
                        ModifiedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SubmissionTypes",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Title = c.String(nullable: false, maxLength: 100),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                        CreatedBy = c.String(maxLength: 256),
                        ModifiedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TeamRelatedQuoteTemplates",
                c => new
                    {
                        Team_Id = c.Int(nullable: false),
                        QuoteTemplate_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Team_Id, t.QuoteTemplate_Id })
                .ForeignKey("dbo.Teams", t => t.Team_Id, cascadeDelete: true)
                .ForeignKey("dbo.QuoteTemplates", t => t.QuoteTemplate_Id, cascadeDelete: true)
                .Index(t => t.Team_Id)
                .Index(t => t.QuoteTemplate_Id);
            
            CreateTable(
                "dbo.TeamAppAccelerators",
                c => new
                    {
                        Team_Id = c.Int(nullable: false),
                        AppAccelerator_Id = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => new { t.Team_Id, t.AppAccelerator_Id })
                .ForeignKey("dbo.Teams", t => t.Team_Id, cascadeDelete: true)
                .ForeignKey("dbo.AppAccelerators", t => t.AppAccelerator_Id, cascadeDelete: true)
                .Index(t => t.Team_Id)
                .Index(t => t.AppAccelerator_Id);
            
            CreateTable(
                "dbo.SubmissionsPV",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        ExtraProperty1 = c.String(nullable: false),
                        ExtraProperty2 = c.String(nullable: false),
                        ExtraProperty3 = c.String(nullable: false),
                        ExtraProperty4 = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Submissions", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.SubmissionsEnergy",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        ExtraProperty1 = c.String(),
                        ExtraProperty2 = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Submissions", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.QuotesEnergy",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        QuoteExtraProperty1 = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Quotes", t => t.Id)
                .Index(t => t.Id);
            
            AddColumn("dbo.Submissions", "SubmissionTypeId", c => c.String());
            AddColumn("dbo.TeamMemberships", "PrimaryTeamMembership", c => c.Boolean(nullable: false));
            AddColumn("dbo.Teams", "SubmissionTypeId", c => c.String());
            AlterColumn("dbo.Submissions", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.Submissions", "ModifiedOn", c => c.DateTime());
            AlterColumn("dbo.Underwriters", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.Underwriters", "ModifiedOn", c => c.DateTime());
            AlterColumn("dbo.Offices", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.Offices", "ModifiedOn", c => c.DateTime());
            AlterColumn("dbo.Addresses", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.Addresses", "ModifiedOn", c => c.DateTime());
            AlterColumn("dbo.Options", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.Options", "ModifiedOn", c => c.DateTime());
            AlterColumn("dbo.OptionVersions", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.OptionVersions", "ModifiedOn", c => c.DateTime());
            AlterColumn("dbo.Quotes", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.Quotes", "ModifiedOn", c => c.DateTime());
            AlterColumn("dbo.COBs", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.COBs", "ModifiedOn", c => c.DateTime());
            AlterColumn("dbo.QuoteSheets", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.QuoteSheets", "ModifiedOn", c => c.DateTime());
            AlterColumn("dbo.Users", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.Users", "ModifiedOn", c => c.DateTime());
            AlterColumn("dbo.TeamMemberships", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.TeamMemberships", "ModifiedOn", c => c.DateTime());
            AlterColumn("dbo.Teams", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.Teams", "ModifiedOn", c => c.DateTime());
            AlterColumn("dbo.Links", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.Links", "ModifiedOn", c => c.DateTime());
            AlterColumn("dbo.Tabs", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.Tabs", "ModifiedOn", c => c.DateTime());
            AlterColumn("dbo.Brokers", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.Brokers", "ModifiedOn", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropIndex("dbo.QuotesEnergy", new[] { "Id" });
            DropIndex("dbo.SubmissionsEnergy", new[] { "Id" });
            DropIndex("dbo.SubmissionsPV", new[] { "Id" });
            DropIndex("dbo.TeamAppAccelerators", new[] { "AppAccelerator_Id" });
            DropIndex("dbo.TeamAppAccelerators", new[] { "Team_Id" });
            DropIndex("dbo.TeamRelatedQuoteTemplates", new[] { "QuoteTemplate_Id" });
            DropIndex("dbo.TeamRelatedQuoteTemplates", new[] { "Team_Id" });
            DropIndex("dbo.SubjectToClauseWordingSettings", new[] { "Submission_Id1" });
            DropIndex("dbo.SubjectToClauseWordingSettings", new[] { "Submission_Id" });
            DropIndex("dbo.SubjectToClauseWordingSettings", new[] { "TeamOfficeSetting_TeamId", "TeamOfficeSetting_OfficeId" });
            DropIndex("dbo.SubjectToClauseWordingSettings", new[] { "SubjectToClauseWording_Id" });
            DropIndex("dbo.TermsNConditionWordingSettings", new[] { "Submission_Id1" });
            DropIndex("dbo.TermsNConditionWordingSettings", new[] { "Submission_Id" });
            DropIndex("dbo.TermsNConditionWordingSettings", new[] { "TeamOfficeSetting_TeamId", "TeamOfficeSetting_OfficeId" });
            DropIndex("dbo.TermsNConditionWordingSettings", new[] { "TermsNConditionWording_Id" });
            DropIndex("dbo.MarketWordingSettings", new[] { "Submission_Id1" });
            DropIndex("dbo.MarketWordingSettings", new[] { "Submission_Id" });
            DropIndex("dbo.MarketWordingSettings", new[] { "TeamOfficeSetting_TeamId", "TeamOfficeSetting_OfficeId" });
            DropIndex("dbo.MarketWordingSettings", new[] { "MarketWording_Id" });
            DropIndex("dbo.TeamOfficeSettings", new[] { "OfficeId" });
            DropIndex("dbo.TeamOfficeSettings", new[] { "TeamId" });
            DropForeignKey("dbo.QuotesEnergy", "Id", "dbo.Quotes");
            DropForeignKey("dbo.SubmissionsEnergy", "Id", "dbo.Submissions");
            DropForeignKey("dbo.SubmissionsPV", "Id", "dbo.Submissions");
            DropForeignKey("dbo.TeamAppAccelerators", "AppAccelerator_Id", "dbo.AppAccelerators");
            DropForeignKey("dbo.TeamAppAccelerators", "Team_Id", "dbo.Teams");
            DropForeignKey("dbo.TeamRelatedQuoteTemplates", "QuoteTemplate_Id", "dbo.QuoteTemplates");
            DropForeignKey("dbo.TeamRelatedQuoteTemplates", "Team_Id", "dbo.Teams");
            DropForeignKey("dbo.SubjectToClauseWordingSettings", "Submission_Id1", "dbo.Submissions");
            DropForeignKey("dbo.SubjectToClauseWordingSettings", "Submission_Id", "dbo.Submissions");
            DropForeignKey("dbo.SubjectToClauseWordingSettings", new[] { "TeamOfficeSetting_TeamId", "TeamOfficeSetting_OfficeId" }, "dbo.TeamOfficeSettings");
            DropForeignKey("dbo.SubjectToClauseWordingSettings", "SubjectToClauseWording_Id", "dbo.Wordings");
            DropForeignKey("dbo.TermsNConditionWordingSettings", "Submission_Id1", "dbo.Submissions");
            DropForeignKey("dbo.TermsNConditionWordingSettings", "Submission_Id", "dbo.Submissions");
            DropForeignKey("dbo.TermsNConditionWordingSettings", new[] { "TeamOfficeSetting_TeamId", "TeamOfficeSetting_OfficeId" }, "dbo.TeamOfficeSettings");
            DropForeignKey("dbo.TermsNConditionWordingSettings", "TermsNConditionWording_Id", "dbo.Wordings");
            DropForeignKey("dbo.MarketWordingSettings", "Submission_Id1", "dbo.Submissions");
            DropForeignKey("dbo.MarketWordingSettings", "Submission_Id", "dbo.Submissions");
            DropForeignKey("dbo.MarketWordingSettings", new[] { "TeamOfficeSetting_TeamId", "TeamOfficeSetting_OfficeId" }, "dbo.TeamOfficeSettings");
            DropForeignKey("dbo.MarketWordingSettings", "MarketWording_Id", "dbo.Wordings");
            DropForeignKey("dbo.TeamOfficeSettings", "OfficeId", "dbo.Offices");
            DropForeignKey("dbo.TeamOfficeSettings", "TeamId", "dbo.Teams");
            AlterColumn("dbo.Brokers", "ModifiedOn", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Brokers", "CreatedOn", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Tabs", "ModifiedOn", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Tabs", "CreatedOn", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Links", "ModifiedOn", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Links", "CreatedOn", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Teams", "ModifiedOn", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Teams", "CreatedOn", c => c.DateTime(nullable: false));
            AlterColumn("dbo.TeamMemberships", "ModifiedOn", c => c.DateTime(nullable: false));
            AlterColumn("dbo.TeamMemberships", "CreatedOn", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Users", "ModifiedOn", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Users", "CreatedOn", c => c.DateTime(nullable: false));
            AlterColumn("dbo.QuoteSheets", "ModifiedOn", c => c.DateTime(nullable: false));
            AlterColumn("dbo.QuoteSheets", "CreatedOn", c => c.DateTime(nullable: false));
            AlterColumn("dbo.COBs", "ModifiedOn", c => c.DateTime(nullable: false));
            AlterColumn("dbo.COBs", "CreatedOn", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Quotes", "ModifiedOn", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Quotes", "CreatedOn", c => c.DateTime(nullable: false));
            AlterColumn("dbo.OptionVersions", "ModifiedOn", c => c.DateTime(nullable: false));
            AlterColumn("dbo.OptionVersions", "CreatedOn", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Options", "ModifiedOn", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Options", "CreatedOn", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Addresses", "ModifiedOn", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Addresses", "CreatedOn", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Offices", "ModifiedOn", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Offices", "CreatedOn", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Underwriters", "ModifiedOn", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Underwriters", "CreatedOn", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Submissions", "ModifiedOn", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Submissions", "CreatedOn", c => c.DateTime(nullable: false));
            DropColumn("dbo.Teams", "SubmissionTypeId");
            DropColumn("dbo.TeamMemberships", "PrimaryTeamMembership");
            DropColumn("dbo.Submissions", "SubmissionTypeId");
            DropTable("dbo.QuotesEnergy");
            DropTable("dbo.SubmissionsEnergy");
            DropTable("dbo.SubmissionsPV");
            DropTable("dbo.TeamAppAccelerators");
            DropTable("dbo.TeamRelatedQuoteTemplates");
            DropTable("dbo.SubmissionTypes");
            DropTable("dbo.AppAccelerators");
            DropTable("dbo.SubjectToClauseWordingSettings");
            DropTable("dbo.TermsNConditionWordingSettings");
            DropTable("dbo.Wordings");
            DropTable("dbo.MarketWordingSettings");
            DropTable("dbo.TeamOfficeSettings");
            DropTable("dbo.QuoteTemplates");
        }
    }
}
