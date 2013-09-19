using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using Validus.Console.DTO;
using Validus.Console.SubscribeService;
using Validus.Core.LogHandling;
using Validus.Models;

namespace Validus.Console.BusinessLogic
{
    public static class SubmissionModuleHelpers
    {
        public static void SetCopiedFrom(Quote quote, Submission submission) 
        {
			if (quote.CorrelationToken.HasValue)
			{
				var masters = submission.Options
				                        .SelectMany(o => o.OptionVersions)
				                        .SelectMany(ov => ov.Quotes)
				                        .Where(q => (q.CorrelationToken.Value == quote.CorrelationToken.Value &&
				                                     q.IsSubscribeMaster == true))
				                        .ToList();
                
				if (masters.Count == 1)
                {
					quote.CopiedFromQuoteId = masters[0].Id;
					quote.SubscribeReference = masters[0].SubscribeReference;
					quote.SubscribeTimestamp = masters[0].SubscribeTimestamp.Value;
                }
                else
                {
                    //  We've got either more than one or no master for the quote.
                    throw new ApplicationException("Couldn't find master quote.");
                }
            }
            else
            {
                //  The non-master quote doesn't have a correlation token
                throw new ApplicationException("Couldn't find master quote.");
            }
        }

        public static Quote CopyQuote(Quote q)
        {
            return new Quote
            {
                Id = q.Id,
                AccountYear = q.AccountYear,
                BenchmarkPremium = q.BenchmarkPremium,
                COBId = q.COBId,
                Comment = q.Comment,
                CopiedFromQuoteId = q.CopiedFromQuoteId,
                CorrelationToken = q.CorrelationToken,
                CreatedBy = q.CreatedBy,
                CreatedOn = q.CreatedOn,
                Currency = q.Currency,
                DeclinatureComments = q.DeclinatureComments,
                DeclinatureReason = q.DeclinatureReason,
                EntryStatus = q.EntryStatus,
                ExcessAmount = q.ExcessAmount,
                ExcessCCY = q.ExcessCCY,
                ExpiryDate = q.ExpiryDate,
                FacilityRef = q.FacilityRef,
                InceptionDate = q.InceptionDate,
                IsSubscribeMaster = q.IsSubscribeMaster,
                ModifiedBy = q.ModifiedBy,
                ModifiedOn = q.ModifiedOn,
                LimitCCY = q.LimitCCY,
                LimitAmount = q.LimitAmount,
                MOA = q.MOA,
                OptionId = q.OptionId,
                OptionVersion = q.OptionVersion,
                OriginatingOfficeId = q.OriginatingOfficeId,
                PolicyType = q.PolicyType,
                QuoteExpiryDate = q.QuoteExpiryDate,
                QuotedPremium = q.QuotedPremium,
                RenPolId = q.RenPolId,
                SubmissionStatus = q.SubmissionStatus,
                SubscribeReference = q.SubscribeReference,
                SubscribeTimestamp = q.SubscribeTimestamp,
                TechnicalPremium = q.TechnicalPremium,
                TechnicalPricingBindStatus = q.TechnicalPricingBindStatus,
                TechnicalPricingMethod = q.TechnicalPricingMethod,
                TechnicalPricingPremiumPctgAmt = q.TechnicalPricingPremiumPctgAmt,
                Timestamp = q.Timestamp,
                VersionNumber = q.VersionNumber
            };
        }

        public static UpdatePolicyResponse UpdateSubscribeRecord(Quote q, Submission s, ILogHandler logHandler, IPolicyService subsribeService)
        {
	        var updateResp = subsribeService.UpdatePolicy(new UpdatePolicyRequest
	        {
		        objPolicyContract = new PolicyContract
		        {
			        AccYr = q.AccountYear.ToString(),
			        BenchPrm = q.BenchmarkPremium,
			        BindSt = q.TechnicalPricingBindStatus,
			        BkrNo = "???", // TODO - not referenced in CreateQuote sp
			        BkrPsu = s.BrokerPseudonym, // No validation carried out in service validation sp
			        BkrSeqId = s.BrokerSequenceId,
			        Brokerage = s.Brokerage,
			        COB = q.COBId,
			        CtcNm = s.BrokerContact,
			        DeclineReason = q.DeclinatureReason,
			        DOM = s.Domicile,
			        EntSt = q.EntryStatus,
			        ExpyDt = (q.ExpiryDate.HasValue) ? q.ExpiryDate.Value.ToString("yyyyMMdd") : null,
			        ExsAmt = q.ExcessAmount,
			        FacyPolId = q.FacilityRef,
			        IncpDt = (q.InceptionDate.HasValue) ? q.InceptionDate.Value.ToString("yyyyMMdd") : null,
			        InsdId = s.InsuredId,
			        InsdNm = s.InsuredName,
			        LmtAmt = q.LimitAmount,
			        LmtCcy = q.LimitCCY,
			        MOA = q.MOA,
			        OrigOff = q.OriginatingOfficeId,
			        PolTy = q.PolicyType,
			        PricingCcy = q.Currency,
			        RenPolId = q.RenPolId,
			        Status = q.SubmissionStatus,
			        TechPrm = q.TechnicalPremium,
			        UnitPsu = "AGY",
			        Uwr = s.UnderwriterCode,
			        ExsCcy = q.ExcessCCY,
			        Method = q.TechnicalPricingMethod,
			        NonLonBkr = s.NonLondonBrokerCode,
			        PctgAmt = q.TechnicalPricingPremiumPctgAmt,
			        PolDsc = s.Description,
                    SettDueDt = q.QuoteExpiryDate.ToString("yyyyMMdd"),
			        TechPmTy = "TechPm", // TODO - not referenced in CreateQuote sp

			        //  These are the fields required for edit only
			        TimeStamp = q.SubscribeTimestamp,
			        PolId = q.SubscribeReference,
		        }
	        });

            if (updateResp.UpdatePolicyResult.ErrorInfo == null)
            {
                var doc = new XmlDocument();

                doc.LoadXml(updateResp.UpdatePolicyResult.OutputXml);

                q.SubscribeTimestamp = long.Parse(doc.GetElementsByTagName("TimeStamp")[0].InnerText);
            }
            else
            {
                logHandler.WriteLog(updateResp.UpdatePolicyResult.ErrorInfo.ErrorXML, LogSeverity.Error,
                                     LogCategory.BusinessComponent);

                throw new ApplicationException(
                    string.IsNullOrEmpty(updateResp.UpdatePolicyResult.ErrorInfo.Description)
                        ? ParseDetailedError(updateResp.UpdatePolicyResult.ErrorInfo.DetailedDescription)
                        : updateResp.UpdatePolicyResult.ErrorInfo.Description);
            }

            return updateResp;
        }

        public static CreateQuoteResponse CreateSubscribeRecord(Quote q, Submission s, ILogHandler logHandler, IPolicyService subsribeService)
        {
            var createQuoteResponse = subsribeService.CreateQuote(new CreateQuoteRequest
            {
				objPolicyContract = new PolicyContract
				{
					AccYr = q.AccountYear.ToString(),
					BenchPrm = q.BenchmarkPremium,
					BindSt = q.TechnicalPricingBindStatus,
					BkrNo = "???", // TODO - not referenced in CreateQuote sp
					BkrPsu = s.BrokerPseudonym, // No validation carried out in service validation sp
					BkrSeqId = s.BrokerSequenceId,
					Brokerage = s.Brokerage,
					COB = q.COBId,
					CtcNm = s.BrokerContact,
					DeclineReason = q.DeclinatureReason,
					DOM = s.Domicile,
					EntSt = q.EntryStatus,
					ExpyDt = (q.ExpiryDate.HasValue) ? q.ExpiryDate.Value.ToString("yyyyMMdd") : null,
					ExsAmt = q.ExcessAmount,
					FacyPolId = q.FacilityRef,
					IncpDt = (q.InceptionDate.HasValue) ? q.InceptionDate.Value.ToString("yyyyMMdd") : null,
					InsdId = s.InsuredId,
					InsdNm = s.InsuredName,
					LmtAmt = q.LimitAmount,
					LmtCcy = q.LimitCCY,
					MOA = q.MOA,
					OrigOff = q.OriginatingOfficeId,
					PolTy = q.PolicyType,
					PricingCcy = q.Currency,
					RenPolId = q.RenPolId,
					Status = q.SubmissionStatus,
					TechPrm = q.TechnicalPremium,
					UnitPsu = "AGY",
					Uwr = s.UnderwriterCode,
					ExsCcy = q.ExcessCCY,
					Method = q.TechnicalPricingMethod,
					NonLonBkr = s.NonLondonBrokerCode,
					PctgAmt = q.TechnicalPricingPremiumPctgAmt,
					PolDsc = s.Description,
                    SettDueDt = q.QuoteExpiryDate.ToString("yyyyMMdd"),
					TechPmTy = "TechPm" // TODO - not referenced in CreateQuote sp
				}
            });

            if (createQuoteResponse.CreateQuoteResult.ErrorInfo == null)
            {
                q.SubscribeReference = createQuoteResponse.objInfoCollection.PolId;

                var doc = new XmlDocument();

                doc.LoadXml(createQuoteResponse.CreateQuoteResult.OutputXml);
                
				var subscribeTimestamp = default(long);

	            q.SubscribeTimestamp = long.TryParse(doc.GetElementsByTagName("TimeStamp")[0].InnerText,
	                                                 out subscribeTimestamp)
		                                   ? subscribeTimestamp
		                                   : 0;
            }
            else
            {
                logHandler.WriteLog(createQuoteResponse.CreateQuoteResult.ErrorInfo.ErrorXML, LogSeverity.Error,
                                     LogCategory.BusinessComponent);

	            throw new ApplicationException(
		            string.IsNullOrEmpty(createQuoteResponse.CreateQuoteResult.ErrorInfo.Description)
			            ? ParseDetailedError(createQuoteResponse.CreateQuoteResult.ErrorInfo.DetailedDescription)
			            : createQuoteResponse.CreateQuoteResult.ErrorInfo.Description);
            }

            return createQuoteResponse;
        }

        // TODO: Implement ParseDetailedError(string message)
        public static string ParseDetailedError(string message)
        {
            return message;
        }

        public static bool QuoteValuesMatchSubscribePolicy(PolicyContract polAsSubscribe, Quote q, Submission s)
        {
	        //  Is this most efficient, or are separate if statements better?
	        return polAsSubscribe.AccYr == q.AccountYear.ToString()
	               && (polAsSubscribe.BenchPrm.HasValue
		                ? polAsSubscribe.BenchPrm.Value == q.BenchmarkPremium
		                : q.BenchmarkPremium == 0)
	               && polAsSubscribe.BindSt == q.TechnicalPricingBindStatus
	               //&& polAsSubscribe.BkrNo == s.?        // Should we bother checking? not used by service
	               && polAsSubscribe.BkrPsu == s.BrokerPseudonym
	               && polAsSubscribe.BkrSeqId == s.BrokerSequenceId
	               &&
	               (polAsSubscribe.Brokerage.HasValue ? polAsSubscribe.Brokerage.Value == s.Brokerage : s.Brokerage == 0)
	               && polAsSubscribe.COB == q.COBId
	               && polAsSubscribe.CtcNm == s.BrokerContact
	               && polAsSubscribe.DeclineReason == q.DeclinatureReason
	               && polAsSubscribe.DOM == s.Domicile
	               && polAsSubscribe.EntSt == q.EntryStatus

	               && (q.ExpiryDate.HasValue
		                ? polAsSubscribe.ExpyDt == q.ExpiryDate.Value.ToString("yyyyMMdd")
						: string.IsNullOrEmpty(polAsSubscribe.ExpyDt))
				   && (q.InceptionDate.HasValue
						? polAsSubscribe.IncpDt == q.InceptionDate.Value.ToString("yyyyMMdd")
						: string.IsNullOrEmpty(polAsSubscribe.IncpDt))
				   && (polAsSubscribe.SettDueDt == q.QuoteExpiryDate.ToString("yyyyMMdd"))

	               && (polAsSubscribe.ExsAmt.HasValue ? polAsSubscribe.ExsAmt == q.ExcessAmount : q.ExcessAmount == 0)
	               && polAsSubscribe.ExsCcy == q.ExcessCCY
	               && polAsSubscribe.FacyPolId == q.FacilityRef
	               && polAsSubscribe.InsdId == s.InsuredId
	               && polAsSubscribe.InsdNm == s.InsuredName
	               && (polAsSubscribe.LmtAmt.HasValue ? polAsSubscribe.LmtAmt.Value == q.LimitAmount : q.LimitAmount == 0)
	               && polAsSubscribe.LmtCcy == q.LimitCCY
	               && polAsSubscribe.Method == q.TechnicalPricingMethod
	               && polAsSubscribe.MOA == q.MOA
	               && polAsSubscribe.NonLonBkr == s.NonLondonBrokerCode
	               && polAsSubscribe.OrigOff == q.OriginatingOfficeId
	               && polAsSubscribe.PctgAmt == q.TechnicalPricingPremiumPctgAmt
	               && polAsSubscribe.PolDsc == s.Description
	               && polAsSubscribe.PolId == q.SubscribeReference
	               && polAsSubscribe.PolTy == q.PolicyType
	               && polAsSubscribe.PricingCcy == q.Currency
	               && polAsSubscribe.RenPolId == q.RenPolId
	               && polAsSubscribe.Status == q.SubmissionStatus
	               //&& polAsSubscribe.TechPmTy == "TechPm"
	               && (polAsSubscribe.TechPrm.HasValue
		                ? polAsSubscribe.TechPrm.Value == q.TechnicalPremium
		                : q.TechnicalPremium == 0)
	               && polAsSubscribe.Uwr == s.UnderwriterCode;
        }

	    public static bool SynchroniseSubmission(Submission submission, PolicyContract polAsSubscribe)
        {
            submission.InsuredName = polAsSubscribe.InsdNm;
            submission.BrokerSequenceId = polAsSubscribe.BkrSeqId;
            submission.NonLondonBrokerCode = polAsSubscribe.NonLonBkr;
            submission.UnderwriterCode = polAsSubscribe.Uwr;
            submission.Brokerage = polAsSubscribe.Brokerage;

            // TODO: values missing from service response...
            //s.Leader = polAsSubscribe.;
            //s.Domicile = polAsSubscribe. 
            //s.BrokerContact = polAsSubscribe.
            //s.Description = polAsSubscribe.

            return true;
        }

	    public static void SynchroniseQuote(Quote q, PolicyContract polAsSubscribe)
	    {
		    q.TechnicalPricingMethod = polAsSubscribe.Method;

		    q.AccountYear = int.Parse(polAsSubscribe.AccYr);
		    q.BenchmarkPremium = polAsSubscribe.BenchPrm.HasValue ? polAsSubscribe.BenchPrm.Value : 0;
		    q.TechnicalPricingBindStatus = polAsSubscribe.BindSt;
		    q.COBId = polAsSubscribe.COB;
		    q.Currency = polAsSubscribe.PricingCcy;
		    q.DeclinatureReason = polAsSubscribe.DeclineReason;
		    q.EntryStatus = polAsSubscribe.EntSt;

		    q.ExpiryDate = polAsSubscribe.ExpyDt != null
			                   ? (DateTime?)DateTime.ParseExact(polAsSubscribe.ExpyDt, "yyyyMMdd",
			                                                    CultureInfo.InvariantCulture,
			                                                    DateTimeStyles.None)
			                   : null;

		    q.InceptionDate = polAsSubscribe.IncpDt != null
			                      ? (DateTime?)DateTime.ParseExact(polAsSubscribe.IncpDt, "yyyyMMdd",
			                                                       CultureInfo.InvariantCulture,
			                                                       DateTimeStyles.None)
								  : null;

			// TODO: Why is QuoteExpiryDate nullable ?
			q.QuoteExpiryDate = DateTime.ParseExact(polAsSubscribe.SettDueDt, "yyyyMMdd",
													CultureInfo.InvariantCulture,
													DateTimeStyles.None);

		    q.ExcessAmount = polAsSubscribe.ExsAmt.HasValue ? polAsSubscribe.ExsAmt.Value : 0;
		    q.ExcessCCY = polAsSubscribe.ExsCcy;
		    q.FacilityRef = polAsSubscribe.FacyPolId;
		    q.LimitAmount = polAsSubscribe.LmtAmt.HasValue ? polAsSubscribe.LmtAmt.Value : 0;
		    q.LimitCCY = polAsSubscribe.LmtCcy;
		    q.MOA = polAsSubscribe.MOA;
		    q.OriginatingOfficeId = polAsSubscribe.OrigOff;
		    q.PolicyType = polAsSubscribe.PolTy;
		    q.RenPolId = polAsSubscribe.RenPolId;
		    q.SubmissionStatus = polAsSubscribe.Status;
		    q.SubscribeTimestamp = polAsSubscribe.TimeStamp.HasValue ? polAsSubscribe.TimeStamp.Value : 0;
		    q.TechnicalPremium = polAsSubscribe.TechPrm.HasValue ? polAsSubscribe.TechPrm.Value : 0;
		    q.TechnicalPricingMethod = polAsSubscribe.Method;
		    q.TechnicalPricingPremiumPctgAmt = polAsSubscribe.PctgAmt;
	    }

	    // TODO: have one method using reflection
        public static SubmissionPV SetupPVSubmission(SubmissionDto submissionDto)
        {
            var submission = new SubmissionPV
            {
                Id = submissionDto.Id,
                Title = submissionDto.Title,
                Description = submissionDto.Description,
                InsuredName = submissionDto.InsuredName,
                InsuredId = submissionDto.InsuredId,
                BrokerCode = submissionDto.BrokerCode,
                BrokerPseudonym = submissionDto.BrokerPseudonym,
                BrokerSequenceId = submissionDto.BrokerSequenceId,
                BrokerContact = submissionDto.BrokerContact,
                NonLondonBrokerCode = submissionDto.NonLondonBrokerCode,
                NonLondonBrokerName = submissionDto.NonLondonBrokerName,
                UnderwriterCode = submissionDto.UnderwriterCode,
                Underwriter = submissionDto.Underwriter,
                UnderwriterContactCode = submissionDto.UnderwriterContactCode,
                UnderwriterContact = submissionDto.UnderwriterContact,
                QuotingOfficeId = submissionDto.QuotingOfficeId,
                QuotingOffice = submissionDto.QuotingOffice,
                Domicile = submissionDto.Domicile,
                Leader = submissionDto.Leader,
                Brokerage = submissionDto.Brokerage,
                QuoteSheetNotes = submissionDto.QuoteSheetNotes,
                UnderwriterNotes = submissionDto.UnderwriterNotes,
                Timestamp = submissionDto.Timestamp,
                SubmissionTypeId = submissionDto.SubmissionTypeId,
                Options = new List<Option>(),

                // Extra Properties
                Industry = submissionDto.Industry,
                Situation = submissionDto.Situation,
                Order = submissionDto.Order,
                EstSignPctg = submissionDto.EstSignPctg,

            };

            #region wording
            var displayOrder = 0;
            submission.MarketWordingSettings = new List<MarketWordingSetting>();
            foreach (var marketWordingSetting in submissionDto.SubmissionMarketWordingsList ?? new List<MarketWordingSettingDto>())
            {
                displayOrder++;
                submission.MarketWordingSettings.Add(new MarketWordingSetting { Id = marketWordingSetting.SettingId, DisplayOrder = displayOrder, MarketWording = new MarketWording { Id = marketWordingSetting.Id } });

            }
            displayOrder = 0;
            submission.TermsNConditionWordingSettings = new List<TermsNConditionWordingSetting>();
            foreach (var termsNConditionWordingSetting in submissionDto.SubmissionTermsNConditionWordingsList ?? new List<TermsNConditionWordingSettingDto>())
            {
                displayOrder++;
                submission.TermsNConditionWordingSettings.Add(new TermsNConditionWordingSetting { Id = termsNConditionWordingSetting.SettingId, DisplayOrder = displayOrder, IsStrikeThrough = termsNConditionWordingSetting.IsStrikeThrough, TermsNConditionWording = new TermsNConditionWording { Id = termsNConditionWordingSetting.Id } });

            }
            displayOrder = 0;
            submission.SubjectToClauseWordingSettings = new List<SubjectToClauseWordingSetting>();
            foreach (var subjectToClauseWordingSetting in submissionDto.SubmissionSubjectToClauseWordingsList ?? new List<SubjectToClauseWordingSettingDto>())
            {
                displayOrder++;
                submission.SubjectToClauseWordingSettings.Add(new SubjectToClauseWordingSetting { Id = subjectToClauseWordingSetting.SettingId, DisplayOrder = displayOrder, IsStrikeThrough = subjectToClauseWordingSetting.IsStrikeThrough, SubjectToClauseWording = new SubjectToClauseWording { Id = subjectToClauseWordingSetting.Id } });

            }

            displayOrder = 0;
            submission.CustomMarketWordingSettings = new List<MarketWordingSetting>();
            foreach (var marketWordingSetting in submissionDto.CustomSubmissionMarketWordingsList ?? new List<MarketWordingSettingDto>())
            {
                displayOrder++;
                submission.CustomMarketWordingSettings.Add(new MarketWordingSetting { Id = marketWordingSetting.SettingId, DisplayOrder = displayOrder, MarketWording = new MarketWording { Id = 0, WordingRefNumber = marketWordingSetting.WordingRefNumber, Title = marketWordingSetting.Title, WordingType = "Custom" } });

            }
            displayOrder = 0;
            submission.CustomTermsNConditionWordingSettings = new List<TermsNConditionWordingSetting>();
            foreach (var termsNConditionWordingSetting in submissionDto.CustomSubmissionTermsNConditionWordingsList ?? new List<TermsNConditionWordingSettingDto>())
            {
                displayOrder++;
                submission.CustomTermsNConditionWordingSettings.Add(new TermsNConditionWordingSetting { Id = termsNConditionWordingSetting.SettingId, DisplayOrder = displayOrder, IsStrikeThrough = termsNConditionWordingSetting.IsStrikeThrough, TermsNConditionWording = new TermsNConditionWording { Id = 0, WordingRefNumber = termsNConditionWordingSetting.WordingRefNumber, Title = termsNConditionWordingSetting.Title, WordingType = "Custom" } });

            }
            displayOrder = 0;
            submission.CustomSubjectToClauseWordingSettings = new List<SubjectToClauseWordingSetting>();
            foreach (var subjectToClauseWordingSetting in submissionDto.CustomSubmissionSubjectToClauseWordingsList ?? new List<SubjectToClauseWordingSettingDto>())
            {
                displayOrder++;
                submission.CustomSubjectToClauseWordingSettings.Add(new SubjectToClauseWordingSetting { Id = subjectToClauseWordingSetting.SettingId, DisplayOrder = displayOrder, IsStrikeThrough = subjectToClauseWordingSetting.IsStrikeThrough, SubjectToClauseWording = new SubjectToClauseWording { Id = 0, Title = subjectToClauseWordingSetting.Title, WordingType = "Custom" } });

            }
            #endregion

            foreach (var optionDto in submissionDto.Options)
            {
                var option = new Option
                {
                    Id = optionDto.Id,
                    SubmissionId = optionDto.SubmissionId,
                    Title = optionDto.Title,
                    Timestamp = optionDto.Timestamp,
                    Comments = optionDto.Comments,
                    OptionVersions = new List<OptionVersion>()
                };

                foreach (var optionVersionDto in optionDto.OptionVersions)
                {
                    var optionVersion = new OptionVersionPV
                    {
                        OptionId = optionVersionDto.OptionId,
                        VersionNumber = optionVersionDto.VersionNumber,
                        Comments = optionVersionDto.Comments,
                        Title = optionVersionDto.Title,
                        IsExperiment = optionVersionDto.IsExperiment,
                        IsLocked = optionVersionDto.IsLocked,
                        Timestamp = optionVersionDto.Timestamp,
                        Quotes = new List<Quote>(),

                        TSICurrency = optionVersionDto.TSICurrency,
                        TSIPD = optionVersionDto.TSIPD,
                        TSIBI = optionVersionDto.TSIBI,
                        TSITotal = optionVersionDto.TSITotal,
                    };

                    foreach (var quoteDto in optionVersionDto.Quotes)
                    {
                        var quoteEnergy = new QuotePV
                        {
                            Id = quoteDto.Id,
                            OptionId = quoteDto.OptionId,
                            VersionNumber = quoteDto.VersionNumber,
                            CorrelationToken = quoteDto.CorrelationToken,
                            IsSubscribeMaster = quoteDto.IsSubscribeMaster,
                            CopiedFromQuoteId = quoteDto.CopiedFromQuoteId,
                            SubscribeReference = quoteDto.SubscribeReference,
                            RenPolId = quoteDto.RenPolId,
                            FacilityRef = quoteDto.FacilityRef,
                            SubmissionStatus = quoteDto.SubmissionStatus,
                            EntryStatus = quoteDto.EntryStatus,
                            PolicyType = quoteDto.PolicyType,
                            OriginatingOfficeId = quoteDto.OriginatingOfficeId,
                            COBId = quoteDto.COBId,
                            MOA = quoteDto.MOA,
                            AccountYear = quoteDto.AccountYear,
                            InceptionDate = quoteDto.InceptionDate,
                            ExpiryDate = quoteDto.ExpiryDate,
                            QuoteExpiryDate = quoteDto.QuoteExpiryDate,
                            TechnicalPricingMethod = quoteDto.TechnicalPricingMethod,
                            TechnicalPricingBindStatus = quoteDto.TechnicalPricingBindStatus,
                            TechnicalPricingPremiumPctgAmt = quoteDto.TechnicalPricingPremiumPctgAmt,
                            TechnicalPremium = quoteDto.TechnicalPremium,
                            Currency = quoteDto.Currency,
                            LimitCCY = quoteDto.LimitCCY,
                            ExcessCCY = quoteDto.ExcessCCY,
                            BenchmarkPremium = quoteDto.BenchmarkPremium,
                            QuotedPremium = Convert.ToDecimal(quoteDto.QuotedPremium),
                            LimitAmount = quoteDto.LimitAmount,
                            ExcessAmount = quoteDto.ExcessAmount,
                            Comment = quoteDto.Comment,
                            DeclinatureReason = quoteDto.DeclinatureReason,
                            DeclinatureComments = quoteDto.DeclinatureComments,
                            Timestamp = quoteDto.Timestamp,
                            SubscribeTimestamp = quoteDto.SubscribeTimestamp,
                            PDPctgAmt = quoteDto.PDPctgAmt,
                            PDExcessCurrency = quoteDto.PDExcessCurrency,
                            PDExcessAmount = quoteDto.PDExcessAmount,
                            BIPctgAmtDays = quoteDto.BIPctgAmtDays,
                            BIExcessCurrency = quoteDto.BIExcessCurrency,
                            BIExcessAmount = quoteDto.BIExcessAmount, 
                            LineSize = quoteDto.LineSize, 
                            LineToStand = quoteDto.LineToStand,
                        };

                        optionVersion.Quotes.Add(quoteEnergy);
                    }

                    option.OptionVersions.Add(optionVersion);
                }

                submission.Options.Add(option);
            }
            return submission;

        }

        // TODO: have one method using reflection
        public static SubmissionEN SetupENSubmission(SubmissionDto submissionDto)
        {
            var submission = new SubmissionEN
            {
                Id = submissionDto.Id,
                Title = submissionDto.Title,
                Description = submissionDto.Description,
                InsuredName = submissionDto.InsuredName,
                InsuredId = submissionDto.InsuredId,
                BrokerCode = submissionDto.BrokerCode,
                BrokerPseudonym = submissionDto.BrokerPseudonym,
                BrokerSequenceId = submissionDto.BrokerSequenceId,
                BrokerContact = submissionDto.BrokerContact,
                NonLondonBrokerCode = submissionDto.NonLondonBrokerCode,
                NonLondonBrokerName = submissionDto.NonLondonBrokerName,
                UnderwriterCode = submissionDto.UnderwriterCode,
                Underwriter = submissionDto.Underwriter,
                UnderwriterContactCode = submissionDto.UnderwriterContactCode,
                UnderwriterContact = submissionDto.UnderwriterContact,
                QuotingOfficeId = submissionDto.QuotingOfficeId,
                QuotingOffice = submissionDto.QuotingOffice,
                Domicile = submissionDto.Domicile,
                Leader = submissionDto.Leader,
                Brokerage = submissionDto.Brokerage,
                QuoteSheetNotes = submissionDto.QuoteSheetNotes,
                UnderwriterNotes = submissionDto.UnderwriterNotes,
                Timestamp = submissionDto.Timestamp,
                SubmissionTypeId = submissionDto.SubmissionTypeId,
                Options = new List<Option>(),
            };

            #region wording
            var displayOrder = 0;
            submission.MarketWordingSettings = new List<MarketWordingSetting>();
            foreach (var marketWordingSetting in submissionDto.SubmissionMarketWordingsList ?? new List<MarketWordingSettingDto>())
            {
                displayOrder++;
                submission.MarketWordingSettings.Add(new MarketWordingSetting { Id = marketWordingSetting.SettingId, DisplayOrder = displayOrder, MarketWording = new MarketWording { Id = marketWordingSetting.Id } });

            }
            displayOrder = 0;
            submission.TermsNConditionWordingSettings = new List<TermsNConditionWordingSetting>();
            foreach (var termsNConditionWordingSetting in submissionDto.SubmissionTermsNConditionWordingsList ?? new List<TermsNConditionWordingSettingDto>())
            {
                displayOrder++;
                submission.TermsNConditionWordingSettings.Add(new TermsNConditionWordingSetting { Id = termsNConditionWordingSetting.SettingId, DisplayOrder = displayOrder, IsStrikeThrough = termsNConditionWordingSetting.IsStrikeThrough, TermsNConditionWording = new TermsNConditionWording { Id = termsNConditionWordingSetting.Id } });

            }
            displayOrder = 0;
            submission.SubjectToClauseWordingSettings = new List<SubjectToClauseWordingSetting>();
            foreach (var subjectToClauseWordingSetting in submissionDto.SubmissionSubjectToClauseWordingsList ?? new List<SubjectToClauseWordingSettingDto>())
            {
                displayOrder++;
                submission.SubjectToClauseWordingSettings.Add(new SubjectToClauseWordingSetting { Id = subjectToClauseWordingSetting.SettingId, DisplayOrder = displayOrder, IsStrikeThrough = subjectToClauseWordingSetting.IsStrikeThrough, SubjectToClauseWording = new SubjectToClauseWording { Id = subjectToClauseWordingSetting.Id } });

            }

            displayOrder = 0;
            submission.CustomMarketWordingSettings = new List<MarketWordingSetting>();
            foreach (var marketWordingSetting in submissionDto.CustomSubmissionMarketWordingsList ?? new List<MarketWordingSettingDto>())
            {
                displayOrder++;
                submission.CustomMarketWordingSettings.Add(new MarketWordingSetting { Id = marketWordingSetting.SettingId, DisplayOrder = displayOrder, MarketWording = new MarketWording { Id = 0, WordingRefNumber = marketWordingSetting.WordingRefNumber, Title = marketWordingSetting.Title, WordingType = "Custom" } });

            }
            displayOrder = 0;
            submission.CustomTermsNConditionWordingSettings = new List<TermsNConditionWordingSetting>();
            foreach (var termsNConditionWordingSetting in submissionDto.CustomSubmissionTermsNConditionWordingsList ?? new List<TermsNConditionWordingSettingDto>())
            {
                displayOrder++;
                submission.CustomTermsNConditionWordingSettings.Add(new TermsNConditionWordingSetting { Id = termsNConditionWordingSetting.SettingId, DisplayOrder = displayOrder, IsStrikeThrough = termsNConditionWordingSetting.IsStrikeThrough, TermsNConditionWording = new TermsNConditionWording { Id = 0, WordingRefNumber = termsNConditionWordingSetting.WordingRefNumber, Title = termsNConditionWordingSetting.Title, WordingType = "Custom" } });

            }
            displayOrder = 0;
            submission.CustomSubjectToClauseWordingSettings = new List<SubjectToClauseWordingSetting>();
            foreach (var subjectToClauseWordingSetting in submissionDto.CustomSubmissionSubjectToClauseWordingsList ?? new List<SubjectToClauseWordingSettingDto>())
            {
                displayOrder++;
                submission.CustomSubjectToClauseWordingSettings.Add(new SubjectToClauseWordingSetting { Id = subjectToClauseWordingSetting.SettingId, DisplayOrder = displayOrder, IsStrikeThrough = subjectToClauseWordingSetting.IsStrikeThrough, SubjectToClauseWording = new SubjectToClauseWording { Id = 0, Title = subjectToClauseWordingSetting.Title, WordingType = "Custom" } });

            }
            #endregion

            foreach (var optionDto in submissionDto.Options)
            {
                var option = new Option
                {
                    Id = optionDto.Id,
                    SubmissionId = optionDto.SubmissionId,
                    Title = optionDto.Title,
                    Timestamp = optionDto.Timestamp,
                    Comments = optionDto.Comments,
                    OptionVersions = new List<OptionVersion>()
                };

                foreach (var optionVersionDto in optionDto.OptionVersions)
                {
                    var optionVersion = new OptionVersion
                    {
                        OptionId = optionVersionDto.OptionId,
                        VersionNumber = optionVersionDto.VersionNumber,
                        Comments = optionVersionDto.Comments,
                        Title = optionVersionDto.Title,
                        IsExperiment = optionVersionDto.IsExperiment,
                        IsLocked = optionVersionDto.IsLocked,
                        Timestamp = optionVersionDto.Timestamp,
                        Quotes = new List<Quote>()
                    };

                    foreach (var quoteDto in optionVersionDto.Quotes)
                    {
                        var quoteEnergy = new QuoteEN
                        {
                            Id = quoteDto.Id,
                            OptionId = quoteDto.OptionId,
                            VersionNumber = quoteDto.VersionNumber,
                            CorrelationToken = quoteDto.CorrelationToken,
                            IsSubscribeMaster = quoteDto.IsSubscribeMaster,
                            CopiedFromQuoteId = quoteDto.CopiedFromQuoteId,
                            SubscribeReference = quoteDto.SubscribeReference,
                            RenPolId = quoteDto.RenPolId,
                            FacilityRef = quoteDto.FacilityRef,
                            SubmissionStatus = quoteDto.SubmissionStatus,
                            EntryStatus = quoteDto.EntryStatus,
                            PolicyType = quoteDto.PolicyType,
                            OriginatingOfficeId = quoteDto.OriginatingOfficeId,
                            COBId = quoteDto.COBId,
                            MOA = quoteDto.MOA,
                            AccountYear = quoteDto.AccountYear,
                            InceptionDate = quoteDto.InceptionDate,
                            ExpiryDate = quoteDto.ExpiryDate,
                            QuoteExpiryDate = quoteDto.QuoteExpiryDate,
                            TechnicalPricingMethod = quoteDto.TechnicalPricingMethod,
                            TechnicalPricingBindStatus = quoteDto.TechnicalPricingBindStatus,
                            TechnicalPricingPremiumPctgAmt = quoteDto.TechnicalPricingPremiumPctgAmt,
                            TechnicalPremium = quoteDto.TechnicalPremium,
                            Currency = quoteDto.Currency,
                            LimitCCY = quoteDto.LimitCCY,
                            ExcessCCY = quoteDto.ExcessCCY,
                            BenchmarkPremium = quoteDto.BenchmarkPremium,
                            QuotedPremium = Convert.ToDecimal(quoteDto.QuotedPremium),
                            LimitAmount = quoteDto.LimitAmount,
                            ExcessAmount = quoteDto.ExcessAmount,
                            Comment = quoteDto.Comment,
                            DeclinatureReason = quoteDto.DeclinatureReason,
                            DeclinatureComments = quoteDto.DeclinatureComments,
                            Timestamp = quoteDto.Timestamp,
                            SubscribeTimestamp = quoteDto.SubscribeTimestamp,

                            // Extra Properties
							AmountOrOPL = quoteDto.AmountOrOPL,
							AmountOrONP = quoteDto.AmountOrONP,
							QuoteComments = quoteDto.QuoteComments
                        };

                        optionVersion.Quotes.Add(quoteEnergy);
                    }

                    option.OptionVersions.Add(optionVersion);
                }

                submission.Options.Add(option);
            }
            return submission;
        }

        //// TODO: SubmissionDto needs to be returned from Edit / Create Submission
        //public static SubmissionDto SetupPVSubmissionDto(SubmissionPV submission)
        //{
        //    return new SubmissionDto();
        //}

        //// TODO: SubmissionDto needs to be returned from Edit / Create Submission
        //public static SubmissionDto SetupENSubmission(SubmissionEN submission)
        //{
        //    return new SubmissionDto();
        //}

        public static Submission SetupFISubmission(SubmissionDto submissionDto)
        {
            var submission = new Submission
            {
                Id = submissionDto.Id,
                Title = submissionDto.Title,
                Description = submissionDto.Description,
                InsuredName = submissionDto.InsuredName,
                InsuredId = submissionDto.InsuredId,
                BrokerCode = submissionDto.BrokerCode,
                BrokerPseudonym = submissionDto.BrokerPseudonym,
                BrokerSequenceId = submissionDto.BrokerSequenceId,
                BrokerContact = submissionDto.BrokerContact,
                NonLondonBrokerCode = submissionDto.NonLondonBrokerCode,
                NonLondonBrokerName = submissionDto.NonLondonBrokerName,
                UnderwriterCode = submissionDto.UnderwriterCode,
                Underwriter = submissionDto.Underwriter,
                UnderwriterContactCode = submissionDto.UnderwriterContactCode,
                UnderwriterContact = submissionDto.UnderwriterContact,
                QuotingOfficeId = submissionDto.QuotingOfficeId,
                QuotingOffice = submissionDto.QuotingOffice,
                Domicile = submissionDto.Domicile,
                Leader = submissionDto.Leader,
                Brokerage = submissionDto.Brokerage,
                QuoteSheetNotes = submissionDto.QuoteSheetNotes,
                UnderwriterNotes = submissionDto.UnderwriterNotes,
                Timestamp = submissionDto.Timestamp,
                SubmissionTypeId = submissionDto.SubmissionTypeId,
                Options = new List<Option>(),
            };

            #region wording
            var displayOrder = 0;
            submission.MarketWordingSettings = new List<MarketWordingSetting>();
            foreach (var marketWordingSetting in submissionDto.SubmissionMarketWordingsList ?? new List<MarketWordingSettingDto>())
            {
                displayOrder++;
                submission.MarketWordingSettings.Add(new MarketWordingSetting { Id = marketWordingSetting.SettingId, DisplayOrder = displayOrder, MarketWording = new MarketWording { Id = marketWordingSetting.Id } });

            }
            displayOrder = 0;
            submission.TermsNConditionWordingSettings = new List<TermsNConditionWordingSetting>();
            foreach (var termsNConditionWordingSetting in submissionDto.SubmissionTermsNConditionWordingsList ?? new List<TermsNConditionWordingSettingDto>())
            {
                displayOrder++;
                submission.TermsNConditionWordingSettings.Add(new TermsNConditionWordingSetting { Id = termsNConditionWordingSetting.SettingId, DisplayOrder = displayOrder, IsStrikeThrough = termsNConditionWordingSetting.IsStrikeThrough, TermsNConditionWording = new TermsNConditionWording { Id = termsNConditionWordingSetting.Id } });

            }
            displayOrder = 0;
            submission.SubjectToClauseWordingSettings = new List<SubjectToClauseWordingSetting>();
            foreach (var subjectToClauseWordingSetting in submissionDto.SubmissionSubjectToClauseWordingsList ?? new List<SubjectToClauseWordingSettingDto>())
            {
                displayOrder++;
                submission.SubjectToClauseWordingSettings.Add(new SubjectToClauseWordingSetting { Id = subjectToClauseWordingSetting.SettingId, DisplayOrder = displayOrder, IsStrikeThrough = subjectToClauseWordingSetting.IsStrikeThrough, SubjectToClauseWording = new SubjectToClauseWording { Id = subjectToClauseWordingSetting.Id } });

            }

            displayOrder = 0;
            submission.CustomMarketWordingSettings = new List<MarketWordingSetting>();
            foreach (var marketWordingSetting in submissionDto.CustomSubmissionMarketWordingsList ?? new List<MarketWordingSettingDto>())
            {
                displayOrder++;
                submission.CustomMarketWordingSettings.Add(new MarketWordingSetting { Id = marketWordingSetting.SettingId, DisplayOrder = displayOrder, MarketWording = new MarketWording { Id = 0, WordingRefNumber = marketWordingSetting.WordingRefNumber, Title = marketWordingSetting.Title, WordingType = "Custom" } });

            }
            displayOrder = 0;
            submission.CustomTermsNConditionWordingSettings = new List<TermsNConditionWordingSetting>();
            foreach (var termsNConditionWordingSetting in submissionDto.CustomSubmissionTermsNConditionWordingsList ?? new List<TermsNConditionWordingSettingDto>())
            {
                displayOrder++;
                submission.CustomTermsNConditionWordingSettings.Add(new TermsNConditionWordingSetting { Id = termsNConditionWordingSetting.SettingId, DisplayOrder = displayOrder, IsStrikeThrough = termsNConditionWordingSetting.IsStrikeThrough, TermsNConditionWording = new TermsNConditionWording { Id = 0, WordingRefNumber = termsNConditionWordingSetting.WordingRefNumber, Title = termsNConditionWordingSetting.Title, WordingType = "Custom" } });

            }
            displayOrder = 0;
            submission.CustomSubjectToClauseWordingSettings = new List<SubjectToClauseWordingSetting>();
            foreach (var subjectToClauseWordingSetting in submissionDto.CustomSubmissionSubjectToClauseWordingsList ?? new List<SubjectToClauseWordingSettingDto>())
            {
                displayOrder++;
                submission.CustomSubjectToClauseWordingSettings.Add(new SubjectToClauseWordingSetting { Id = subjectToClauseWordingSetting.SettingId, DisplayOrder = displayOrder, IsStrikeThrough = subjectToClauseWordingSetting.IsStrikeThrough, SubjectToClauseWording = new SubjectToClauseWording { Id = 0, Title = subjectToClauseWordingSetting.Title, WordingType = "Custom" } });

            }
            #endregion

            foreach (var optionDto in submissionDto.Options)
            {
                var option = new OptionFI
                {
                    Id = optionDto.Id,
                    SubmissionId = optionDto.SubmissionId,
                    Title = optionDto.Title,
                    Timestamp = optionDto.Timestamp,
                    Comments = optionDto.Comments,
                    RiskCodes = optionDto.RiskCodes,
                    OptionVersions = new List<OptionVersion>()
                };

                foreach (var optionVersionDto in optionDto.OptionVersions)
                {
                    var optionVersion = new OptionVersion
                    {
                        OptionId = optionVersionDto.OptionId,
                        VersionNumber = optionVersionDto.VersionNumber,
                        Comments = optionVersionDto.Comments,
                        Title = optionVersionDto.Title,
                        IsExperiment = optionVersionDto.IsExperiment,
                        IsLocked = optionVersionDto.IsLocked,
                        Timestamp = optionVersionDto.Timestamp,
                        Quotes = new List<Quote>()
                    };

                    foreach (var quoteDto in optionVersionDto.Quotes)
                    {
                        var quoteEnergy = new QuoteFI
                        {
                            Id = quoteDto.Id,
                            OptionId = quoteDto.OptionId,
                            VersionNumber = quoteDto.VersionNumber,
                            CorrelationToken = quoteDto.CorrelationToken,
                            IsSubscribeMaster = quoteDto.IsSubscribeMaster,
                            CopiedFromQuoteId = quoteDto.CopiedFromQuoteId,
                            SubscribeReference = quoteDto.SubscribeReference,
                            RenPolId = quoteDto.RenPolId,
                            FacilityRef = quoteDto.FacilityRef,
                            SubmissionStatus = quoteDto.SubmissionStatus,
                            EntryStatus = quoteDto.EntryStatus,
                            PolicyType = quoteDto.PolicyType,
                            OriginatingOfficeId = quoteDto.OriginatingOfficeId,
                            COBId = quoteDto.COBId,
                            MOA = quoteDto.MOA,
                            AccountYear = quoteDto.AccountYear,
                            InceptionDate = quoteDto.InceptionDate,
                            ExpiryDate = quoteDto.ExpiryDate,
                            QuoteExpiryDate = quoteDto.QuoteExpiryDate,
                            TechnicalPricingMethod = quoteDto.TechnicalPricingMethod,
                            TechnicalPricingBindStatus = quoteDto.TechnicalPricingBindStatus,
                            TechnicalPricingPremiumPctgAmt = quoteDto.TechnicalPricingPremiumPctgAmt,
                            TechnicalPremium = quoteDto.TechnicalPremium,
                            Currency = quoteDto.Currency,
                            LimitCCY = quoteDto.LimitCCY,
                            ExcessCCY = quoteDto.ExcessCCY,
                            BenchmarkPremium = quoteDto.BenchmarkPremium,
                            QuotedPremium = Convert.ToDecimal(quoteDto.QuotedPremium),
                            LimitAmount = quoteDto.LimitAmount,
                            ExcessAmount = quoteDto.ExcessAmount,
                            Comment = quoteDto.Comment,
                            DeclinatureReason = quoteDto.DeclinatureReason,
                            DeclinatureComments = quoteDto.DeclinatureComments,
                            Timestamp = quoteDto.Timestamp,
                            SubscribeTimestamp = quoteDto.SubscribeTimestamp,

                            // Extra Properties
							AmountOrOPL = quoteDto.AmountOrOPL,
							AmountOrONP = quoteDto.AmountOrONP,
                            RiskCodeId = quoteDto.RiskCodeId,
                            LineSize = quoteDto.LineSize,
                            LineToStand = quoteDto.LineToStand,
                            IsReinstatement = quoteDto.IsReinstatement,
                        };

                        optionVersion.Quotes.Add(quoteEnergy);
                    }

                    option.OptionVersions.Add(optionVersion);
                }

                submission.Options.Add(option);
            }
            return submission;
        }

        public static Submission SetupWording(Submission submission)
        {
            #region wording
            var displayOrder = 0;
            submission.MarketWordingSettings = new List<MarketWordingSetting>();
            if (submission.SubmissionMarketWordingsList != null)
                foreach (var marketWordingSetting in submission.SubmissionMarketWordingsList)
                {
                    displayOrder++;
                    submission.MarketWordingSettings.Add(new MarketWordingSetting { Id = marketWordingSetting.SettingId, DisplayOrder = displayOrder, MarketWording = new MarketWording { Id = marketWordingSetting.Id } });

                }
            displayOrder = 0;
            submission.TermsNConditionWordingSettings = new List<TermsNConditionWordingSetting>();
            if (submission.SubmissionTermsNConditionWordingsList != null)
                foreach (var termsNConditionWordingSetting in submission.SubmissionTermsNConditionWordingsList )
                {
                    displayOrder++;
                    submission.TermsNConditionWordingSettings.Add(new TermsNConditionWordingSetting { Id = termsNConditionWordingSetting.SettingId, DisplayOrder = displayOrder, IsStrikeThrough = termsNConditionWordingSetting.IsStrikeThrough, TermsNConditionWording = new TermsNConditionWording { Id = termsNConditionWordingSetting.Id } });

                }
            displayOrder = 0;
            submission.SubjectToClauseWordingSettings = new List<SubjectToClauseWordingSetting>();
            if (submission.SubmissionSubjectToClauseWordingsList != null)
                foreach (var subjectToClauseWordingSetting in submission.SubmissionSubjectToClauseWordingsList)
                {
                    displayOrder++;
                    submission.SubjectToClauseWordingSettings.Add(new SubjectToClauseWordingSetting { Id = subjectToClauseWordingSetting.SettingId, DisplayOrder = displayOrder, IsStrikeThrough = subjectToClauseWordingSetting.IsStrikeThrough, SubjectToClauseWording = new SubjectToClauseWording { Id = subjectToClauseWordingSetting.Id } });

                }

            displayOrder = 0;
            submission.CustomMarketWordingSettings = new List<MarketWordingSetting>();
            if (submission.CustomSubmissionMarketWordingsList != null)
                foreach (var marketWordingSetting in submission.CustomSubmissionMarketWordingsList )
                {
                    displayOrder++;
                    submission.CustomMarketWordingSettings.Add(new MarketWordingSetting { Id = marketWordingSetting.SettingId, DisplayOrder = displayOrder, MarketWording = new MarketWording { Id = 0, WordingRefNumber = marketWordingSetting.WordingRefNumber, Title = marketWordingSetting.Title, WordingType = "Custom" } });

                }
            displayOrder = 0;
            submission.CustomTermsNConditionWordingSettings = new List<TermsNConditionWordingSetting>();
            if (submission.CustomSubmissionTermsNConditionWordingsList != null)
                foreach (var termsNConditionWordingSetting in submission.CustomSubmissionTermsNConditionWordingsList )
                {
                    displayOrder++;
                    submission.CustomTermsNConditionWordingSettings.Add(new TermsNConditionWordingSetting { Id = termsNConditionWordingSetting.SettingId, DisplayOrder = displayOrder, IsStrikeThrough = termsNConditionWordingSetting.IsStrikeThrough, TermsNConditionWording = new TermsNConditionWording { Id = 0, WordingRefNumber = termsNConditionWordingSetting.WordingRefNumber, Title = termsNConditionWordingSetting.Title, WordingType = "Custom" } });

                }
            displayOrder = 0;
            submission.CustomSubjectToClauseWordingSettings = new List<SubjectToClauseWordingSetting>();
            if (submission.CustomSubmissionSubjectToClauseWordingsList != null)
                foreach (var subjectToClauseWordingSetting in submission.CustomSubmissionSubjectToClauseWordingsList )
                {
                    displayOrder++;
                    submission.CustomSubjectToClauseWordingSettings.Add(new SubjectToClauseWordingSetting { Id = subjectToClauseWordingSetting.SettingId, DisplayOrder = displayOrder, IsStrikeThrough = subjectToClauseWordingSetting.IsStrikeThrough, SubjectToClauseWording = new SubjectToClauseWording { Id = 0, Title = subjectToClauseWordingSetting.Title, WordingType = "Custom" } });

                }

            #endregion

            return submission;
        }
    }
}