namespace Validus.Console.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Submissions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 100),
                        Description = c.String(),
                        InsuredName = c.String(nullable: false, maxLength: 100),
                        InsuredId = c.Int(nullable: false),
                        BrokerCode = c.String(nullable: false, maxLength: 10),
                        BrokerPseudonym = c.String(nullable: false, maxLength: 10),
                        BrokerSequenceId = c.Int(nullable: false),
                        BrokerContact = c.String(nullable: false, maxLength: 50),
                        NonLondonBrokerCode = c.String(maxLength: 10),
                        NonLondonBrokerName = c.String(maxLength: 100),
                        UnderwriterCode = c.String(nullable: false, maxLength: 10),
                        UnderwriterContactCode = c.String(nullable: false, maxLength: 10),
                        QuotingOfficeId = c.String(nullable: false, maxLength: 256),
                        Domicile = c.String(nullable: false, maxLength: 10),
                        Leader = c.String(nullable: false, maxLength: 10),
                        Brokerage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        QuoteSheetNotes = c.String(),
                        UnderwriterNotes = c.String(),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifiedOn = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        ModifiedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Underwriters", t => t.UnderwriterCode)
                .ForeignKey("dbo.Underwriters", t => t.UnderwriterContactCode)
                .ForeignKey("dbo.Offices", t => t.QuotingOfficeId, cascadeDelete: true)
                .Index(t => t.UnderwriterCode)
                .Index(t => t.UnderwriterContactCode)
                .Index(t => t.QuotingOfficeId);
            
            CreateTable(
                "dbo.Underwriters",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 10),
                        Name = c.String(maxLength: 256),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifiedOn = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        ModifiedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "dbo.Offices",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 256),
                        Name = c.String(nullable: false, maxLength: 256),
                        Title = c.String(nullable: false, maxLength: 256),
                        Footer = c.String(maxLength: 256),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifiedOn = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        ModifiedBy = c.String(maxLength: 256),
                        Address_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Addresses", t => t.Address_Id)
                .Index(t => t.Address_Id);
            
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AddressLine1 = c.String(maxLength: 256),
                        AddressLine2 = c.String(maxLength: 256),
                        City = c.String(maxLength: 256),
                        StateProvinceRegion = c.String(maxLength: 256),
                        ZipPostalCode = c.String(maxLength: 20),
                        Country = c.String(maxLength: 256),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifiedOn = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        ModifiedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Options",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SubmissionId = c.Int(nullable: false),
                        Title = c.String(nullable: false, maxLength: 256),
                        Comments = c.String(),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifiedOn = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        ModifiedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Submissions", t => t.SubmissionId, cascadeDelete: true)
                .Index(t => t.SubmissionId);
            
            CreateTable(
                "dbo.OptionVersions",
                c => new
                    {
                        OptionId = c.Int(nullable: false),
                        VersionNumber = c.Int(nullable: false),
                        Title = c.String(nullable: false, maxLength: 256),
                        Comments = c.String(),
                        IsExperiment = c.Boolean(nullable: false),
                        IsLocked = c.Boolean(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifiedOn = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        ModifiedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => new { t.OptionId, t.VersionNumber })
                .ForeignKey("dbo.Options", t => t.OptionId, cascadeDelete: true)
                .Index(t => t.OptionId);
            
            CreateTable(
                "dbo.Quotes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OptionId = c.Int(nullable: false),
                        VersionNumber = c.Int(nullable: false),
                        CorrelationToken = c.Guid(),
                        IsSubscribeMaster = c.Boolean(nullable: false),
                        CopiedFromQuoteId = c.Int(),
                        SubscribeReference = c.String(maxLength: 50),
                        RenPolId = c.String(maxLength: 50),
                        FacilityRef = c.String(maxLength: 50),
                        SubmissionStatus = c.String(nullable: false, maxLength: 50),
                        EntryStatus = c.String(nullable: false, maxLength: 50),
                        PolicyType = c.String(nullable: false, maxLength: 50),
                        OriginatingOfficeId = c.String(nullable: false, maxLength: 256),
                        COBId = c.String(nullable: false, maxLength: 10),
                        MOA = c.String(nullable: false, maxLength: 10),
                        AccountYear = c.Int(nullable: false),
                        InceptionDate = c.DateTime(),
                        ExpiryDate = c.DateTime(),
                        QuoteExpiryDate = c.DateTime(nullable: false),
                        TechnicalPricingMethod = c.String(nullable: false, maxLength: 10),
                        TechnicalPricingBindStatus = c.String(maxLength: 10),
                        TechnicalPricingPremiumPctgAmt = c.String(maxLength: 10),
                        TechnicalPremium = c.Decimal(precision: 18, scale: 2),
                        Currency = c.String(nullable: false, maxLength: 10),
                        LimitCCY = c.String(maxLength: 10),
                        ExcessCCY = c.String(maxLength: 10),
                        BenchmarkPremium = c.Decimal(precision: 18, scale: 2),
                        QuotedPremium = c.Decimal(precision: 18, scale: 2),
                        LimitAmount = c.Decimal(precision: 18, scale: 2),
                        ExcessAmount = c.Decimal(precision: 18, scale: 2),
                        Comment = c.String(),
                        DeclinatureReason = c.String(maxLength: 256),
                        DeclinatureComments = c.String(),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        SubscribeTimestamp = c.Long(),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifiedOn = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        ModifiedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.OptionVersions", t => new { t.OptionId, t.VersionNumber }, cascadeDelete: true)
                .ForeignKey("dbo.Offices", t => t.OriginatingOfficeId)
                .ForeignKey("dbo.COBs", t => t.COBId, cascadeDelete: true)
                .Index(t => new { t.OptionId, t.VersionNumber })
                .Index(t => t.OriginatingOfficeId)
                .Index(t => t.COBId);
            
            CreateTable(
                "dbo.COBs",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 10),
                        Narrative = c.String(nullable: false, maxLength: 100),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifiedOn = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        ModifiedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.QuoteSheets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 256),
                        Guid = c.Guid(nullable: false),
                        ObjectStore = c.String(nullable: false, maxLength: 50),
                        IssuedDate = c.DateTime(),
                        IssuedById = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifiedOn = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        ModifiedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.IssuedById, cascadeDelete: true)
                .Index(t => t.IssuedById);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DomainLogon = c.String(nullable: false, maxLength: 256),
                        UnderwriterCode = c.String(maxLength: 10),
                        PrimaryOfficeId = c.String(maxLength: 256),
                        DefaultOrigOfficeId = c.String(maxLength: 256),
                        DefaultUWId = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifiedOn = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        ModifiedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Underwriters", t => t.UnderwriterCode)
                .ForeignKey("dbo.Offices", t => t.PrimaryOfficeId)
                .ForeignKey("dbo.Offices", t => t.DefaultOrigOfficeId)
                .ForeignKey("dbo.Users", t => t.DefaultUWId)
                .Index(t => t.UnderwriterCode)
                .Index(t => t.PrimaryOfficeId)
                .Index(t => t.DefaultOrigOfficeId)
                .Index(t => t.DefaultUWId);
            
            CreateTable(
                "dbo.TeamMemberships",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TeamId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(),
                        IsCurrent = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifiedOn = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        ModifiedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Teams", t => t.TeamId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.TeamId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Teams",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 256),
                        DefaultMOA = c.String(maxLength: 10),
                        DefaultDomicile = c.String(maxLength: 10),
                        QuoteExpiryDaysDefault = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifiedOn = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        ModifiedBy = c.String(maxLength: 256),
                        PricingActuary_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.PricingActuary_Id)
                .Index(t => t.PricingActuary_Id);
            
            CreateTable(
                "dbo.Links",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Url = c.String(nullable: false, maxLength: 2048),
                        Title = c.String(nullable: false, maxLength: 256),
                        Category = c.String(maxLength: 256),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifiedOn = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        ModifiedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tabs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Url = c.String(nullable: false, maxLength: 2048),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifiedOn = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        ModifiedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Brokers",
                c => new
                    {
                        BrokerSequenceId = c.Int(nullable: false),
                        GroupCode = c.String(maxLength: 10),
                        Code = c.String(maxLength: 10),
                        Name = c.String(maxLength: 256),
                        Psu = c.String(maxLength: 10),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifiedOn = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        ModifiedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.BrokerSequenceId);
            
            CreateTable(
                "dbo.LinkTeams",
                c => new
                    {
                        Link_Id = c.Int(nullable: false),
                        Team_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Link_Id, t.Team_Id })
                .ForeignKey("dbo.Links", t => t.Link_Id, cascadeDelete: true)
                .ForeignKey("dbo.Teams", t => t.Team_Id, cascadeDelete: true)
                .Index(t => t.Link_Id)
                .Index(t => t.Team_Id);
            
            CreateTable(
                "dbo.TeamRelatedCOBs",
                c => new
                    {
                        Team_Id = c.Int(nullable: false),
                        COB_Id = c.String(nullable: false, maxLength: 10),
                    })
                .PrimaryKey(t => new { t.Team_Id, t.COB_Id })
                .ForeignKey("dbo.Teams", t => t.Team_Id, cascadeDelete: true)
                .ForeignKey("dbo.COBs", t => t.COB_Id, cascadeDelete: true)
                .Index(t => t.Team_Id)
                .Index(t => t.COB_Id);
            
            CreateTable(
                "dbo.TeamRelatedOffices",
                c => new
                    {
                        Team_Id = c.Int(nullable: false),
                        Office_Id = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => new { t.Team_Id, t.Office_Id })
                .ForeignKey("dbo.Teams", t => t.Team_Id, cascadeDelete: true)
                .ForeignKey("dbo.Offices", t => t.Office_Id, cascadeDelete: true)
                .Index(t => t.Team_Id)
                .Index(t => t.Office_Id);
            
            CreateTable(
                "dbo.UserFilterCOBs",
                c => new
                    {
                        User_Id = c.Int(nullable: false),
                        COB_Id = c.String(nullable: false, maxLength: 10),
                    })
                .PrimaryKey(t => new { t.User_Id, t.COB_Id })
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .ForeignKey("dbo.COBs", t => t.COB_Id, cascadeDelete: true)
                .Index(t => t.User_Id)
                .Index(t => t.COB_Id);
            
            CreateTable(
                "dbo.UserFilterOffices",
                c => new
                    {
                        User_Id = c.Int(nullable: false),
                        Office_Id = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => new { t.User_Id, t.Office_Id })
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .ForeignKey("dbo.Offices", t => t.Office_Id, cascadeDelete: true)
                .Index(t => t.User_Id)
                .Index(t => t.Office_Id);
            
            CreateTable(
                "dbo.UserFilterMembers",
                c => new
                    {
                        User_Id = c.Int(nullable: false),
                        User_Id1 = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_Id, t.User_Id1 })
                .ForeignKey("dbo.Users", t => t.User_Id)
                .ForeignKey("dbo.Users", t => t.User_Id1)
                .Index(t => t.User_Id)
                .Index(t => t.User_Id1);
            
            CreateTable(
                "dbo.UserAdditionalCOBs",
                c => new
                    {
                        User_Id = c.Int(nullable: false),
                        COB_Id = c.String(nullable: false, maxLength: 10),
                    })
                .PrimaryKey(t => new { t.User_Id, t.COB_Id })
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .ForeignKey("dbo.COBs", t => t.COB_Id, cascadeDelete: true)
                .Index(t => t.User_Id)
                .Index(t => t.COB_Id);
            
            CreateTable(
                "dbo.UserAdditionalOffices",
                c => new
                    {
                        User_Id = c.Int(nullable: false),
                        Office_Id = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => new { t.User_Id, t.Office_Id })
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .ForeignKey("dbo.Offices", t => t.Office_Id, cascadeDelete: true)
                .Index(t => t.User_Id)
                .Index(t => t.Office_Id);
            
            CreateTable(
                "dbo.UserAdditionalUsers",
                c => new
                    {
                        User_Id = c.Int(nullable: false),
                        User_Id1 = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_Id, t.User_Id1 })
                .ForeignKey("dbo.Users", t => t.User_Id)
                .ForeignKey("dbo.Users", t => t.User_Id1)
                .Index(t => t.User_Id)
                .Index(t => t.User_Id1);
            
            CreateTable(
                "dbo.QuoteSheetOptionVersions",
                c => new
                    {
                        QuoteSheet_Id = c.Int(nullable: false),
                        OptionVersion_OptionId = c.Int(nullable: false),
                        OptionVersion_VersionNumber = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.QuoteSheet_Id, t.OptionVersion_OptionId, t.OptionVersion_VersionNumber })
                .ForeignKey("dbo.QuoteSheets", t => t.QuoteSheet_Id, cascadeDelete: true)
                .ForeignKey("dbo.OptionVersions", t => new { t.OptionVersion_OptionId, t.OptionVersion_VersionNumber }, cascadeDelete: true)
                .Index(t => t.QuoteSheet_Id)
                .Index(t => new { t.OptionVersion_OptionId, t.OptionVersion_VersionNumber });
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.QuoteSheetOptionVersions", new[] { "OptionVersion_OptionId", "OptionVersion_VersionNumber" });
            DropIndex("dbo.QuoteSheetOptionVersions", new[] { "QuoteSheet_Id" });
            DropIndex("dbo.UserAdditionalUsers", new[] { "User_Id1" });
            DropIndex("dbo.UserAdditionalUsers", new[] { "User_Id" });
            DropIndex("dbo.UserAdditionalOffices", new[] { "Office_Id" });
            DropIndex("dbo.UserAdditionalOffices", new[] { "User_Id" });
            DropIndex("dbo.UserAdditionalCOBs", new[] { "COB_Id" });
            DropIndex("dbo.UserAdditionalCOBs", new[] { "User_Id" });
            DropIndex("dbo.UserFilterMembers", new[] { "User_Id1" });
            DropIndex("dbo.UserFilterMembers", new[] { "User_Id" });
            DropIndex("dbo.UserFilterOffices", new[] { "Office_Id" });
            DropIndex("dbo.UserFilterOffices", new[] { "User_Id" });
            DropIndex("dbo.UserFilterCOBs", new[] { "COB_Id" });
            DropIndex("dbo.UserFilterCOBs", new[] { "User_Id" });
            DropIndex("dbo.TeamRelatedOffices", new[] { "Office_Id" });
            DropIndex("dbo.TeamRelatedOffices", new[] { "Team_Id" });
            DropIndex("dbo.TeamRelatedCOBs", new[] { "COB_Id" });
            DropIndex("dbo.TeamRelatedCOBs", new[] { "Team_Id" });
            DropIndex("dbo.LinkTeams", new[] { "Team_Id" });
            DropIndex("dbo.LinkTeams", new[] { "Link_Id" });
            DropIndex("dbo.Tabs", new[] { "UserId" });
            DropIndex("dbo.Teams", new[] { "PricingActuary_Id" });
            DropIndex("dbo.TeamMemberships", new[] { "UserId" });
            DropIndex("dbo.TeamMemberships", new[] { "TeamId" });
            DropIndex("dbo.Users", new[] { "DefaultUWId" });
            DropIndex("dbo.Users", new[] { "DefaultOrigOfficeId" });
            DropIndex("dbo.Users", new[] { "PrimaryOfficeId" });
            DropIndex("dbo.Users", new[] { "UnderwriterCode" });
            DropIndex("dbo.QuoteSheets", new[] { "IssuedById" });
            DropIndex("dbo.Quotes", new[] { "COBId" });
            DropIndex("dbo.Quotes", new[] { "OriginatingOfficeId" });
            DropIndex("dbo.Quotes", new[] { "OptionId", "VersionNumber" });
            DropIndex("dbo.OptionVersions", new[] { "OptionId" });
            DropIndex("dbo.Options", new[] { "SubmissionId" });
            DropIndex("dbo.Offices", new[] { "Address_Id" });
            DropIndex("dbo.Submissions", new[] { "QuotingOfficeId" });
            DropIndex("dbo.Submissions", new[] { "UnderwriterContactCode" });
            DropIndex("dbo.Submissions", new[] { "UnderwriterCode" });
            DropForeignKey("dbo.QuoteSheetOptionVersions", new[] { "OptionVersion_OptionId", "OptionVersion_VersionNumber" }, "dbo.OptionVersions");
            DropForeignKey("dbo.QuoteSheetOptionVersions", "QuoteSheet_Id", "dbo.QuoteSheets");
            DropForeignKey("dbo.UserAdditionalUsers", "User_Id1", "dbo.Users");
            DropForeignKey("dbo.UserAdditionalUsers", "User_Id", "dbo.Users");
            DropForeignKey("dbo.UserAdditionalOffices", "Office_Id", "dbo.Offices");
            DropForeignKey("dbo.UserAdditionalOffices", "User_Id", "dbo.Users");
            DropForeignKey("dbo.UserAdditionalCOBs", "COB_Id", "dbo.COBs");
            DropForeignKey("dbo.UserAdditionalCOBs", "User_Id", "dbo.Users");
            DropForeignKey("dbo.UserFilterMembers", "User_Id1", "dbo.Users");
            DropForeignKey("dbo.UserFilterMembers", "User_Id", "dbo.Users");
            DropForeignKey("dbo.UserFilterOffices", "Office_Id", "dbo.Offices");
            DropForeignKey("dbo.UserFilterOffices", "User_Id", "dbo.Users");
            DropForeignKey("dbo.UserFilterCOBs", "COB_Id", "dbo.COBs");
            DropForeignKey("dbo.UserFilterCOBs", "User_Id", "dbo.Users");
            DropForeignKey("dbo.TeamRelatedOffices", "Office_Id", "dbo.Offices");
            DropForeignKey("dbo.TeamRelatedOffices", "Team_Id", "dbo.Teams");
            DropForeignKey("dbo.TeamRelatedCOBs", "COB_Id", "dbo.COBs");
            DropForeignKey("dbo.TeamRelatedCOBs", "Team_Id", "dbo.Teams");
            DropForeignKey("dbo.LinkTeams", "Team_Id", "dbo.Teams");
            DropForeignKey("dbo.LinkTeams", "Link_Id", "dbo.Links");
            DropForeignKey("dbo.Tabs", "UserId", "dbo.Users");
            DropForeignKey("dbo.Teams", "PricingActuary_Id", "dbo.Users");
            DropForeignKey("dbo.TeamMemberships", "UserId", "dbo.Users");
            DropForeignKey("dbo.TeamMemberships", "TeamId", "dbo.Teams");
            DropForeignKey("dbo.Users", "DefaultUWId", "dbo.Users");
            DropForeignKey("dbo.Users", "DefaultOrigOfficeId", "dbo.Offices");
            DropForeignKey("dbo.Users", "PrimaryOfficeId", "dbo.Offices");
            DropForeignKey("dbo.Users", "UnderwriterCode", "dbo.Underwriters");
            DropForeignKey("dbo.QuoteSheets", "IssuedById", "dbo.Users");
            DropForeignKey("dbo.Quotes", "COBId", "dbo.COBs");
            DropForeignKey("dbo.Quotes", "OriginatingOfficeId", "dbo.Offices");
            DropForeignKey("dbo.Quotes", new[] { "OptionId", "VersionNumber" }, "dbo.OptionVersions");
            DropForeignKey("dbo.OptionVersions", "OptionId", "dbo.Options");
            DropForeignKey("dbo.Options", "SubmissionId", "dbo.Submissions");
            DropForeignKey("dbo.Offices", "Address_Id", "dbo.Addresses");
            DropForeignKey("dbo.Submissions", "QuotingOfficeId", "dbo.Offices");
            DropForeignKey("dbo.Submissions", "UnderwriterContactCode", "dbo.Underwriters");
            DropForeignKey("dbo.Submissions", "UnderwriterCode", "dbo.Underwriters");
            DropTable("dbo.QuoteSheetOptionVersions");
            DropTable("dbo.UserAdditionalUsers");
            DropTable("dbo.UserAdditionalOffices");
            DropTable("dbo.UserAdditionalCOBs");
            DropTable("dbo.UserFilterMembers");
            DropTable("dbo.UserFilterOffices");
            DropTable("dbo.UserFilterCOBs");
            DropTable("dbo.TeamRelatedOffices");
            DropTable("dbo.TeamRelatedCOBs");
            DropTable("dbo.LinkTeams");
            DropTable("dbo.Brokers");
            DropTable("dbo.Tabs");
            DropTable("dbo.Links");
            DropTable("dbo.Teams");
            DropTable("dbo.TeamMemberships");
            DropTable("dbo.Users");
            DropTable("dbo.QuoteSheets");
            DropTable("dbo.COBs");
            DropTable("dbo.Quotes");
            DropTable("dbo.OptionVersions");
            DropTable("dbo.Options");
            DropTable("dbo.Addresses");
            DropTable("dbo.Offices");
            DropTable("dbo.Underwriters");
            DropTable("dbo.Submissions");
        }
    }
}
