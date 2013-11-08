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
                VersionNumber = q.VersionNumber,
                Description = q.Description,

                RenewalRate = q.RenewalRate,
                RenewalConditions = q.RenewalConditions,
                RenewalDeductibles = q.RenewalDeductibles,
                RenewalExposure = q.RenewalExposure,
                RenewalBase = q.RenewalBase,
                RenewalFull = q.RenewalFull
            };
        }

        public static UpdatePolicyResponse UpdateSubscribeRecord(Quote q, Submission s, ILogHandler logHandler, IPolicyService subsribeService)
        {
            var insuredList = new InsuredCollection();
            if (s.AdditionalInsuredList != null)
            {
                insuredList.AddRange(s.AdditionalInsuredList
                                      .Select(insured => new Insured
                                      {
                                          AddlInsdId = insured.InsuredId,
                                          AddlInsdNm = insured.InsuredId == 0 ? insured.InsuredName : null,
                                          AddlInsdTy = insured.InsuredType
                                      }));
            }

            var policyContract = new PolicyContract
            {
                AccYr = q.AccountYear.ToString(),
                BenchPrm = q.BenchmarkPremium,
                BindSt = q.TechnicalPricingBindStatus,
                BkrNo = "???", // TODO - not referenced in CreateQuote sp
                BkrPsu = s.BrokerPseudonym, // No validation carried out in service validation sp
                BkrSeqId = s.BrokerSequenceId,
                Brokerage = s.Brokerage,
                COB = q.COBId,
                DeclineReason = q.DeclinatureReason,
                DOM = s.Domicile,
                EntSt = q.EntryStatus,
                ExpyDt = (q.ExpiryDate.HasValue) ? q.ExpiryDate.Value.ToString("yyyyMMdd") : null,
                ExsAmt = q.ExcessAmount,
                FacyPolId = q.FacilityRef,
                LdrNo = s.LeaderNo,
                Ldr = s.Leader,
                IncpDt = (q.InceptionDate.HasValue) ? q.InceptionDate.Value.ToString("yyyyMMdd") : null,
                InsdId = s.InsuredId,
                InsdNm = s.InsuredName,
                AddlInsuredCollection = insuredList,
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
                SettDueDt = q.QuoteExpiryDate.ToString("yyyyMMdd"),
                TechPmTy = "TechPm", // TODO - not referenced in CreateQuote sp
                PolDsc = q.Description,

                //  These are the fields required for edit only
                TimeStamp = q.SubscribeTimestamp,
                PolId = q.SubscribeReference,

                CtcNm = (string.IsNullOrEmpty(s.BrokerContact)) ? s.NewBrokerContactName : s.BrokerContact,
                CtcTelNo = s.NewBrokerContactPhoneNumber,
                CtcEmail = s.NewBrokerContactEmail
            };

            if (q.RenewalRate.HasValue) policyContract.RenewalRate = q.RenewalRate;
            if (q.RenewalConditions.HasValue) policyContract.RenewalConditions = q.RenewalConditions;
            if (q.RenewalDeductibles.HasValue) policyContract.RenewalDeductibles = q.RenewalDeductibles;
            if (q.RenewalExposure.HasValue) policyContract.RenewalExposurePctg = q.RenewalExposure;
            if (q.RenewalBase.HasValue) policyContract.RenewalBase = q.RenewalBase;
            if (q.RenewalFull.HasValue) policyContract.RenewalFull = q.RenewalFull;

            var updateResp = subsribeService.UpdatePolicy(new UpdatePolicyRequest
            {
                objPolicyContract = policyContract
            });
          
            if (updateResp.UpdatePolicyResult.ErrorInfo == null)
            {
                
                    var doc = new XmlDocument();

                    doc.LoadXml(updateResp.UpdatePolicyResult.OutputXml);

                    q.SubscribeTimestamp = long.Parse(doc.GetElementsByTagName("TimeStamp")[0].InnerText);

                    q.FacilityRef = string.IsNullOrEmpty(doc.GetElementsByTagName("NewFacyPolId")[0].InnerText)
                                        ? q.FacilityRef
                                        : doc.GetElementsByTagName("NewFacyPolId")[0].InnerText;
               
               
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
            var insuredList = new InsuredCollection();
            if (s.AdditionalInsuredList != null)
            {
                insuredList.AddRange(s.AdditionalInsuredList
                                      .Select(insured => new Insured
                                          {
                                              AddlInsdId = insured.InsuredId,
                                              AddlInsdNm = insured.InsuredId == 0 ? insured.InsuredName : null,
                                              AddlInsdTy = insured.InsuredType
                                          }));
            }

            var policyContract = new PolicyContract
                {
                    AccYr = q.AccountYear.ToString(),
                    BenchPrm = q.BenchmarkPremium,
                    BindSt = q.TechnicalPricingBindStatus,
                    BkrNo = "???", // TODO - not referenced in CreateQuote sp
                    BkrPsu = s.BrokerPseudonym, // No validation carried out in service validation sp
                    BkrSeqId = s.BrokerSequenceId,
                    Brokerage = s.Brokerage,
                    COB = q.COBId,
                    DeclineReason = q.DeclinatureReason,
                    DOM = s.Domicile,
                    EntSt = q.EntryStatus,
                    ExpyDt = (q.ExpiryDate.HasValue) ? q.ExpiryDate.Value.ToString("yyyyMMdd") : null,
                    ExsAmt = q.ExcessAmount,
                    FacyPolId = q.FacilityRef,
                    LdrNo = s.LeaderNo,
                    Ldr = s.Leader,
                    IncpDt = (q.InceptionDate.HasValue) ? q.InceptionDate.Value.ToString("yyyyMMdd") : null,
                    InsdId = s.InsuredId,
                    InsdNm = s.InsuredName,
                    AddlInsuredCollection = insuredList,
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
                    SettDueDt = q.QuoteExpiryDate.ToString("yyyyMMdd"),
                    TechPmTy = "TechPm", // TODO - not referenced in CreateQuote sp
                    PolDsc = q.Description,

                    //  These are the fields required for edit only
                    TimeStamp = q.SubscribeTimestamp,
                    PolId = q.SubscribeReference,

                    CtcNm = (string.IsNullOrEmpty(s.BrokerContact)) ? s.NewBrokerContactName : s.BrokerContact,
                    CtcTelNo = s.NewBrokerContactPhoneNumber,
                    CtcEmail = s.NewBrokerContactEmail
                };

            if (q.RenewalRate.HasValue) policyContract.RenewalRate = q.RenewalRate;
            if (q.RenewalConditions.HasValue) policyContract.RenewalConditions = q.RenewalConditions;
            if (q.RenewalDeductibles.HasValue) policyContract.RenewalDeductibles = q.RenewalDeductibles;
            if (q.RenewalExposure.HasValue) policyContract.RenewalExposurePctg = q.RenewalExposure;
            if (q.RenewalBase.HasValue) policyContract.RenewalBase = q.RenewalBase;
            if (q.RenewalFull.HasValue) policyContract.RenewalFull = q.RenewalFull;

            var createQuoteResponse = subsribeService.CreateQuote(new CreateQuoteRequest
                {
                    objPolicyContract = policyContract
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
            submission.LeaderNo = polAsSubscribe.LdrNo;
            submission.Leader = polAsSubscribe.Ldr;

	        if (polAsSubscribe.AddlInsuredCollection != null)
	        {
	            submission.AdditionalInsuredList = new List<AdditionalInsured>
	                (
	                polAsSubscribe.AddlInsuredCollection
                                  .Where(i => i.AddlInsdTy != "Insured")
	                              .Select(i => new AdditionalInsured
	                                  {
	                                      InsuredId = i.AddlInsdId,
	                                      InsuredName = i.AddlInsdNm,
                                          InsuredType = i.AddlInsdTy
	                                  })
	                );
	        }

	        // TODO: values missing from service response...
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
	        q.Description = polAsSubscribe.PolDsc;
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

        public static Submission RemoveDeletedItems(Submission submission)
        {
            //Todo: this code has to be removed when bug related to soft delete is fixed.
            var removeOptions = submission.Options.Where(o => o.IsDeleted).ToList();
            foreach (var removeOption in removeOptions)
            {
                submission.Options.Remove(removeOption);
            }

            foreach (var activeOption in submission.Options)
            {
                var removeOptionVersions = activeOption.OptionVersions.Where(ov => ov.IsDeleted).ToList();
                foreach (var removeOptionVersion in removeOptionVersions)
                {
                    activeOption.OptionVersions.Remove(removeOptionVersion);
                }
                foreach (var activeOptionVersion in activeOption.OptionVersions)
                {
                    var removeQuotes = activeOptionVersion.Quotes.Where(q => q.IsDeleted).ToList();
                    foreach (var removeQuote in removeQuotes)
                    {
                        activeOptionVersion.Quotes.Remove(removeQuote);
                    }
                }
            }
            return submission;
        }
    }
}