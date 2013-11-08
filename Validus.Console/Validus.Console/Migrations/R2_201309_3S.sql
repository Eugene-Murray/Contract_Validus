--FI team and related risk codes
USE [Console];
SET NOCOUNT ON;
SET XACT_ABORT ON;
GO

SET IDENTITY_INSERT [dbo].[Teams] ON;

BEGIN TRANSACTION;
INSERT INTO [dbo].[Teams]([Id], [Title], [DefaultMOA], [DefaultDomicile], [QuoteExpiryDaysDefault], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy], [PricingActuary_Id], [DefaultPolicyType], [SubmissionTypeId])
SELECT 4, N'Financial Institutions', NULL, NULL, 30, '20130906 13:00:57.123', '20130906 13:00:57.123', N'', N'', NULL, N'NONMARINE', N'FI'
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