
--Marine&Energy, Cargo, Hull&Marine Team rollout

USE [Console];
SET NOCOUNT ON;
SET XACT_ABORT ON;
GO

SET IDENTITY_INSERT [dbo].[Teams] ON;

BEGIN TRANSACTION;
/****** Object:  Table [dbo].[SubmissionTypes]    Script Date: 09/30/2013 12:36:54 ******/
INSERT [dbo].[SubmissionTypes] ([Id], [Title], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (N'CA', N'CA Submission', CAST(0x0000A24600F12A4E AS DateTime), CAST(0x0000A24600F12A4E AS DateTime), N'', N'')
INSERT [dbo].[SubmissionTypes] ([Id], [Title], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (N'HM', N'HM Submission', CAST(0x0000A24600F12A4E AS DateTime), CAST(0x0000A24600F12A4E AS DateTime), N'', N'')
INSERT [dbo].[SubmissionTypes] ([Id], [Title], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (N'ME', N'ME Submission', CAST(0x0000A24600F12A4E AS DateTime), CAST(0x0000A24600F12A4E AS DateTime), N'', N'')
/****** Object:  Table [dbo].[QuoteTemplates]    Script Date: 09/30/2013 12:36:54 ******/
SET IDENTITY_INSERT [dbo].[QuoteTemplates] ON
INSERT [dbo].[QuoteTemplates] ([Id], [Name], [RdlPath], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (3, N'Marine&Energy', N'/Underwriting/Console/QuoteSheet_ME', CAST(0x0000A24600F12A57 AS DateTime), CAST(0x0000A24600F12A57 AS DateTime), N'', N'')
INSERT [dbo].[QuoteTemplates] ([Id], [Name], [RdlPath], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (6, N'Fine Art', N'/Underwriting/Console/QuoteSheet_CA', CAST(0x0000A24600F12A58 AS DateTime), CAST(0x0000A24600F12A58 AS DateTime), N'', N'')
INSERT [dbo].[QuoteTemplates] ([Id], [Name], [RdlPath], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (7, N'Specie', N'/Underwriting/Console/QuoteSheet_CA', CAST(0x0000A24600F12A58 AS DateTime), CAST(0x0000A24600F12A58 AS DateTime), N'', N'')
INSERT [dbo].[QuoteTemplates] ([Id], [Name], [RdlPath], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (8, N'Cargo', N'/Underwriting/Console/QuoteSheet_CA', CAST(0x0000A24600F12A58 AS DateTime), CAST(0x0000A24600F12A58 AS DateTime), N'', N'')
INSERT [dbo].[QuoteTemplates] ([Id], [Name], [RdlPath], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (9, N'Hull&Marine', N'/Underwriting/Console/QuoteSheet_HM', CAST(0x0000A24600F12A58 AS DateTime), CAST(0x0000A24600F12A58 AS DateTime), N'', N'')
SET IDENTITY_INSERT [dbo].[QuoteTemplates] OFF
/****** Object:  Table [dbo].[Teams]    Script Date: 09/30/2013 12:36:54 ******/
SET IDENTITY_INSERT [dbo].[Teams] ON
INSERT [dbo].[Teams] ([Id], [Title], [DefaultMOA], [DefaultDomicile], [QuoteExpiryDaysDefault], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy], [PricingActuary_Id], [DefaultPolicyType], [SubmissionTypeId]) VALUES (5, N'Cargo', NULL, NULL, 30, CAST(0x0000A24600F12A58 AS DateTime), CAST(0x0000A24600F12A58 AS DateTime), N'', N'', NULL, N'MARINE', N'CA')
INSERT [dbo].[Teams] ([Id], [Title], [DefaultMOA], [DefaultDomicile], [QuoteExpiryDaysDefault], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy], [PricingActuary_Id], [DefaultPolicyType], [SubmissionTypeId]) VALUES (6, N'Hull&Marine', NULL, NULL, 30, CAST(0x0000A24600F12A58 AS DateTime), CAST(0x0000A24600F12A58 AS DateTime), N'', N'', NULL, N'MARINE', N'HM')
INSERT [dbo].[Teams] ([Id], [Title], [DefaultMOA], [DefaultDomicile], [QuoteExpiryDaysDefault], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy], [PricingActuary_Id], [DefaultPolicyType], [SubmissionTypeId]) VALUES (7, N'Marine&Energy', NULL, NULL, 30, CAST(0x0000A24600F12A58 AS DateTime), CAST(0x0000A24600F12A58 AS DateTime), N'', N'', NULL, N'MARINE', N'ME')
SET IDENTITY_INSERT [dbo].[Teams] OFF
/****** Object:  Table [dbo].[TeamRelatedQuoteTemplates]    Script Date: 09/30/2013 12:36:54 ******/
INSERT [dbo].[TeamRelatedQuoteTemplates] ([Team_Id], [QuoteTemplate_Id]) VALUES (7, 3)
INSERT [dbo].[TeamRelatedQuoteTemplates] ([Team_Id], [QuoteTemplate_Id]) VALUES (5, 6)
INSERT [dbo].[TeamRelatedQuoteTemplates] ([Team_Id], [QuoteTemplate_Id]) VALUES (5, 7)
INSERT [dbo].[TeamRelatedQuoteTemplates] ([Team_Id], [QuoteTemplate_Id]) VALUES (5, 8)
INSERT [dbo].[TeamRelatedQuoteTemplates] ([Team_Id], [QuoteTemplate_Id]) VALUES (6, 9)
COMMIT;
GO

























----------------------------------------------------------------
INSERT INTO [dbo].[Teams]([Id], [Title], [DefaultMOA], [DefaultDomicile], [QuoteExpiryDaysDefault], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy], [PricingActuary_Id], [DefaultPolicyType], [SubmissionTypeId])
SELECT 5, N'Cargo', NULL, NULL, 30, '20130906 13:00:57.123', '20130906 13:00:57.123', N'', N'', NULL, N'MARINE', N'CA'
COMMIT;
RAISERROR (N'[dbo].[Teams]: Insert Batch: 1.....Done!', 10, 1) WITH NOWAIT;
GO



















SET IDENTITY_INSERT [dbo].[Teams] OFF;

BEGIN TRANSACTION;
INSERT [RiskCodes] ([Code], [Name], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (N'BB', N'Fidelity, comp crime and bankers'' policies', getDate(), NULL, N'system', NULL)
INSERT [RiskCodes] ([Code], [Name], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (N'DO', N'Directors and officers liability', GetDate(), NULL, N'system', NULL)
INSERT [RiskCodes] ([Code], [Name], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (N'PI', N'Errors and omissions/professional indemnity', GetDate(), NULL, N'system', NULL)
COMMIT;
RAISERROR (N'[dbo].[RiskCodes]: Insert Batch: 1.....Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[TeamRelatedRisks]([Team_Id], [RiskCode_Code])
SELECT 4, N'BB' UNION ALL
SELECT 4, N'DO' UNION ALL
SELECT 4, N'PI'
COMMIT;
RAISERROR (N'[dbo].[TeamRelatedRisks]: Insert Batch: 1.....Done!', 10, 1) WITH NOWAIT;
GO