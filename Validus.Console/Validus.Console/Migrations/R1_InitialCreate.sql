﻿CREATE TABLE [dbo].[Submissions] (
    [Id] [int] NOT NULL IDENTITY,
    [Title] [nvarchar](100) NOT NULL,
    [Description] [nvarchar](max),
    [InsuredName] [nvarchar](100) NOT NULL,
    [InsuredId] [int] NOT NULL,
    [BrokerCode] [nvarchar](10) NOT NULL,
    [BrokerPseudonym] [nvarchar](10) NOT NULL,
    [BrokerSequenceId] [int] NOT NULL,
    [BrokerContact] [nvarchar](50) NOT NULL,
    [NonLondonBrokerCode] [nvarchar](10),
    [NonLondonBrokerName] [nvarchar](100),
    [UnderwriterCode] [nvarchar](10) NOT NULL,
    [UnderwriterContactCode] [nvarchar](10) NOT NULL,
    [QuotingOfficeId] [nvarchar](256) NOT NULL,
    [Domicile] [nvarchar](10) NOT NULL,
    [Leader] [nvarchar](10) NOT NULL,
    [Brokerage] [decimal](18, 2) NOT NULL,
    [QuoteSheetNotes] [nvarchar](max),
    [UnderwriterNotes] [nvarchar](max),
    [Timestamp] rowversion NOT NULL,
    [CreatedOn] [datetime] NOT NULL,
    [ModifiedOn] [datetime] NOT NULL,
    [CreatedBy] [nvarchar](256),
    [ModifiedBy] [nvarchar](256),
    CONSTRAINT [PK_dbo.Submissions] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_UnderwriterCode] ON [dbo].[Submissions]([UnderwriterCode])
CREATE INDEX [IX_UnderwriterContactCode] ON [dbo].[Submissions]([UnderwriterContactCode])
CREATE INDEX [IX_QuotingOfficeId] ON [dbo].[Submissions]([QuotingOfficeId])
CREATE TABLE [dbo].[Underwriters] (
    [Code] [nvarchar](10) NOT NULL,
    [Name] [nvarchar](256),
    [CreatedOn] [datetime] NOT NULL,
    [ModifiedOn] [datetime] NOT NULL,
    [CreatedBy] [nvarchar](256),
    [ModifiedBy] [nvarchar](256),
    CONSTRAINT [PK_dbo.Underwriters] PRIMARY KEY ([Code])
)
CREATE TABLE [dbo].[Offices] (
    [Id] [nvarchar](256) NOT NULL,
    [Name] [nvarchar](256) NOT NULL,
    [Title] [nvarchar](256) NOT NULL,
    [Footer] [nvarchar](256),
    [CreatedOn] [datetime] NOT NULL,
    [ModifiedOn] [datetime] NOT NULL,
    [CreatedBy] [nvarchar](256),
    [ModifiedBy] [nvarchar](256),
    [Address_Id] [int],
    CONSTRAINT [PK_dbo.Offices] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_Address_Id] ON [dbo].[Offices]([Address_Id])
CREATE TABLE [dbo].[Addresses] (
    [Id] [int] NOT NULL IDENTITY,
    [AddressLine1] [nvarchar](256),
    [AddressLine2] [nvarchar](256),
    [City] [nvarchar](256),
    [StateProvinceRegion] [nvarchar](256),
    [ZipPostalCode] [nvarchar](20),
    [Country] [nvarchar](256),
    [CreatedOn] [datetime] NOT NULL,
    [ModifiedOn] [datetime] NOT NULL,
    [CreatedBy] [nvarchar](256),
    [ModifiedBy] [nvarchar](256),
    CONSTRAINT [PK_dbo.Addresses] PRIMARY KEY ([Id])
)
CREATE TABLE [dbo].[Options] (
    [Id] [int] NOT NULL IDENTITY,
    [SubmissionId] [int] NOT NULL,
    [Title] [nvarchar](256) NOT NULL,
    [Comments] [nvarchar](max),
    [Timestamp] rowversion NOT NULL,
    [CreatedOn] [datetime] NOT NULL,
    [ModifiedOn] [datetime] NOT NULL,
    [CreatedBy] [nvarchar](256),
    [ModifiedBy] [nvarchar](256),
    CONSTRAINT [PK_dbo.Options] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_SubmissionId] ON [dbo].[Options]([SubmissionId])
CREATE TABLE [dbo].[OptionVersions] (
    [OptionId] [int] NOT NULL,
    [VersionNumber] [int] NOT NULL,
    [Title] [nvarchar](256) NOT NULL,
    [Comments] [nvarchar](max),
    [IsExperiment] [bit] NOT NULL,
    [IsLocked] [bit] NOT NULL,
    [Timestamp] rowversion NOT NULL,
    [CreatedOn] [datetime] NOT NULL,
    [ModifiedOn] [datetime] NOT NULL,
    [CreatedBy] [nvarchar](256),
    [ModifiedBy] [nvarchar](256),
    CONSTRAINT [PK_dbo.OptionVersions] PRIMARY KEY ([OptionId], [VersionNumber])
)
CREATE INDEX [IX_OptionId] ON [dbo].[OptionVersions]([OptionId])
CREATE TABLE [dbo].[Quotes] (
    [Id] [int] NOT NULL IDENTITY,
    [OptionId] [int] NOT NULL,
    [VersionNumber] [int] NOT NULL,
    [CorrelationToken] [uniqueidentifier],
    [IsSubscribeMaster] [bit] NOT NULL,
    [CopiedFromQuoteId] [int],
    [SubscribeReference] [nvarchar](50),
    [RenPolId] [nvarchar](50),
    [FacilityRef] [nvarchar](50),
    [SubmissionStatus] [nvarchar](50) NOT NULL,
    [EntryStatus] [nvarchar](50) NOT NULL,
    [PolicyType] [nvarchar](50) NOT NULL,
    [OriginatingOfficeId] [nvarchar](256) NOT NULL,
    [COBId] [nvarchar](10) NOT NULL,
    [MOA] [nvarchar](10) NOT NULL,
    [AccountYear] [int] NOT NULL,
    [InceptionDate] [datetime],
    [ExpiryDate] [datetime],
    [QuoteExpiryDate] [datetime] NOT NULL,
    [TechnicalPricingMethod] [nvarchar](10) NOT NULL,
    [TechnicalPricingBindStatus] [nvarchar](10),
    [TechnicalPricingPremiumPctgAmt] [nvarchar](10),
    [TechnicalPremium] [decimal](18, 2),
    [Currency] [nvarchar](10) NOT NULL,
    [LimitCCY] [nvarchar](10),
    [ExcessCCY] [nvarchar](10),
    [BenchmarkPremium] [decimal](18, 2),
    [QuotedPremium] [decimal](18, 2),
    [LimitAmount] [decimal](18, 2),
    [ExcessAmount] [decimal](18, 2),
    [Comment] [nvarchar](max),
    [DeclinatureReason] [nvarchar](256),
    [DeclinatureComments] [nvarchar](max),
    [Timestamp] rowversion NOT NULL,
    [SubscribeTimestamp] [bigint],
    [CreatedOn] [datetime] NOT NULL,
    [ModifiedOn] [datetime] NOT NULL,
    [CreatedBy] [nvarchar](256),
    [ModifiedBy] [nvarchar](256),
    CONSTRAINT [PK_dbo.Quotes] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_OptionId_VersionNumber] ON [dbo].[Quotes]([OptionId], [VersionNumber])
CREATE INDEX [IX_OriginatingOfficeId] ON [dbo].[Quotes]([OriginatingOfficeId])
CREATE INDEX [IX_COBId] ON [dbo].[Quotes]([COBId])
CREATE TABLE [dbo].[COBs] (
    [Id] [nvarchar](10) NOT NULL,
    [Narrative] [nvarchar](100) NOT NULL,
    [CreatedOn] [datetime] NOT NULL,
    [ModifiedOn] [datetime] NOT NULL,
    [CreatedBy] [nvarchar](256),
    [ModifiedBy] [nvarchar](256),
    CONSTRAINT [PK_dbo.COBs] PRIMARY KEY ([Id])
)
CREATE TABLE [dbo].[QuoteSheets] (
    [Id] [int] NOT NULL IDENTITY,
    [Title] [nvarchar](256) NOT NULL,
    [Guid] [uniqueidentifier] NOT NULL,
    [ObjectStore] [nvarchar](50) NOT NULL,
    [IssuedDate] [datetime],
    [IssuedById] [int] NOT NULL,
    [CreatedOn] [datetime] NOT NULL,
    [ModifiedOn] [datetime] NOT NULL,
    [CreatedBy] [nvarchar](256),
    [ModifiedBy] [nvarchar](256),
    CONSTRAINT [PK_dbo.QuoteSheets] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_IssuedById] ON [dbo].[QuoteSheets]([IssuedById])
CREATE TABLE [dbo].[Users] (
    [Id] [int] NOT NULL IDENTITY,
    [DomainLogon] [nvarchar](256) NOT NULL,
    [UnderwriterCode] [nvarchar](10),
    [PrimaryOfficeId] [nvarchar](256),
    [DefaultOrigOfficeId] [nvarchar](256),
    [DefaultUWId] [int],
    [IsActive] [bit] NOT NULL,
    [CreatedOn] [datetime] NOT NULL,
    [ModifiedOn] [datetime] NOT NULL,
    [CreatedBy] [nvarchar](256),
    [ModifiedBy] [nvarchar](256),
    CONSTRAINT [PK_dbo.Users] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_UnderwriterCode] ON [dbo].[Users]([UnderwriterCode])
CREATE INDEX [IX_PrimaryOfficeId] ON [dbo].[Users]([PrimaryOfficeId])
CREATE INDEX [IX_DefaultOrigOfficeId] ON [dbo].[Users]([DefaultOrigOfficeId])
CREATE INDEX [IX_DefaultUWId] ON [dbo].[Users]([DefaultUWId])
CREATE TABLE [dbo].[TeamMemberships] (
    [Id] [int] NOT NULL IDENTITY,
    [TeamId] [int] NOT NULL,
    [UserId] [int] NOT NULL,
    [StartDate] [datetime] NOT NULL,
    [EndDate] [datetime],
    [IsCurrent] [bit] NOT NULL,
    [CreatedOn] [datetime] NOT NULL,
    [ModifiedOn] [datetime] NOT NULL,
    [CreatedBy] [nvarchar](256),
    [ModifiedBy] [nvarchar](256),
    CONSTRAINT [PK_dbo.TeamMemberships] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_TeamId] ON [dbo].[TeamMemberships]([TeamId])
CREATE INDEX [IX_UserId] ON [dbo].[TeamMemberships]([UserId])
CREATE TABLE [dbo].[Teams] (
    [Id] [int] NOT NULL IDENTITY,
    [Title] [nvarchar](256) NOT NULL,
    [DefaultMOA] [nvarchar](10),
    [DefaultDomicile] [nvarchar](10),
    [QuoteExpiryDaysDefault] [int] NOT NULL,
    [CreatedOn] [datetime] NOT NULL,
    [ModifiedOn] [datetime] NOT NULL,
    [CreatedBy] [nvarchar](256),
    [ModifiedBy] [nvarchar](256),
    [PricingActuary_Id] [int],
    CONSTRAINT [PK_dbo.Teams] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_PricingActuary_Id] ON [dbo].[Teams]([PricingActuary_Id])
CREATE TABLE [dbo].[Links] (
    [Id] [int] NOT NULL IDENTITY,
    [Url] [nvarchar](2048) NOT NULL,
    [Title] [nvarchar](256) NOT NULL,
    [Category] [nvarchar](256),
    [CreatedOn] [datetime] NOT NULL,
    [ModifiedOn] [datetime] NOT NULL,
    [CreatedBy] [nvarchar](256),
    [ModifiedBy] [nvarchar](256),
    CONSTRAINT [PK_dbo.Links] PRIMARY KEY ([Id])
)
CREATE TABLE [dbo].[Tabs] (
    [Id] [int] NOT NULL IDENTITY,
    [UserId] [int] NOT NULL,
    [Url] [nvarchar](2048) NOT NULL,
    [CreatedOn] [datetime] NOT NULL,
    [ModifiedOn] [datetime] NOT NULL,
    [CreatedBy] [nvarchar](256),
    [ModifiedBy] [nvarchar](256),
    CONSTRAINT [PK_dbo.Tabs] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_UserId] ON [dbo].[Tabs]([UserId])
CREATE TABLE [dbo].[Brokers] (
    [BrokerSequenceId] [int] NOT NULL,
    [GroupCode] [nvarchar](10),
    [Code] [nvarchar](10),
    [Name] [nvarchar](256),
    [Psu] [nvarchar](10),
    [CreatedOn] [datetime] NOT NULL,
    [ModifiedOn] [datetime] NOT NULL,
    [CreatedBy] [nvarchar](256),
    [ModifiedBy] [nvarchar](256),
    CONSTRAINT [PK_dbo.Brokers] PRIMARY KEY ([BrokerSequenceId])
)
CREATE TABLE [dbo].[LinkTeams] (
    [Link_Id] [int] NOT NULL,
    [Team_Id] [int] NOT NULL,
    CONSTRAINT [PK_dbo.LinkTeams] PRIMARY KEY ([Link_Id], [Team_Id])
)
CREATE INDEX [IX_Link_Id] ON [dbo].[LinkTeams]([Link_Id])
CREATE INDEX [IX_Team_Id] ON [dbo].[LinkTeams]([Team_Id])
CREATE TABLE [dbo].[TeamRelatedCOBs] (
    [Team_Id] [int] NOT NULL,
    [COB_Id] [nvarchar](10) NOT NULL,
    CONSTRAINT [PK_dbo.TeamRelatedCOBs] PRIMARY KEY ([Team_Id], [COB_Id])
)
CREATE INDEX [IX_Team_Id] ON [dbo].[TeamRelatedCOBs]([Team_Id])
CREATE INDEX [IX_COB_Id] ON [dbo].[TeamRelatedCOBs]([COB_Id])
CREATE TABLE [dbo].[TeamRelatedOffices] (
    [Team_Id] [int] NOT NULL,
    [Office_Id] [nvarchar](256) NOT NULL,
    CONSTRAINT [PK_dbo.TeamRelatedOffices] PRIMARY KEY ([Team_Id], [Office_Id])
)
CREATE INDEX [IX_Team_Id] ON [dbo].[TeamRelatedOffices]([Team_Id])
CREATE INDEX [IX_Office_Id] ON [dbo].[TeamRelatedOffices]([Office_Id])
CREATE TABLE [dbo].[UserFilterCOBs] (
    [User_Id] [int] NOT NULL,
    [COB_Id] [nvarchar](10) NOT NULL,
    CONSTRAINT [PK_dbo.UserFilterCOBs] PRIMARY KEY ([User_Id], [COB_Id])
)
CREATE INDEX [IX_User_Id] ON [dbo].[UserFilterCOBs]([User_Id])
CREATE INDEX [IX_COB_Id] ON [dbo].[UserFilterCOBs]([COB_Id])
CREATE TABLE [dbo].[UserFilterOffices] (
    [User_Id] [int] NOT NULL,
    [Office_Id] [nvarchar](256) NOT NULL,
    CONSTRAINT [PK_dbo.UserFilterOffices] PRIMARY KEY ([User_Id], [Office_Id])
)
CREATE INDEX [IX_User_Id] ON [dbo].[UserFilterOffices]([User_Id])
CREATE INDEX [IX_Office_Id] ON [dbo].[UserFilterOffices]([Office_Id])
CREATE TABLE [dbo].[UserFilterMembers] (
    [User_Id] [int] NOT NULL,
    [User_Id1] [int] NOT NULL,
    CONSTRAINT [PK_dbo.UserFilterMembers] PRIMARY KEY ([User_Id], [User_Id1])
)
CREATE INDEX [IX_User_Id] ON [dbo].[UserFilterMembers]([User_Id])
CREATE INDEX [IX_User_Id1] ON [dbo].[UserFilterMembers]([User_Id1])
CREATE TABLE [dbo].[UserAdditionalCOBs] (
    [User_Id] [int] NOT NULL,
    [COB_Id] [nvarchar](10) NOT NULL,
    CONSTRAINT [PK_dbo.UserAdditionalCOBs] PRIMARY KEY ([User_Id], [COB_Id])
)
CREATE INDEX [IX_User_Id] ON [dbo].[UserAdditionalCOBs]([User_Id])
CREATE INDEX [IX_COB_Id] ON [dbo].[UserAdditionalCOBs]([COB_Id])
CREATE TABLE [dbo].[UserAdditionalOffices] (
    [User_Id] [int] NOT NULL,
    [Office_Id] [nvarchar](256) NOT NULL,
    CONSTRAINT [PK_dbo.UserAdditionalOffices] PRIMARY KEY ([User_Id], [Office_Id])
)
CREATE INDEX [IX_User_Id] ON [dbo].[UserAdditionalOffices]([User_Id])
CREATE INDEX [IX_Office_Id] ON [dbo].[UserAdditionalOffices]([Office_Id])
CREATE TABLE [dbo].[UserAdditionalUsers] (
    [User_Id] [int] NOT NULL,
    [User_Id1] [int] NOT NULL,
    CONSTRAINT [PK_dbo.UserAdditionalUsers] PRIMARY KEY ([User_Id], [User_Id1])
)
CREATE INDEX [IX_User_Id] ON [dbo].[UserAdditionalUsers]([User_Id])
CREATE INDEX [IX_User_Id1] ON [dbo].[UserAdditionalUsers]([User_Id1])
CREATE TABLE [dbo].[QuoteSheetOptionVersions] (
    [QuoteSheet_Id] [int] NOT NULL,
    [OptionVersion_OptionId] [int] NOT NULL,
    [OptionVersion_VersionNumber] [int] NOT NULL,
    CONSTRAINT [PK_dbo.QuoteSheetOptionVersions] PRIMARY KEY ([QuoteSheet_Id], [OptionVersion_OptionId], [OptionVersion_VersionNumber])
)
CREATE INDEX [IX_QuoteSheet_Id] ON [dbo].[QuoteSheetOptionVersions]([QuoteSheet_Id])
CREATE INDEX [IX_OptionVersion_OptionId_OptionVersion_VersionNumber] ON [dbo].[QuoteSheetOptionVersions]([OptionVersion_OptionId], [OptionVersion_VersionNumber])
ALTER TABLE [dbo].[Submissions] ADD CONSTRAINT [FK_dbo.Submissions_dbo.Underwriters_UnderwriterCode] FOREIGN KEY ([UnderwriterCode]) REFERENCES [dbo].[Underwriters] ([Code])
ALTER TABLE [dbo].[Submissions] ADD CONSTRAINT [FK_dbo.Submissions_dbo.Underwriters_UnderwriterContactCode] FOREIGN KEY ([UnderwriterContactCode]) REFERENCES [dbo].[Underwriters] ([Code])
ALTER TABLE [dbo].[Submissions] ADD CONSTRAINT [FK_dbo.Submissions_dbo.Offices_QuotingOfficeId] FOREIGN KEY ([QuotingOfficeId]) REFERENCES [dbo].[Offices] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[Offices] ADD CONSTRAINT [FK_dbo.Offices_dbo.Addresses_Address_Id] FOREIGN KEY ([Address_Id]) REFERENCES [dbo].[Addresses] ([Id])
ALTER TABLE [dbo].[Options] ADD CONSTRAINT [FK_dbo.Options_dbo.Submissions_SubmissionId] FOREIGN KEY ([SubmissionId]) REFERENCES [dbo].[Submissions] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[OptionVersions] ADD CONSTRAINT [FK_dbo.OptionVersions_dbo.Options_OptionId] FOREIGN KEY ([OptionId]) REFERENCES [dbo].[Options] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[Quotes] ADD CONSTRAINT [FK_dbo.Quotes_dbo.OptionVersions_OptionId_VersionNumber] FOREIGN KEY ([OptionId], [VersionNumber]) REFERENCES [dbo].[OptionVersions] ([OptionId], [VersionNumber]) ON DELETE CASCADE
ALTER TABLE [dbo].[Quotes] ADD CONSTRAINT [FK_dbo.Quotes_dbo.Offices_OriginatingOfficeId] FOREIGN KEY ([OriginatingOfficeId]) REFERENCES [dbo].[Offices] ([Id])
ALTER TABLE [dbo].[Quotes] ADD CONSTRAINT [FK_dbo.Quotes_dbo.COBs_COBId] FOREIGN KEY ([COBId]) REFERENCES [dbo].[COBs] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[QuoteSheets] ADD CONSTRAINT [FK_dbo.QuoteSheets_dbo.Users_IssuedById] FOREIGN KEY ([IssuedById]) REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[Users] ADD CONSTRAINT [FK_dbo.Users_dbo.Underwriters_UnderwriterCode] FOREIGN KEY ([UnderwriterCode]) REFERENCES [dbo].[Underwriters] ([Code])
ALTER TABLE [dbo].[Users] ADD CONSTRAINT [FK_dbo.Users_dbo.Offices_PrimaryOfficeId] FOREIGN KEY ([PrimaryOfficeId]) REFERENCES [dbo].[Offices] ([Id])
ALTER TABLE [dbo].[Users] ADD CONSTRAINT [FK_dbo.Users_dbo.Offices_DefaultOrigOfficeId] FOREIGN KEY ([DefaultOrigOfficeId]) REFERENCES [dbo].[Offices] ([Id])
ALTER TABLE [dbo].[Users] ADD CONSTRAINT [FK_dbo.Users_dbo.Users_DefaultUWId] FOREIGN KEY ([DefaultUWId]) REFERENCES [dbo].[Users] ([Id])
ALTER TABLE [dbo].[TeamMemberships] ADD CONSTRAINT [FK_dbo.TeamMemberships_dbo.Teams_TeamId] FOREIGN KEY ([TeamId]) REFERENCES [dbo].[Teams] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[TeamMemberships] ADD CONSTRAINT [FK_dbo.TeamMemberships_dbo.Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[Teams] ADD CONSTRAINT [FK_dbo.Teams_dbo.Users_PricingActuary_Id] FOREIGN KEY ([PricingActuary_Id]) REFERENCES [dbo].[Users] ([Id])
ALTER TABLE [dbo].[Tabs] ADD CONSTRAINT [FK_dbo.Tabs_dbo.Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[LinkTeams] ADD CONSTRAINT [FK_dbo.LinkTeams_dbo.Links_Link_Id] FOREIGN KEY ([Link_Id]) REFERENCES [dbo].[Links] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[LinkTeams] ADD CONSTRAINT [FK_dbo.LinkTeams_dbo.Teams_Team_Id] FOREIGN KEY ([Team_Id]) REFERENCES [dbo].[Teams] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[TeamRelatedCOBs] ADD CONSTRAINT [FK_dbo.TeamRelatedCOBs_dbo.Teams_Team_Id] FOREIGN KEY ([Team_Id]) REFERENCES [dbo].[Teams] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[TeamRelatedCOBs] ADD CONSTRAINT [FK_dbo.TeamRelatedCOBs_dbo.COBs_COB_Id] FOREIGN KEY ([COB_Id]) REFERENCES [dbo].[COBs] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[TeamRelatedOffices] ADD CONSTRAINT [FK_dbo.TeamRelatedOffices_dbo.Teams_Team_Id] FOREIGN KEY ([Team_Id]) REFERENCES [dbo].[Teams] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[TeamRelatedOffices] ADD CONSTRAINT [FK_dbo.TeamRelatedOffices_dbo.Offices_Office_Id] FOREIGN KEY ([Office_Id]) REFERENCES [dbo].[Offices] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[UserFilterCOBs] ADD CONSTRAINT [FK_dbo.UserFilterCOBs_dbo.Users_User_Id] FOREIGN KEY ([User_Id]) REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[UserFilterCOBs] ADD CONSTRAINT [FK_dbo.UserFilterCOBs_dbo.COBs_COB_Id] FOREIGN KEY ([COB_Id]) REFERENCES [dbo].[COBs] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[UserFilterOffices] ADD CONSTRAINT [FK_dbo.UserFilterOffices_dbo.Users_User_Id] FOREIGN KEY ([User_Id]) REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[UserFilterOffices] ADD CONSTRAINT [FK_dbo.UserFilterOffices_dbo.Offices_Office_Id] FOREIGN KEY ([Office_Id]) REFERENCES [dbo].[Offices] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[UserFilterMembers] ADD CONSTRAINT [FK_dbo.UserFilterMembers_dbo.Users_User_Id] FOREIGN KEY ([User_Id]) REFERENCES [dbo].[Users] ([Id])
ALTER TABLE [dbo].[UserFilterMembers] ADD CONSTRAINT [FK_dbo.UserFilterMembers_dbo.Users_User_Id1] FOREIGN KEY ([User_Id1]) REFERENCES [dbo].[Users] ([Id])
ALTER TABLE [dbo].[UserAdditionalCOBs] ADD CONSTRAINT [FK_dbo.UserAdditionalCOBs_dbo.Users_User_Id] FOREIGN KEY ([User_Id]) REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[UserAdditionalCOBs] ADD CONSTRAINT [FK_dbo.UserAdditionalCOBs_dbo.COBs_COB_Id] FOREIGN KEY ([COB_Id]) REFERENCES [dbo].[COBs] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[UserAdditionalOffices] ADD CONSTRAINT [FK_dbo.UserAdditionalOffices_dbo.Users_User_Id] FOREIGN KEY ([User_Id]) REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[UserAdditionalOffices] ADD CONSTRAINT [FK_dbo.UserAdditionalOffices_dbo.Offices_Office_Id] FOREIGN KEY ([Office_Id]) REFERENCES [dbo].[Offices] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[UserAdditionalUsers] ADD CONSTRAINT [FK_dbo.UserAdditionalUsers_dbo.Users_User_Id] FOREIGN KEY ([User_Id]) REFERENCES [dbo].[Users] ([Id])
ALTER TABLE [dbo].[UserAdditionalUsers] ADD CONSTRAINT [FK_dbo.UserAdditionalUsers_dbo.Users_User_Id1] FOREIGN KEY ([User_Id1]) REFERENCES [dbo].[Users] ([Id])
ALTER TABLE [dbo].[QuoteSheetOptionVersions] ADD CONSTRAINT [FK_dbo.QuoteSheetOptionVersions_dbo.QuoteSheets_QuoteSheet_Id] FOREIGN KEY ([QuoteSheet_Id]) REFERENCES [dbo].[QuoteSheets] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[QuoteSheetOptionVersions] ADD CONSTRAINT [FK_dbo.QuoteSheetOptionVersions_dbo.OptionVersions_OptionVersion_OptionId_OptionVersion_VersionNumber] FOREIGN KEY ([OptionVersion_OptionId], [OptionVersion_VersionNumber]) REFERENCES [dbo].[OptionVersions] ([OptionId], [VersionNumber]) ON DELETE CASCADE
CREATE TABLE [dbo].[__MigrationHistory] (
    [MigrationId] [nvarchar](255) NOT NULL,
    [Model] [varbinary](max) NOT NULL,
    [ProductVersion] [nvarchar](32) NOT NULL,
    CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY ([MigrationId])
)
BEGIN TRY
    EXEC sp_MS_marksystemobject 'dbo.__MigrationHistory'
END TRY
BEGIN CATCH
END CATCH
INSERT INTO [__MigrationHistory] ([MigrationId], [Model], [ProductVersion]) VALUES ('201307030937493_InitialCreate', 0x1F8B0800000000000400ED5DDD72DCBA91BEDFAA7D87A9B9DA4D553492734EEAE4949C942CDBA75CB12CAD65E7D4C98D8BE2401263FE4C488E633DDB5EEC23ED2B2CC05FFC7483000992335A5E4903808D46A3D168008D0FFFFBDFFF73FE97EF51B8FA46D22C48E297EBB393D3F58AC47EB20DE28797EB7D7EFFFB9FD67FF9F3BFFFDBF99B6DF47DF5B7BADC1F5839FA659CBD5C3FE6F9EEE7CD26F31F49E4652751E0A74996DCE7277E126DBC6DB279717AFAA7CDD9D98650126B4A6BB53AFFB88FF32022C50FFAF332897DB2CBF75E78956C499855E934E7B6A0BAFAE04524DB793E79B9FE9B1706DB7D7642BFC992909CBCF6726FBDBA08038FF2724BC2FBF56AF7C3CF9F33729BA749FC70BBF3F2C00B3F3DED08CDBFF7C28C549CFFBCFBC194F9D3178CF98D17C7494EC92571AFC6AF9B66D186BDA102C89F185B45E328EBFBBB28C89878F972B4E45FC9939040936ED26447D2FCE923B9AFBE7EB75DAF36E2771BF9C3E633EE1BC600FD2FCEFFF062BDFAB00F43EF2E248D9CA8206FF32425BF9098A45E4EB6375E9E933466DF92A2014AAD521D9F823C247535B447A85AAD576F83EF64FB9EC40FF96353D595F7BD4E393BA5CAF5390EA816D28FF2744F00D6F4D5BE26999F06BBB2ABAC2AA7FF6A2A2F7FEBEB7E1767FB946CD98FE91B5E55DEDDB57A32AFD2E42B492F291FD64D18DA82B2EA9B8CECB749FC14CD54FF2DF9E79EDA41E24A9071EEF9B9655B7E1CDC960F49FC3E89A91CC7E94F93D120B1E07C5498F0F039DE92F45F6990CFA4D242FD8526CCC2C67FED939CD6747D7F1FF08A6D58FF8B1FFF38D82A27747E0C7ACC07432B7E4F3CDA03339912EFA169F06BE2079117AE573729FDAF72B97E5AAF6E7D8F11B5372FAC4BC9ED2321F907FA4F36F95CC769F63C0C7CA25E64967BD1AEAEF955107B29754B984BB94F536AC39FAE0AEA052B9D1CFD64E5055D26D16E9F13D5F792B8BC4C09FBEEBA7146A8D34A18EBD61D4EDB12DC074E48554CBD7A726A084C3AAD6EC414557FF0BE050F85CF8EABEE7AF591844599EC31D895AB8893D61FFF22147D9B26D1C724143C76BEC497DB649FFA6CC827DA629FBCF481E4BDF86DDC0963B69B2FBAB8AF0A1A36A22E6DDB166126EA6A8654186C815046C7BC58D096EFEB624591811C97795FB8651CC7AA92D9545DF3A896A85BC13377BE69D78DDAD524AFB23D9693A57F62BBA09CC5ABE9E1510EB7608B419FC7A01BEB7F652D26DF4999CE9976AEF746B5F6D9CF7150EDDB24C9AD1DF865981FEB30D7CDC117DB6D4A32640E2EA7F5A60837010B398A872065437E81B1E9A96B7F46BBB85593DE0731399B5CEBB8CA5F4C6F010AE94C5CE96D4E7B89A67D0B629F7C240FF6BBD8C379F87BB0BB49E8CA3AECE1D4BD18BA617799ECE33C9D5EF08BBD3F74B7AE3CD27946A6B55DF00D3D6A98C935BB4CA2883675D9FA5B4CCA2199149D0BC9EFD10CDFC9517C4974AFC76E9BA90AB6D0ED365545BE5466516153CC47F69CA44283B69D048A7DCC74494035D660E1AA9E0FFBE88E2D0E6DCD7B5BD710B32B31B158708B4889ECCD77FA3B60B537463CA1AAE9C5F6810FD9FBC4FF4AB643E92CB3CA32ABF49B556A23ECCA5AC3F30A62D26D4E5E083CA514595F440BCEF108642B130A54069A4F3A392C8EB4356C16F95FE4795262162A04B30C961C341116649FD33AE5E026CBCB841AE3523B3E255F4963BE7ED9075BFB89887A6B2C62F08E5C7919B7D3DC7716B94C76D41C317D2C14412336A3FDA09A39AA2684CD40B64E823692CC84838F24BE4942EB638EC1F5BEF5FC200C8AF13175D5ADFFCEB6E3F6B6DED1F0E0BD376C0B6CA6BA6967077E61CF26AFFA3A0D1EA8CB356FA4DAE5F52BEB6A871F655F5D5F4C5EE785EFB3CDD6DF8837D01EBF2B42F8E97061FEA3CE9734197C745D10A44F2E2815F6D78C9CD9FA80F88F54E05E7893063EEDA22B923F26D3AB8ACC065DA56C7BD98AC101BE3227372989827D74E3E70F17916DC8B34B6E0A36FA445E1A9D14544BC1E943598328C82F2F7F9B5AB06FBEFB24CB66A8F81515F363E4A55F47EED1C2506C47AEA4E8BD8B8859DCB1AA283B6ADC3AAA1DA3C9378C683B42E61BEC53EA0A7BD90CC7A21C07CBC187D9A2456197FA177FFC61399A7D2E3B5ECD5E919B3D2579DB4BB7EF64CCA8BCA6D031AB965518968B604C2BE56C19A7EB100DAB45AECC1C4D44D8613983229B587D871A51E9229038A5962EF866BBE475706B73316E071E77D2EE123FA73DDD99CE1BCB1D5A78B7D66C8BE8EE1FC4CF8BC64FBE3DF52ECBF664EB6267A2A4F4EA69E89EFA623C0ECF33AAFBB6EB04AB2D079E5DD5D9F0842E951921CA64E0599B866BE454AEDF5DA7ACDF25A74335CBAF93C80BE2F7C983E3F5ADD1C01DF1AEBCC9E8BD498388AE6547D9F9375BDFDF7BFB30679EFBDC3C7CFE75E0B1E1BBECC2E7FDD9DE8799CB04737013CC27E24557849DA233730D5B6FB1CC97C24C72761BC856A223A032B6011D6F83901993EB573097054DBE4CCBA194A5CC2772BEED0C587E5A8EF34EE69A62207F55AE8EC5BA483F2EAB5EE8E2B229067259E5EAB8AC8BD87279B1DD062CD90BF53D2D9793F814B36146A532FD39EDEC79A028CAAF5603D462FDB966048D78AE0AA21C17F95DFC9685EC7D4B127FF2EE10BBE4DDA9C6A84A532D509D616B76BAF01D0AA208B2839C070B69089A83E0E7E0FC49C5240E855C9847B1882D978A3784730A1495B8554AC01CABC57A72FDF9D74E6E591198CBCFBF6AB9A3D983962CE2BCFA9C162FAC6543773498A487D2B8CDBD347712F1F12676B4DD53C60C0C8E745FDCF1C374C7511FFC8BE0A88B0E389F077ADF4201EB39304326BF1E4B03D91CEA960FBD6DE2B3B284F3ECAE573394E358428B7D8B3150FEAC23FD9EB28A9B656FFDB9D95A936D0F63930B5935D0269B72F73E88BFC27CB19C2F8CBEC0519BAA987F2ECBD6F017D5932DBA142F9A29149264C4E5C132E20BD8CAA8FA56B7FCE6EB00D6DE40B696CDBEABEE2AA6F5C2CFF7458415C6A95C4EE254CC863995CA0C9A4A99E63CA7A9F4731ADA1A9CD31F7E3A52ACAB4B2AA28764413F5926BBCA34F49C4E642B03CC34FDFC74EFEE59D916079B0DF3D8A765BC1EDE78C557DCBA4D6FC5219077C37B0DD4127EBCCF5855DF40B01DB9B6AF28E0E3F843127776FD2F69B2DFCD11AE30CB8B0ADE0C80B737D97E72D92EE6EDA082622FB22CF183C2AEB5573C208874B1196FE2EDCA0C2FBD6C9488FE4D5BB20FF36017D2C551FEF472FD3B454C9DF49B65754B5F38F6132B38532AA82EDDB357A4D89B5379EA0571AEDAD020F6839D171AF1227D0DDA6008829B754E53939CF39AEC48CC9C1E23719BB0A0C482A9DC34954AF34397D0CE379C3AF5D1B21AD1DE4E191478FB71744EC6C53F00D593589A5103A53EB05644EE019D59F4510219C63403431C6E55A13E2737D73604A698A3D9401F8B444F4F4ECE642BDF67008A6F31180C0AE4610687C30E7ED1A153CC67B2E29D5FC7AF494872B2625193EC86ECA597F91EF40E02E5C3ED3805DB60323040A0BC5E0314EC2813169457A5E619940A66233A867000474E672A882E8BA189BFF061A2ECF3AB23DA8009F410ED1393BA4504DD19B54FC277D36B0A06F626EB60736BD6561531ECCF2E253F144D84F99F4C19E1FE31A95E03613A852E4257AE314DD1DEBF6E15A504B3B3D0402D56A0A186CFAF879A560CD303B0B816CCD64A8135BD3A07DFD3E9BC7273BF433FF16BFCC3751FBDFA6FE0993A590676F0318121ED90B2912E42607033AA988A802077BC00BF30588D78C8068E5A01FC7098E692E3783215E3646EB4AF5182FBCDA2464A7012D6FF78A452AB0665D4E9887AA06107D04A29A8BD53D97B6919269709940D938149D57558FC2C4AC7850A60FD0BC50DB43D5BC41359182F28A84DAFB6BFEBBD3BA584B26955188C6BB3624D4FD3CC58BB696D1D1767C29C1224E7A6CD725C9DEDEEA66DCBA5383B2D8B58D0DDC09623717AFC167FA6EEED0FDA8085A2F5753CA2A1FBFD6DB5BE024321CCEFA368989F680241FAC6A4F63A5269963944BEA68DA9077A675BAF1A3A8DC3AE798F6563814BDF06AC692CEC80064F6860815BE4060C2A57CA9D345BBE856E47D5B6D1D29D742D7FD805F581CD46EEB48FAAE2EAED7643165DAB3A7E3B7E6C75972FCB1BB229DD9C77D67CF1B2FD684ADFC422A273BC1298C8790EDE9D95BBA05CEC3F781F41E2780AC74092F771780326315F3892C2C041D33FC24B75C5FB6A0AC688D126D4D0E01A4CB246BA7310815D00C285B6CB11B88B818A042364744E3C8EB508E46202CB830BD6A47205AD6D3E3D52F147B4BDAE012319A84F387EC9D43A857232955EA14236610044E29B5DBF3EFF6AA4573C7C8C1B7DE21067ACB79A5C68515BFFC4DAD38AD2426B4AECC4F90E0A257457AC83B550AFD2E161893E6D7B828860C41EBC13AE617E02FDD3F48B51ED1CB2F3DC3A28E1F61A280B06E2EB501FB13739FB05BFA18229EFB11411E3414CD2C6F38E33CAD047B24BB2204F806304F6D52DC995F8C26CBD6AAFC600219D4AD345429CD70D5112962C1DA41A4804854AED597410A8E2C4210A4D0879170F4517813C54818546045A4869844EA3041DE4EAA76D153255EC45C7E7250C86F271B1ED665273FD642D5C7D355ABA342443542333D00905125621231FFA1810C4C8747E5CE1A0281F9747C95D3517D0916AC56CEBA9E3D3F2E229F475751D5626C0D90E68CC8B10915C6165FCE3FB218219ECB805D7B447323CCA3C627CEF8DA3285A2079E61625D15B4AF5CD2C63618157B90C5A285FE672263AF9FAD6781294EE120122D3DD36129A82DC37E2786F260D8D40901B461C9576E670A93FE2C522BDEA682E21619D0B5F431AA630F0C5A36E79F75113E5760BA429FA2B306237A3976078FEEB095EA72FE8B51733E1F6168674D9029587EE5206D012E45A862295D66BE9140E7211A35BD03D6403BEB7A68AA6F38E80EAB823B704B856D4FE974620BA7B01C622EE2F16F565375434FA5072A8516830792F11A1E1E3A3D817EE39394C224AE433C0351FFBDCA7D57CB433F77DE9990F6EA48A0BA8B6551F9E2BB08C06E8729C571EB4A6E1685CAD4445E07BB02878EC28550858B8A8C0381030CAB15C79FF9A860321A25D82EBDBE702CE21D2E76800A9DA5B5008A97D9F4341A323E9BC0CA3A81701180B82F22F8782F416841CF9318A9D03B11A1171E8424DD55620C1A6F6E240C24BF92548E666ED016240C3C2D0069F2A6DC0C24FCD4D5A27C911C5A2BC47A38A441B0BA99EE000D1905D7C6B898C672EA0E76EB4CDEF36169A08C9BE4298C45400D1907A518031935813E4A8C9BEA290E324C71A0FF2AB3D88247481946A1390504A7B5920C193238D0FE051A04E71188E133CC4728850A61B2FCADB439D8201C22E754D11032F8708450CB51C637A6D001F8139150CC014673D3904939F3D8BED71DD9429075D8E65173A36C7F561826AF7746C881B76F2B49BE040141B26094DAC9BDA0A38DACD5E1A707CDB78360078080A114747C896DA143C68CB5E2C7898D6E8A261AF4EE945224719A1EC737146BD45C045168D6026C0C78E914D265DD40C164AA0C4CDC81B4FF5B96CD7EE13122933AE48E49D4EAD6074A11C96C11CC38484846FB8D8C1ADC1479BA08D26EF7C73EB3F92C8AB12CE37B4884F7674991C5E255B126675C695B7DBD11574D67E59A5AC6E779ECF4EFA7F7FBB5E7D8FC2387BB97ECCF3DDCF9B4D5690CE4EA2C04F932CB9CF4FFC24DA78DB64F3E2F4F44F9BB3B34D54D2D8F882E0E51093A6A63C49BD0722E59651DF6F8334632F9379771E436ABDDC464031C31095BA3E355245EDC8FA38BCFE86FD5F41B87861B0DD672755B5278CB913FC1CA995E85BDAC888854D15D8CDE0898FFA31FDFCD6F7422F05C0DE2F93701FC5780C17FE75F558034FA04A32A7F19A647E1A54074A3C2521C39CDEBB38DBA7645B02360B4DE333ACE929826A93CD6995D11065F43F4F8C4FB7A5769391FD36899F22882497694B9787115709E330E526EDAF621520115459E6343F24F1FB24A68DC4840B16E84D5FD52BB080397DE55A084FBBF3CE88215D0EBE1525AF8178C56B51802879F29D289538DDF63D33C12434A9E694DE136FCB164A3C9D3ACD5673E9EC02696D916C27B37252FF509ED1C93213327BF5384058CDB5B1F311C9722FDAC9B6BE4936A7C521CCF3B4B864735A3CC63C4F8C4FB7E68CF9CA0067AF94474D4C389389F1E92AB5F38D34C7CB0EC546F128241F4F76528C5C1861713ED887D16C3D183831DAAFD19E532C9BB59557CDBAA51D5FF4FAE0F41ADF00B6546978A7C4409BB10FC771C7872BB10B87FE6D92E4F2845BA72DC3E988875313843A783C2181B506030AFD729C115555F73E88C9994847CCE945F1054AF185953E06B9AC8A81FA029B8EC26D4E3597A67D0BE88AEE23795096E2600173FA7F0F763709751443759E96B22C5A9DECE33C951B5E272E76E688ED4C1D993B7CDA06C38F4DA66DE4C3718C8C88A52F8C3B2DCA3E4ED1C5347E99444C36993CC2EAD465F5B80C576EB86A4E487A8D5A2442DE78F0A2DF63326F11D87989EB70D9314A12383B4FAE03B71DA7795823FA5DF6E63BFD1DB00F25CB27E4D8507C9FF85F896C479BD4C5DA2CD6A6DD1F756065CAFBC9F6D605F96E1CCFE0B06DD26592A6D59BBB9F92AF441E244AAE8D35A09E0F3BFDBBA3DD9929BB0940B60DD73BAAD3EC3DE0A22B65E102D9569E5CC956052BE113C59F53F2CDA97F24F14D12CA0CB7A9165B349E1F8441819E21EDD3F0197D3C58B652DC6798175BE79A537EC3D673105121C39C1E1555E017764224C7A75B8C4FE8A50C61A89A3CA5A1D1D5F2D104413FE17714701A57D71792596709161B16BECF56D5BF114F1A854286CD793A0B20A1CAC09E0396CFD4852C0B2DF9BE0BD22795209F6E793E879154322D3C13E23FC601CDA9AE9B5C91FC3191BA172BD3BF965741BC850690AE5CFFDA6E521205FBE8C6CF1F2EA25C5FA35CB657AD0509B49E2AD762C4ED53669765B7AA49B538770EA220BFBCFC4D3A796E526DB4DB2759A690E2922DCEB169331E232FFD0A4A4ECDB51C355B90AC946529C38B889919408C7586AD242182628EF59A0A5C52D94541F9219B28F629F50DBC4C8D8552B27BD1869780608179565C8D87841085F297F5DC11AFE7903B4B96AB39E0A2BCC15A0EFC6AACA3D934A56EE037E57CB6495EB4F888B5988FE776B335516297F5DC9F403E1E47B55D6C4DFEB20F241ECA148BA5D8DD3F889F17D29096607C86CD1604BB9300AC51B8745B6A2538A54A0D06ADC4A92D06E0E00C0076A5D236760EB89F6830E8E1CFC619EEAF93C80BE2F7C983E2A3F219F3C73F2B30E9C2764F1786BAA6FD1044B6E84D1B606877D22FC19401BA30CAB2A6B733861E2C3B1E6DEA62768ED8ECC8901E830D9084566A6F8ABA088CE483542F7D8ADB40F0EB9F3895FA7517C102212FBEE0546E732FCD55C7814BB6D9FE065C9026D1C608945B58CA696D93BC98812337038E067FCF217F4C8B8D6A22550E46F8746B6AC8852539B3E729C4535651D21C46706596C17CC483B984F11B3C980BCC6FFBC10C7F36CE60FE9C86D264CB12260E8AA2DAF4902881C44DEA32948E782815983DC3A745EFAECFAC087D35D23872E2B70E1D8D8BFE1F9CFED7EF320C1E02D5330EF6A300FB103D9F1E0D7BE09734D9EFD41D262ED9E6D477FEFBA637D95EDAD36209CB703D86E12A22F1409756DBD7298CAEA6B6C5CD6EA0328821F900057C8442158DD1FC5553811FE3E22AB7E60B7D11ACF7BCAAE587CABA042F5CBDCB3EECC3F0E5FADE0B3362DA5A1974C95A1B78D0700387BF2E0A79F5D0321D90B78A31DE53D60521070AA08296F764A8807AB6636870FFA958E8067B30E207D0760BF23000B02B0A21A74F273F33B686752B7BC060C65E6DE00A2D3AB6F906EA5B638BAD81843FA01E9680707B7256CD0273F4B30C5D6FD2CFF237265BA6981C6170FA9E729488B9EA6B1889FFA026E9CE960FD61605C4DEE0B05F280F1DEB9B597A04F2BE670714D41CA80682A17F3C761E82E637EF54D4CAB332C6561EC7F23F9CDE3D661B0FBD3960DEC7CD27501F6368E3B820A5270A0EA787A5370F86310600A68CDDC1F25B0A263D2C7F839867F5DD6E4492F0D30B87D0C7F05B0E4766A7812722EC7AD9C05EF7E8EB0334DAE82B15C767B895C72FECBABCFA0833DE3DBA5B782CE3B03A5B787DE3982CB806BBDD38945EF9120F9CEF7CA2138AD4D021BFF794358F903F5C193AA1E40DB9D4611288AFAEC2E73330550D3E8148BA03AD6088162A30F87291E658A04A697E3730F81504BD808D5F349921DD174DCD2A387C1993BE2CB25EDD3094B52DC3A3BF7DCA721255B8F0FF0C2FC3A008D4AB0B5C7971704FB2BC005778B97E717AFAD37A7511065E563E62608FB64FB6D126CBB62180B5CF86591DDE88A0CC9FFF95285A5877C44772CFE9B8BCEC963F3C07C645F9A47CC004500CDC5F08ED1FB6BD74E3E5D43D8D592952B0BA5EB12D00EF8E3D9F506D036CB4E4AB9091B286F89B97FA8FEC46FB95F7FD3D891FF2C797EBB3D3536BAA027ABD48FB3F22EFFB7FF204F374DF494F40AF77CC2B8765CFC9D992088FAEAEE5AF27650ECA7E0CF2FCA1F270215428F61A467FB4671404B237968589928150F6E6CA66528572A3C36D676288F66E6B5180ED35E45FFCF8477BE3D1C48BBAE5BBC6BD1F63001520F825E12DF183C80BD95C45FFCB8A7775CEE8ECC4667E9AFDA297BC05507C070655C5C3774094BB955E524B937F7DAB9F844726AECB24DAED7306726629152E54A3923BFD99076CCC5A52E2E3340692E2A234CC07858968F9888D2194F9D80DADA38322D19B793A504450B7AF3386B5EA34E33D7A63D1BD51750F3A1D98C2C1763683D9EA9C234FBD0FD51A9B7E1920F30F1095321F5B05FAE503461908267FB4EB5811917E945EA8B0E91D8F94A2B16E698280F56EAB9090EB75C4AD17490D8EFD6293E6B749E69336801A7FB4D644849EEFBF2332CE9CDDC2942D8BB6650C2A63103CBB311B8AF8D10550587B18D13D84DBBAFA8F2F8985673D504570F992E05D60DFD41656BE3F8DC5683C1FA30160BA1FEDBC7D503645C5802F69EDE3E09F7B12146DA43D9DDA1B020500BEFF480610DF0D179AA0D7A400BC1B1F029954D0A2BD3B252BE0BE3BA5AC82BFBB3D141370E0DD92E631E1DD520661E15DCFBC2550BCDBADE4021BC52D490146BEBFA991B0E3F1B9CD4469797CF7619414B8F881932E860FEFB64F74D8F04E0F9CBB20E1C7AAAC4222B73FAE34DA6669D0E21D9FDE36D8F14EC5C2E1C83BA5AB82C98F236E095D7E9C4A04C0F971AA1021E84752CD1A99DEC16A0CC0A477BB0E0061E98F6DBF0702ACAF7DD5871E1EE6B2141C7529A85CB79BEDD4B5D7417F8326EF384270D1BAF1372000D0F6A3DD85186733B1C484EFDE42305B83F1B8F06E97773C46FCB0E5038F0F3F601B6619BDA3068A29D73C8F76DC0A18EE8E476FFFC85F935E5710DD5DFBA200A8FB285594F8EEBD77205B60F7013BA28BBD18D35EE820D28FD672D468EBFD67A91AB1B23F050E627DA0B23510EB43A7EF065F7D198D073C1A9FCB181CC5EBE661D19D4ED90A38BA53EA1836FAE246CF1DD20B203119BA3BC6835A45313FDA415DA030EB3AE0F4879F0EC452B4A8E96E15661978E3CE803252F9F18E95C12EE428A36DD1DF51F517C2183753E12E3CF16E857679799C832277EA098D7151DC737FA5B0C02F77DBF065DC8D39EE605069B391878343AB651154DAEEC1D954D27F4C36759B92B05A77F63C6DC4617AD5B2301A5BB7ECAC1B0E687A55F590434E2B71F6BF356B235114ED6C12A172B50FF2DEAD4E377AEA298E63E64E4F9B3A8E444F31EC57F7121DA0A70E843A8B9EF63D89B3112B8680378954DBCA9DCF47309AE932D0870FF4D145FAFF74A48F2ED7E73AD48DA02ECDC4D985510928AB215264E7A7032F5F4A9C0FD07FA441AE28F6BC8D8569038782591F9B365774BEE098476FE2EDEA63C2EAE2CB54DC30BCC91321FD6A1FE6C12E0C7CCA01B5AA4A8B5B721C9E244F8D4F1689FD4E21565DB6CA0306C6106779EA05EA935D376910FBC1CE0B81664865E1790B7CE1ED7CD3D095735E931D89D94EA6DA5093FA947019B5EAA60649F5BBE42140A1F6D18D1AE0705191C351110EEC706E4D11E0115125A9B2F91EAD9364D590DB731DBF2621C9C98AC53AB13B11975EE67B10DE1AADEED0B40A7EF903EC6018F6792C7552502D67D123E94143ACF784F711ABAE6BD2C47E3B3D39D15917633D74D4FF60CBC651000B55D3BFD83845BF174ECF17145E7AC0C01DC9805C0BE8CEA5E25449A3288EE5701EAA3B009A115293881534A3F6884E383EF39876DCA88A53AFBA54369A9C71E61FF38E75A242C8430A9A15DE6C5A54AC09BFE816C5833B7124952A214C782EAA941155C84DC782C53B5F7930D64100DB652216A7D357194DC285CB8DFB4B13ABDA64DEB285A640F01D332A807A3CD7F616CBE3FBAAF8FD0C4D12F04EE6BC4A52E1AFCCA716D5DE66758310DFABC9E44D9A0CDA9D194F45CA7BAF8A9E54C9E36CEC000F338EA62DC0BD5EAC36EEB6E72C7A539C398CB407DCB10A3752C363DDD733D6B7C3D8F42DD440B85039DCA338A0EE9FCEA530EE77E5F6EA7CFDAEDC745DFA7EDCBE07AF16CFDEFF9F7F1DE63118F479F94ADD44267FA219BF699745CF9737BE67E9F12220B1BD8B8C6FB71771BE7C5F9509937889D27D69990B3E6B14DD51639CC7D11DDDBD70A4C6FAD6F56CCAD3F2FB0508BE3B8C55C6DCFA3395EDE9A13FF585A9F98C8F781974D439C7C8861D619F9B7A98F2A5DB793ADDBB3B604BE1DD890AC27E1FB77EC8773B916A663504C53D1DA6C9F08BD95C0715F7BAF91E2A1326518EE6BA935CFF886644BDC73E8E9AC057B990BAF0BB5B132B0BF470F981F8AC7328CB547EAA95B2E037AB2673313EB22708C8F6F2FA55A77D997B99239FD6346947AD30E055C7E3D0970E1333DFE9DAF4DA32D1099B8DB2C0778966D09572E3EEE0CD0BB063CB271FBD91B1D8DD3D1C3B53EB4E87A9993B5C7A2EFD996EC7DE5283D05B7713E950B15BFF3608F3E2DE66A7ED99735D5DDF2197AB1F6FB29A72BFDF74B6C2AF1ECEA12E07EBD94CAF2C13793636BA32AB67C3A98AA16333B7710126263EF9E84D8CC5B4743056E648FC9AB9D467DA4884E3F46BAAD32C47D6471F8D001279261109E6F16787623CEA9EEF301E4BCF3BEE7945565376FDC5761BB0344AEE3816346790937AF60CD4460502D2ABCD0169CDA1AF6B26D599695736662A33FFD2A6D598A35ADEA89AD3A41FBDC10191B290EA0ECAE61CDD2A676A2D9A639D63A64707B1D0693589FD9C6EA9A3AA41957AF4A6048086432A3B28435276FF54EB9DA5FB675FF270973485FBF49D36C0FAD6E4E8B737712C04B4CC287A667BDFD2D5ED4E7B3C842EDCC47915B0CB99393CE88B8352C36383C7E8A9C3A6489E061F4F0FA6F1A600C72C60F38298A4DC4DDFB7419AB15704BD3B2F538D30FBEA96E40AF2D17AF5A681DB0400A96EFD4712792FD7DBBB842A6609DAD9E667C08C2D56245CAC566A1272A1AAB802DD75D50B13A59A3A03AAA15A0775126F30DA14EA4D0E44BECA34A8A0467252B9AF3240EE8B3C53E28DF540EA68F2F1AA6A53DB59638599A1D454A543351459DD948B9D2F856E910A51659B6B66DC563E08CC729589F25DE41B8C870C1E08193A023213D597EF832915C805A0AAC43266952255E11574932DAF462864CB64882CCB31E096DD89519965A920AFDE5D37CDEA0527956C9D01512EF3CCE48088B8CDC2E46126EA26F016EC447448B14C2ED8D8A81AD438F3991D9599DAEA26EA061C6768B358661B6864540BDA2A3E535F974DA334D6436F4184C34953F99DE1023CC3AB128F352CA48855D7E476D7682B4BACCA2AAFBB4233EB8C7AF19AA9C66046460AABFC70AE24E4020AC8392BAEB0E20FE2183BE2668DEAF4D17AA574C5576EBF57BDD3E2733E595E67894DECDDFC1A20DC580A20A2F8310B43C4C0D6CB4183970D1E2270ECD74933375C026D069AAB837516D895160805AF4D9AA699C6C2E9D33C059B186AA11EC078589F00FB2F6253AB24474D958074D1D6EA0077FBB1ACDB68523E6E7206371B427E055ADD0910EB827D75774BF8B64A71D564053C146FB61E67D481A91ABBB185CB8A350F889590E34638C68ADFF335450277C41AA5C380040E96F86935EB984FD5A57EDBCA2A7970538B139A0EAF4A29339E0B6124A4BECD14F1F6B0866A50F91C8CBF511BA802CB618DEC80A03B92867EFEB5AB8132C6DAC011C90A94986AEE1BA60085014DD3838909ACF2FB31059B6582A671F0CE5CF32D9FE5A4B132B015D25E2DFED5C0FE9CBAC9321613D6C53AC826074DEED48B3ECDAB5187A036818844431BD2EE9296ED60BF07374305CA01DAD381A623B0C9EF11177C96099A86C9BBA9CD578E7A4A8577D137113AAA1E686AC66E22064A828DB72EFC12079655F66E9B34F78DC57BD5087C6390533E5D53A5C0E28ED6EAC2901DF42EE023F1C9A334DCAC9B75D1B30EDCBC291ACF5D5AEA18CC4849D72EA0ACE04D9AF3A6E27D8C94743792276B68F740C60BBBEE594095F9E4319A6DD4C5238FE2899B2E5D3AD5375D7743D5418F835FB95CAB82B72DCD9ADCD9DB07DA64F89621D666833B896E6CF61964CB80A7C79D34BAA3A30DAED4B9B0DE5335D9D0869B5D0B7366C7D5D637E96309C0B8DB27B3E8130AA1BC6061AC0342F1318C9CDAF42A759C861BF7BD50FCB81ADE79A5427F4E62700D63D81108F2357E4E8896195154B8A2D85D1818E728747481B1BB064268789377BE29A387AA04FA334F52EF815C255B126645EAF9E6E39E7E1D91F2D76B92050F2D89734A3326C5558796685DE65D7C9FD461F2124775913ABBEAC22B927B5B2FF72ED23CB8F7FC9C66539B9D05F1C37AF5372FDCD3226FA8ABB67D175FEFF3DD3EA74DA6AE5BF8C40B8345D6EBEA3FDF283C9F57C1D22E9A40D90C6813C875FC6A1F84DB86EFB75E98495308468285ECFF42687AD99739FD4B1E9E1A4A1F92D8905025BEE6A6C12712EDD82645761DDF7ADF08CE5BB70C45899DBF0EBC87D48BB28A46FB3DFD49D56F1B7DFFF3FF01E14F0CB226F40100, '5.0.0.net45')
