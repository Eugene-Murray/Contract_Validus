using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Validus.Console.Data;
using Validus.Console.DTO;
using Validus.Console.SubscribeService;
using Validus.Core.HttpContext;
using Validus.Core.LogHandling;
using Validus.Models;

namespace Validus.Console.BusinessLogic
{
	public class SubmissionModule : ISubmissionModule
	{
	    private readonly IConsoleRepository _repository;
        private readonly IPolicyService _subsribeService;
        private readonly ILogHandler _logHandler;
        private readonly IWebSiteModuleManager _webSiteModuleManager;
        private readonly IPolicyData _policyData;

		public SubmissionModule(IConsoleRepository rep, IPolicyService subscribeService, ILogHandler logHandler, ICurrentHttpContext currentHttpContext, IWebSiteModuleManager webSiteModuleManager, IPolicyData policyData)
		{
			_repository = rep;
			_subsribeService = subscribeService;
			_logHandler = logHandler;
		    _webSiteModuleManager = webSiteModuleManager;
		    _policyData = policyData;
		}

        public Submission GetSubmissionById(int id) 
        {
            var submission =
                _repository.Query<Submission>(u => u.Id == id,
                                              s => s.Options.Select(o => o.OptionVersions.Select(ov => ov.Quotes)),
                                              s => s.MarketWordingSettings.Select(mws => mws.MarketWording),
                                              s => s.TermsNConditionWordingSettings.Select(tncs => tncs.TermsNConditionWording),
                                              s => s.SubjectToClauseWordingSettings.Select(stcs => stcs.SubjectToClauseWording),
                                              s => s.CustomMarketWordingSettings.Select(mws => mws.MarketWording),
                                              s => s.CustomTermsNConditionWordingSettings.Select(tncs => tncs.TermsNConditionWording),
                                              s => s.CustomSubjectToClauseWordingSettings.Select(stcs => stcs.SubjectToClauseWording)
                                              )
                           .FirstOrDefault();


            if (submission != null)
            {
                foreach (var quote in submission.Options
                                                .SelectMany(o => o.OptionVersions)
                                                .SelectMany(ov => ov.Quotes)
                                                .Where(q => (!string.IsNullOrEmpty(q.SubscribeReference)
                                                             && q.IsSubscribeMaster)))
                {
                    var response = _subsribeService.GetReference(new GetReferenceRequest
                    {
                        strPolId = quote.SubscribeReference
                    });

                    var errorInfo = response.GetReferenceResult.ErrorInfo;
                    var outputXml = response.GetReferenceResult.OutputXml;

                    if (errorInfo == null)
                    {
                        var policyContract = Utility.XmlDeserializeFromString<PolicyContract>(outputXml);

						SubmissionModuleHelpers.SynchroniseSubmission(submission, policyContract);

                        if (!quote.SubscribeTimestamp.HasValue
                            || policyContract.TimeStamp != quote.SubscribeTimestamp)
                        {
                            SubmissionModuleHelpers.SynchroniseQuote(quote, policyContract);
                        }
                    }
                }
                submission.MarketWordingSettings = submission.MarketWordingSettings.OrderBy(mw => mw.DisplayOrder).ToList();
                submission.TermsNConditionWordingSettings = submission.TermsNConditionWordingSettings.OrderBy(tncs => tncs.DisplayOrder).ToList();
                submission.SubjectToClauseWordingSettings = submission.SubjectToClauseWordingSettings.OrderBy(stcs => stcs.DisplayOrder).ToList();
                submission.CustomMarketWordingSettings = submission.CustomMarketWordingSettings.OrderBy(mw => mw.DisplayOrder).ToList();
                submission.CustomTermsNConditionWordingSettings = submission.CustomTermsNConditionWordingSettings.OrderBy(tncs => tncs.DisplayOrder).ToList();
                submission.CustomSubjectToClauseWordingSettings = submission.CustomSubjectToClauseWordingSettings.OrderBy(stcs => stcs.DisplayOrder).ToList();
            }

            return submission;
        }

        public T CreateSubmission<T>(T submissionT, out List<String> errorMessages) where T : Submission
        {
            errorMessages = new List<String>();

            var mustSave = false;

            //  IsSubscibeMaster will come from the request
            //  Copied from Quote needs to be set for each non-master.
            //  Do masters first, so their Subscribe references can be copied to the non-masters.
            foreach (var quote in submissionT.Options.SelectMany(o => o.OptionVersions)
                                            .SelectMany(ov => ov.Quotes)
                                            .OrderByDescending(q => q.IsSubscribeMaster))
            {
                try
                {
                    if (quote.IsSubscribeMaster) // Update Subscribe
                    {
                        SubmissionModuleHelpers.CreateSubscribeRecord(quote, submissionT, _logHandler, _subsribeService);
                        _policyData.RemovePolicyFromCache(quote.RenPolId);
                        mustSave = true;
                    }
                    else // No Subscribe update needed for these ones, just relate to masters
                    {
                        SubmissionModuleHelpers.SetCopiedFrom(quote, submissionT);
                    }
                }
                catch (Exception ex)
                {
                    _logHandler.WriteLog(ex.ToString(), LogSeverity.Error, LogCategory.BusinessComponent);

                    errorMessages.Add(ex.Message);
                }
            }

            foreach (var marketWordingSetting in submissionT.MarketWordingSettings)
            {
                var setting = marketWordingSetting;
                marketWordingSetting.MarketWording =
                    _repository.Query<MarketWording>(mw => mw.Id == setting.MarketWording.Id).First();
            }

            foreach (var termsNConditionWordingSetting in submissionT.TermsNConditionWordingSettings)
            {
                var setting = termsNConditionWordingSetting;
                termsNConditionWordingSetting.TermsNConditionWording =
                    _repository.Query<TermsNConditionWording>(tncw => tncw.Id == setting.TermsNConditionWording.Id).First();
            }

            foreach (var subjectToClauseWordingSetting in submissionT.SubjectToClauseWordingSettings)
            {
                var setting = subjectToClauseWordingSetting;
                subjectToClauseWordingSetting.SubjectToClauseWording =
                    _repository.Query<SubjectToClauseWording>(tncw => tncw.Id == setting.SubjectToClauseWording.Id).First();
            }

			//Custom TODO: Are these to be implemented ?
			//foreach (var marketWordingSetting in submissionT.CustomMarketWordingSettings)
			//{ }

			//foreach (var termsNConditionWordingSetting in submissionT.TermsNConditionWordingSettings)
			//{ }
             
			//foreach (var subjectToClauseWordingSetting in submissionT.SubjectToClauseWordingSettings)
			//{ }
             
            if (errorMessages.Count == 0 || mustSave)
            {
                _repository.Add(submissionT);
                _repository.SaveChanges();

                return submissionT;
            }

            submissionT.Id = -1;
            return submissionT;
        }

		public T UpdateSubmission<T>(T submissionT, out List<String> errorMessages, out List<Quote> userValues)
			where T : Submission
        {
            var canSave = true; //  True if no concurrency problems
            var mustSave = false; //  Becomes true once the operation has created a Subscribe reference

            errorMessages = new List<String>();
            userValues = new List<Quote>();

            //  Do all Subscribe concurrency checks first, then create the necessary Subscribe references if no problems. 
            //  Do the Subscribe concurrency check for all quotes with IsSubscribeMaster == true and which have a reference already.
            //  Newly added ones would have IsSubscribeMaster but have no reference
            //  Don't worry about concurrency for deletions (Id == -1)
            foreach (var quote in submissionT.Options
                                            .SelectMany(o => o.OptionVersions)
                                            .SelectMany(ov => ov.Quotes)
			                                 .Where(
				                                 q =>
				                                 !String.IsNullOrEmpty(q.SubscribeReference) && q.IsSubscribeMaster && q.Id != -1))
            {
                var getRefResp = _subsribeService.GetReference(new GetReferenceRequest
                {
                    strPolId = quote.SubscribeReference
                });

                if (getRefResp.GetReferenceResult.ErrorInfo == null)
                {
                    var polAsSubscribe = Utility.XmlDeserializeFromString<PolicyContract>(getRefResp.GetReferenceResult.OutputXml);

                    if (polAsSubscribe.TimeStamp != quote.SubscribeTimestamp)
                    {
                        //  Timestamps are different - data may be same because of overnight updates of unrelated fields in Subscribe.
                        var valuesEqual = SubmissionModuleHelpers.QuoteValuesMatchSubscribePolicy(polAsSubscribe, quote, submissionT);

                        if (!valuesEqual)
                        {
                            //  Data different - need to display concurrency errors to user.
                            canSave = false;

                            errorMessages.Add(String.Format("Subscribe data has been updated independently for {0}.",
                                                     quote.SubscribeReference));

                            userValues.Add(SubmissionModuleHelpers.CopyQuote(quote));
                        }
                        //  else... values same - no point updating, just sync timestamp on our side.                                                    
                    }
                    //  else... timestamps match, will try update if values different or skip if same.
                }
                else
                {
                    canSave = false;

                    errorMessages.Add(getRefResp.GetReferenceResult.ErrorInfo.Description);
                }
            }

            if (canSave)
            {
                try
                {
                    //  Actually do the updates to Subscribe if no concurrency problems are detected.

                    //  Loop every quote this time. Do masters first so their Subscribe references are available 
                    //  to copy to non-masters
                    foreach (var quote in submissionT.Options
                                                    .SelectMany(o => o.OptionVersions)
                                                    .SelectMany(ov => ov.Quotes)
                                                    .OrderByDescending(q => q.IsSubscribeMaster && q.Id != -1))
                    {
                        if (quote.IsSubscribeMaster)
                        {
                            if (!String.IsNullOrEmpty(quote.SubscribeReference)) //  Already has a Subscribe record.
                            {
                                var getRefResp = _subsribeService.GetReference(new GetReferenceRequest()
                                {
                                    strPolId = quote.SubscribeReference
                                });

                                //  TODO: "if (getRefResp.GetReferenceResult.ErrorInfo == null)" ?

                                var polAsSubscribe = Utility.XmlDeserializeFromString<PolicyContract>(getRefResp.GetReferenceResult.OutputXml);

                                var valuesEqual = SubmissionModuleHelpers.QuoteValuesMatchSubscribePolicy(polAsSubscribe, quote, submissionT);

                                if (polAsSubscribe.TimeStamp == quote.SubscribeTimestamp && !valuesEqual)
                                {
                                    //  Timestamps same, but values are not equal (we've just changed them in the UI) 
                                    //  TODO: Confirm if the service checks this (?)
                                    SubmissionModuleHelpers.UpdateSubscribeRecord(quote, submissionT, _logHandler, _subsribeService);
                                }
                                else if (valuesEqual)
                                {
                                    //  Timestamps are different, perhaps due to overnight updates of unrelated fields in Subscribe.
                                    //  Same - no point updating, just reset sync so our timestamp matches Subscribe
                                    quote.SubscribeTimestamp = polAsSubscribe.TimeStamp;
                                }
                                else
                                {
                                    //  This wont work as timestamps are different!
                                    SubmissionModuleHelpers.UpdateSubscribeRecord(quote, submissionT, _logHandler, _subsribeService);
                                }
                            }
                            else //  Got no Subscribe reference, so need to create record in Subscribe rather than update.
                            {
                                //  Need to create Subscribe reference.
                                SubmissionModuleHelpers.CreateSubscribeRecord(quote, submissionT, _logHandler, _subsribeService);

                                mustSave = true;
                            }
                        }
                        else //  Not Subscribe master, need to set copied from related master quote
                        {
                            SubmissionModuleHelpers.SetCopiedFrom(quote, submissionT);
                        }
                    }
                }
                catch (Exception ex)
                {
                    errorMessages.Add(ex.Message);
                }
                finally
                {
                    if (errorMessages.Count == 0 || mustSave)
                    {
                        var osToDelete = new List<Option>();

                        foreach (var o in submissionT.Options)
                        {
                            var ovsToDelete = new List<OptionVersion>();

                            foreach (var ov in o.OptionVersions)
                            {
                                var qsToDelete = new List<Quote>();

                                foreach (var q in ov.Quotes)
                                {
                                    if (q.Id == 0)
                                    {
                                        _repository.Add(q);
                                    }
                                    else if (q.Id > 0)
                                    {
                                        _repository.Attach(q);
                                    }
                                    else
                                    {
                                        q.Id = -q.Id;

                                        qsToDelete.Add(q);
                                    }
                                }

                                if (ov.OptionId == 0)
                                {
                                    ov.OptionId = o.Id;

                                    _repository.Add(ov);
                                }
                                else if (ov.OptionId > 0)
                                {
                                    _repository.Attach(ov);
                                }
                                else
                                {
                                    ov.OptionId = -ov.OptionId;

                                    ovsToDelete.Add(ov);
                                }

                                foreach (var qToDelete in qsToDelete)
                                {
                                    _repository.Delete(qToDelete);
                                }
                            }

                            if (o.Id == 0)
                            {
                                _repository.Add(o);
                            }
                            else if (o.Id > 0)
                            {
                                _repository.Attach(o);
                            }
                            else
                            {
                                o.Id = -o.Id;

                                osToDelete.Add(o);
                            }

                            foreach (var ovToDelete in ovsToDelete)
                            {
                                _repository.Delete(ovToDelete);
                            }
                        }

                        foreach (var oToDelete in osToDelete)
                        {
                            _repository.Delete(oToDelete);
                        }
                        //copy collection in temp

                        #region Wording

                        var _MarketWordingSettings = submissionT.MarketWordingSettings;
                        submissionT.MarketWordingSettings = null;
                        var _TermsNConditionWordingSettings = submissionT.TermsNConditionWordingSettings;
                        submissionT.TermsNConditionWordingSettings = null;
                        var _SubjectToClauseWordingSettings = submissionT.SubjectToClauseWordingSettings;
                        submissionT.SubjectToClauseWordingSettings = null;
                        var _CustomMarketWordingSettings = submissionT.CustomMarketWordingSettings;
                        submissionT.CustomMarketWordingSettings = null;
                        var _CustomTermsNConditionWordingSettings = submissionT.CustomTermsNConditionWordingSettings;
                        submissionT.CustomTermsNConditionWordingSettings = null;
                        var _CustomSubjectToClauseWordingSettings = submissionT.CustomSubjectToClauseWordingSettings;
                        submissionT.CustomSubjectToClauseWordingSettings = null;

                        _repository.Attach(submissionT);
                        _repository.Query<T>(s => s.Id == submissionT.Id,
                                                      s => s.MarketWordingSettings.Select(mws => mws.MarketWording),
                                                      s =>
                                                      s.TermsNConditionWordingSettings.Select(
                                                          tncs => tncs.TermsNConditionWording),
                                                      s =>
                                                      s.SubjectToClauseWordingSettings.Select(
                                                          stcs => stcs.SubjectToClauseWording),
                                                      s =>
                                                      s.CustomMarketWordingSettings.Select(mws => mws.MarketWording),
                                                      s =>
                                                      s.CustomTermsNConditionWordingSettings.Select(
                                                          tncs => tncs.TermsNConditionWording),
                                                      s =>
                                                      s.CustomSubjectToClauseWordingSettings.Select(
// ReSharper disable ReturnValueOfPureMethodIsNotUsed
                                                          stcs => stcs.SubjectToClauseWording)).First();
                        // ReSharper restore ReturnValueOfPureMethodIsNotUsed


                        #region Market Wording

                        if (submissionT.MarketWordingSettings != null)
                        {
                            var removeListMarketWordingSettings = new List<MarketWordingSetting>();

                            foreach (var dbmarketWordingSetting in submissionT.MarketWordingSettings)
                            {
                                if (_MarketWordingSettings.All(mws => mws.Id != dbmarketWordingSetting.Id))
                                {
                                    removeListMarketWordingSettings.Add(dbmarketWordingSetting);
                                }
                            }
                            foreach (var dbmarketWordingSettingToRemove in removeListMarketWordingSettings)
                            {
                                submissionT.MarketWordingSettings.Remove(dbmarketWordingSettingToRemove);
                                _repository.Delete(dbmarketWordingSettingToRemove);

                            }
                        }

                        foreach (var marketWordingSetting in _MarketWordingSettings)
                            {
                                if (marketWordingSetting.Id == 0)
                                {
                                    if (submissionT.MarketWordingSettings != null)
                                    {
                                        MarketWordingSetting setting = marketWordingSetting;
                                        marketWordingSetting.MarketWording =
                                            _repository.Query<MarketWording>(
                                                mw => mw.Id == setting.MarketWording.Id).First();
                                        submissionT.MarketWordingSettings.Add(marketWordingSetting);
                                    }
                                }
                                else
                                {
                                    if (submissionT.MarketWordingSettings != null)
                                    {
                                        var dbmarketWordingSetting =
                                            submissionT.MarketWordingSettings.First(mws => mws.Id == marketWordingSetting.Id);
                                        dbmarketWordingSetting.DisplayOrder = marketWordingSetting.DisplayOrder;
                                    }
                                }
                            }

                        if (submissionT.CustomMarketWordingSettings != null)
                        {
                            var removeListCustomMarketWordingSettings = new List<MarketWordingSetting>();

                            foreach (var dbcustomMarketWordingSetting in submissionT.CustomMarketWordingSettings)
                            {
                                if (_CustomMarketWordingSettings.All(mws => mws.Id != dbcustomMarketWordingSetting.Id))
                                {
                                    removeListCustomMarketWordingSettings.Add(dbcustomMarketWordingSetting);
                                }
                            }
                            foreach (var dbcustomMarketWordingSetting in removeListCustomMarketWordingSettings)
                            {
                                submissionT.CustomMarketWordingSettings.Remove(dbcustomMarketWordingSetting);
                                _repository.Delete(dbcustomMarketWordingSetting);

                            }

                        }

                        foreach (var customMarketWordingSetting in _CustomMarketWordingSettings)
                        {
                            if (customMarketWordingSetting.Id == 0)
                            {
                                if (submissionT.CustomMarketWordingSettings != null)
                                    submissionT.CustomMarketWordingSettings.Add(customMarketWordingSetting);
                            }
                            else
                            {
                                if (submissionT.CustomMarketWordingSettings != null)
                                {
                                    var dbcustomMarketWordingSetting =
                                        submissionT.CustomMarketWordingSettings.First(mws => mws.Id == customMarketWordingSetting.Id);
                                    dbcustomMarketWordingSetting.DisplayOrder = customMarketWordingSetting.DisplayOrder;
                                }
                            }
                        }

                        #endregion

                        #region Terms and condition

                        if (submissionT.TermsNConditionWordingSettings != null)
                        {
                            var removeListTermsNConditionWordingSettings = new List<TermsNConditionWordingSetting>();

                            foreach (var dbtermsNConditionWordingSetting in submissionT.TermsNConditionWordingSettings)
                            {
                                if (_TermsNConditionWordingSettings.All(mws => mws.Id != dbtermsNConditionWordingSetting.Id))
                                {
                                    removeListTermsNConditionWordingSettings.Add(dbtermsNConditionWordingSetting);
                                }
                            }
                            foreach (var dbtermsNConditionWordingSettingToRemove in removeListTermsNConditionWordingSettings)
                            {
                                submissionT.TermsNConditionWordingSettings.Remove(dbtermsNConditionWordingSettingToRemove);
                                _repository.Delete(dbtermsNConditionWordingSettingToRemove);

                            }
                        }

                        foreach (var termsNConditionWordingSetting in _TermsNConditionWordingSettings)
                        {
                            if (termsNConditionWordingSetting.Id == 0)
                            {
                                if (submissionT.TermsNConditionWordingSettings != null)
                                {
                                    TermsNConditionWordingSetting setting = termsNConditionWordingSetting;
                                    termsNConditionWordingSetting.TermsNConditionWording =
                                        _repository.Query<TermsNConditionWording>(
                                            mw => mw.Id == setting.TermsNConditionWording.Id).First();
                                    submissionT.TermsNConditionWordingSettings.Add(termsNConditionWordingSetting);
                                }
                            }
                            else
                            {
                                if (submissionT.TermsNConditionWordingSettings != null)
                                {
                                    var dbtermsNConditionWordingSetting =
                                        submissionT.TermsNConditionWordingSettings.First(mws => mws.Id == termsNConditionWordingSetting.Id);
                                    dbtermsNConditionWordingSetting.DisplayOrder = termsNConditionWordingSetting.DisplayOrder;
                                    dbtermsNConditionWordingSetting.IsStrikeThrough = termsNConditionWordingSetting.IsStrikeThrough;
                                }
                            }
                        }

                        if (submissionT.CustomTermsNConditionWordingSettings != null)
                        {
                            var removeListCustomTermsNConditionWordingSettings = new List<TermsNConditionWordingSetting>();

                            foreach (var dbcustomTermsNConditionWordingSetting in submissionT.CustomTermsNConditionWordingSettings)
                            {
                                if (_CustomTermsNConditionWordingSettings.All(mws => mws.Id != dbcustomTermsNConditionWordingSetting.Id))
                                {
                                    removeListCustomTermsNConditionWordingSettings.Add(dbcustomTermsNConditionWordingSetting);
                                }
                            }
                            foreach (var dbcustomTermsNConditionWordingSetting in removeListCustomTermsNConditionWordingSettings)
                            {
                                submissionT.CustomTermsNConditionWordingSettings.Remove(dbcustomTermsNConditionWordingSetting);
                                _repository.Delete(dbcustomTermsNConditionWordingSetting);

                            }

                        }

                        foreach (var customTermsNConditionWordingSetting in _CustomTermsNConditionWordingSettings)
                        {
                            if (customTermsNConditionWordingSetting.Id == 0)
                            {
                                if (submissionT.CustomTermsNConditionWordingSettings != null)
                                    submissionT.CustomTermsNConditionWordingSettings.Add(customTermsNConditionWordingSetting);
                            }
                            else
                            {
                                if (submissionT.CustomTermsNConditionWordingSettings != null)
                                {
                                    var dbcustomTermsNConditionWordingSetting =
                                        submissionT.CustomTermsNConditionWordingSettings.First(mws => mws.Id == customTermsNConditionWordingSetting.Id);
                                    dbcustomTermsNConditionWordingSetting.DisplayOrder = customTermsNConditionWordingSetting.DisplayOrder;
                                    dbcustomTermsNConditionWordingSetting.IsStrikeThrough = customTermsNConditionWordingSetting.IsStrikeThrough;
                                }
                            }
                        }

                        #endregion

                        #region Subject To Clause Wording

                        if (submissionT.SubjectToClauseWordingSettings != null)
                        {
                            var removeListSubjectToClauseWordingSettings = new List<SubjectToClauseWordingSetting>();

                            foreach (var dbsubjectToClauseWordingSetting in submissionT.SubjectToClauseWordingSettings)
                            {
                                if (_SubjectToClauseWordingSettings.All(mws => mws.Id != dbsubjectToClauseWordingSetting.Id))
                                {
                                    removeListSubjectToClauseWordingSettings.Add(dbsubjectToClauseWordingSetting);
                                }
                            }
                            foreach (var dbsubjectToClauseWordingSettingToRemove in removeListSubjectToClauseWordingSettings)
                            {
                                submissionT.SubjectToClauseWordingSettings.Remove(dbsubjectToClauseWordingSettingToRemove);
                                _repository.Delete(dbsubjectToClauseWordingSettingToRemove);

                            }
                        }

                        foreach (var subjectToClauseWordingSetting in _SubjectToClauseWordingSettings)
                        {
                            if (subjectToClauseWordingSetting.Id == 0)
                            {
                                if (submissionT.SubjectToClauseWordingSettings != null)
                                {
                                    SubjectToClauseWordingSetting setting = subjectToClauseWordingSetting;
                                    subjectToClauseWordingSetting.SubjectToClauseWording =
                                        _repository.Query<SubjectToClauseWording>(
                                            mw => mw.Id == setting.SubjectToClauseWording.Id).First();
                                    submissionT.SubjectToClauseWordingSettings.Add(subjectToClauseWordingSetting);
                                }
                            }
                            else
                            {
                                if (submissionT.SubjectToClauseWordingSettings != null)
                                {
                                    var dbsubjectToClauseWordingSetting =
                                        submissionT.SubjectToClauseWordingSettings.First(mws => mws.Id == subjectToClauseWordingSetting.Id);
                                    dbsubjectToClauseWordingSetting.DisplayOrder = subjectToClauseWordingSetting.DisplayOrder;
                                    dbsubjectToClauseWordingSetting.IsStrikeThrough = subjectToClauseWordingSetting.IsStrikeThrough;
                                }
                            }
                        }

                        if (submissionT.CustomSubjectToClauseWordingSettings != null)
                        {
                            var removeListCustomSubjectToClauseWordingSettings = new List<SubjectToClauseWordingSetting>();

                            foreach (var dbcustomSubjectToClauseWordingSetting in submissionT.CustomSubjectToClauseWordingSettings)
                            {
                                if (_CustomSubjectToClauseWordingSettings.All(mws => mws.Id != dbcustomSubjectToClauseWordingSetting.Id))
                                {
                                    removeListCustomSubjectToClauseWordingSettings.Add(dbcustomSubjectToClauseWordingSetting);
                                }
                            }
                            foreach (var dbcustomSubjectToClauseWordingSetting in removeListCustomSubjectToClauseWordingSettings)
                            {
                                submissionT.CustomSubjectToClauseWordingSettings.Remove(dbcustomSubjectToClauseWordingSetting);
                                _repository.Delete(dbcustomSubjectToClauseWordingSetting);

                            }

                        }

                        foreach (var customSubjectToClauseWordingSetting in _CustomSubjectToClauseWordingSettings)
                        {
                            if (customSubjectToClauseWordingSetting.Id == 0)
                            {
                                if (submissionT.CustomSubjectToClauseWordingSettings != null)
                                    submissionT.CustomSubjectToClauseWordingSettings.Add(customSubjectToClauseWordingSetting);
                            }
                            else
                            {
                                if (submissionT.CustomSubjectToClauseWordingSettings != null)
                                {
                                    var dbcustomSubjectToClauseWordingSetting =
                                        submissionT.CustomSubjectToClauseWordingSettings.First(mws => mws.Id == customSubjectToClauseWordingSetting.Id);
                                    dbcustomSubjectToClauseWordingSetting.DisplayOrder = customSubjectToClauseWordingSetting.DisplayOrder;
                                    dbcustomSubjectToClauseWordingSetting.IsStrikeThrough = customSubjectToClauseWordingSetting.IsStrikeThrough;
                                }
                            }
                        }

                        #endregion

                        #endregion

                        try
                        {
                            _repository.SaveChanges();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            errorMessages.Add("Your changes were not saved because another user modified the record before you."
                                       + (mustSave ? " Subscribe record(s) were updated, however." : String.Empty));
                        }
                    }
                   
                }
            }

            #region Market Wording

            submissionT.MarketWordingSettings = submissionT.MarketWordingSettings.OrderBy(mw => mw.DisplayOrder).ToList();
			submissionT.TermsNConditionWordingSettings =
				submissionT.TermsNConditionWordingSettings.OrderBy(tncs => tncs.DisplayOrder).ToList();
			submissionT.SubjectToClauseWordingSettings =
				submissionT.SubjectToClauseWordingSettings.OrderBy(stcs => stcs.DisplayOrder).ToList();
			submissionT.CustomMarketWordingSettings =
				submissionT.CustomMarketWordingSettings.OrderBy(mw => mw.DisplayOrder).ToList();
			submissionT.CustomTermsNConditionWordingSettings =
				submissionT.CustomTermsNConditionWordingSettings.OrderBy(tncs => tncs.DisplayOrder).ToList();
			submissionT.CustomSubjectToClauseWordingSettings =
				submissionT.CustomSubjectToClauseWordingSettings.OrderBy(stcs => stcs.DisplayOrder).ToList();

            #endregion

            return submissionT; 
        }

		public Object[] GetSubmissions(string sSearch, int skip, int take, string sortCol, string sortDir, bool applyProfileFilters, out int iTotalDisplayRecords, out int iTotalRecords)
		{
			IQueryable<Submission> subs;

			if (applyProfileFilters)
			{
                var currentUser = _webSiteModuleManager.EnsureCurrentUser();

				var filterCOBs = currentUser.FilterCOBs
				                            .Select(w => w.Id).ToArray();
				var filterOffices = currentUser.FilterOffices
				                               .Select(w => w.Id).ToArray();
				var filterMembers = currentUser.FilterMembers
				                               .Where(w => w.UnderwriterCode != null)
											   .Select(w => w.UnderwriterCode).ToArray();
				var additionalCOBs = currentUser.AdditionalCOBs
				                                .Select(w => w.Id).ToArray();
				var additionalOffices = currentUser.AdditionalOffices
				                                   .Select(w => w.Id).ToArray();
				var additionalUsers = currentUser.AdditionalUsers
												 .Where(w => w.UnderwriterCode != null)
												 .Select(w => w.UnderwriterCode).ToArray();

                subs = _repository.Query<Submission>(s => s.Options.Select(o => o.OptionVersions.Select(ov => ov.Quotes))).Where
						(
							s => (
									(
										String.IsNullOrEmpty(sSearch)
										||
										s.InsuredName.Contains(sSearch)
										||
										s.BrokerPseudonym.Contains(sSearch)
                                        ||
                                        s.Options.Any
                                        (
									        o => o.OptionVersions.Any
									        (
									            ov => ov.Quotes.Any
									            (
										            q => q.SubscribeReference == sSearch
                                                )
                                            )
                                        )
									)
								&&
								s.Options.Any
								(
									o => o.OptionVersions.Any
									(
										ov => ov.Quotes.Any
										(
											q => (
													(
														filterCOBs.Contains(q.COBId)
														&&
														filterMembers.Contains(q.OptionVersion.Option.Submission.UnderwriterCode)
														&&
														filterOffices.Contains(q.OriginatingOfficeId)
														&&
														(q.EntryStatus != "NTU" || q.SubmissionStatus != "DECLINED")
													)
													||
													(
													additionalCOBs.Contains(q.COBId)
													||
													additionalUsers.Contains(q.OptionVersion.Option.Submission.UnderwriterCode)
													||
													additionalOffices.Contains(q.OriginatingOfficeId)
													&&
													(q.EntryStatus != "NTU" || q.SubmissionStatus != "DECLINED")
													)
												)
										)
									)
								))
						);
			}
			else
			{
				subs = _repository.Query<Submission>().Where
					(
						s => (
								String.IsNullOrEmpty(sSearch)
								||
								s.InsuredName.Contains(sSearch)
								||
								s.BrokerPseudonym.Contains(sSearch)
                                ||
                                s.Options.Any
                                (
                                    o => o.OptionVersions.Any
                                    (
                                        ov => ov.Quotes.Any
                                        (
                                            q => q.SubscribeReference == sSearch
                                        )
                                    )
                                )
							  )
					);
			}

			iTotalDisplayRecords = iTotalRecords = subs.Count();

			var result = String.Equals(sortDir, "ASC", StringComparison.OrdinalIgnoreCase) ?
				(from sub in subs
				 select new
				 {
                     InceptionDate = (sub.Options.FirstOrDefault().OptionVersions.FirstOrDefault().Quotes.FirstOrDefault().InceptionDate.HasValue) 
                            ? (sub.Options.FirstOrDefault().OptionVersions.FirstOrDefault().Quotes.FirstOrDefault().InceptionDate.Value) 
                            : new DateTime(),
                     sub.BrokerPseudonym,
					 sub.InsuredName,
					 sub.Id

				 }).OrderBy(sortCol).Skip(skip).Take(take).ToArray()
						 :
				(from sub in subs
				 select new
				 {
                     InceptionDate = (sub.Options.FirstOrDefault().OptionVersions.FirstOrDefault().Quotes.FirstOrDefault().InceptionDate.HasValue) 
                            ? (sub.Options.FirstOrDefault().OptionVersions.FirstOrDefault().Quotes.FirstOrDefault().InceptionDate.Value)
                            : new DateTime(),
                     sub.BrokerPseudonym,
					 sub.InsuredName,
					 sub.Id
				 }).OrderByDescending(sortCol).Skip(skip).Take(take).ToArray();

		    return result;
		}

        public Submission[] GetSubmissionsDetailed(string sSearch, int skip, int take, string sortCol, string sortDir, bool applyProfileFilters, out int iTotalDisplayRecords, out int iTotalRecords)
        {
            IQueryable<Submission> subs;

            if (applyProfileFilters)
            {
                var currentUser = _webSiteModuleManager.EnsureCurrentUser();

                var filterCOBs = currentUser.FilterCOBs
                                            .Select(w => w.Id).ToArray();
                var filterOffices = currentUser.FilterOffices
                                               .Select(w => w.Id).ToArray();
                var filterMembers = currentUser.FilterMembers
                                               .Where(w => w.UnderwriterCode != null)
                                               .Select(w => w.UnderwriterCode).ToArray();
                var additionalCOBs = currentUser.AdditionalCOBs
                                                .Select(w => w.Id).ToArray();
                var additionalOffices = currentUser.AdditionalOffices
                                                   .Select(w => w.Id).ToArray();
                var additionalUsers = currentUser.AdditionalUsers
                                                 .Where(w => w.UnderwriterCode != null)
                                                 .Select(w => w.UnderwriterCode).ToArray();

                subs = _repository.Query<Submission>().Where
                        (
                            s => (
                                    (
                                        String.IsNullOrEmpty(sSearch)
                                        ||
                                        s.InsuredName.Contains(sSearch)
                                        ||
                                        s.BrokerPseudonym.Contains(sSearch)
                                        ||
                                        s.BrokerContact.Contains(sSearch)
                                        ||
                                        s.NonLondonBrokerName.Contains(sSearch)
                                        ||
                                        s.UnderwriterCode == sSearch
                                        ||
                                        s.QuotingOfficeId == sSearch
                                        ||
                                        s.Domicile == sSearch
                                        ||
                                        s.Leader == sSearch
                                        ||
                                        s.Options.Any
                                        (
                                            o => o.OptionVersions.Any
                                            (
                                                ov => ov.Quotes.Any
                                                (
                                                    q => q.SubscribeReference == sSearch
                                                )
                                            )
                                        )
                                    )
                                &&
                                s.Options.Any
                                (
                                    o => o.OptionVersions.Any
                                    (
                                        ov => ov.Quotes.Any
                                        (
                                            q => (
                                                    (
                                                        filterCOBs.Contains(q.COBId)
                                                        &&
                                                        filterMembers.Contains(q.OptionVersion.Option.Submission.UnderwriterCode)
                                                        &&
                                                        filterOffices.Contains(q.OriginatingOfficeId)
                                                        &&
                                                        (q.EntryStatus != "NTU" || q.SubmissionStatus != "DECLINED")
                                                    )
                                                    ||
                                                    (
                                                    additionalCOBs.Contains(q.COBId)
                                                    ||
                                                    additionalUsers.Contains(q.OptionVersion.Option.Submission.UnderwriterCode)
                                                    ||
                                                    additionalOffices.Contains(q.OriginatingOfficeId)
                                                    &&
                                                    (q.EntryStatus != "NTU" || q.SubmissionStatus != "DECLINED")
                                                    )
                                                )
                                        )
                                    )
                                ))
                        );
            }
            else
            {
                subs = _repository.Query<Submission>().Where
                    (
                        s => (
                                String.IsNullOrEmpty(sSearch)
                                ||
                                s.InsuredName.Contains(sSearch)
                                ||
                                s.BrokerPseudonym.Contains(sSearch)
                                ||
                                s.BrokerContact.Contains(sSearch)
                                ||
                                s.NonLondonBrokerName.Contains(sSearch)
                                ||

                                s.UnderwriterCode == sSearch
                                ||
                                s.QuotingOfficeId == sSearch
                                ||
                                s.Domicile == sSearch
                                ||
                                s.Leader == sSearch
                                ||
                                s.Options.Any
                                (
                                    o => o.OptionVersions.Any
                                    (
                                        ov => ov.Quotes.Any
                                        (
                                            q => q.SubscribeReference == sSearch
                                        )
                                    )
                                )
                              )
                    );
            }

            iTotalDisplayRecords = iTotalRecords = subs.Count();

            return String.Equals(sortDir, "ASC", StringComparison.OrdinalIgnoreCase) ?
                subs.OrderBy(sortCol).Skip(skip).Take(take).ToArray()
                         :
                subs.OrderByDescending(sortCol).Skip(skip).Take(take).ToArray();
        }

		public SubmissionPreviewDto GetSubmissionPreviewById(int id)
		{
			var submissionPreviewDto = new SubmissionPreviewDto();

            var teamMemberships = _webSiteModuleManager.EnsureCurrentUser().TeamMemberships;
            var currentUserSubmissionTypeIds = teamMemberships.Select(t => t.Team.SubmissionTypeId).ToList();

		    _repository.Query<Submission>(u => u.Id == id,
		                                  s =>
		                                  s.Options.Select(
		                                      o => o.OptionVersions.Select(ov => ov.QuoteSheets.Select(qs => qs.IssuedBy))))
                                              .FirstOrDefault();
			
            var submission =
              _repository.Query<Submission>(u => u.Id == id,
                                            s => s.Options.Select(o => o.OptionVersions.Select(ov => ov.Quotes)))
                         .FirstOrDefault();

			if (submission != null)
			{
                if (currentUserSubmissionTypeIds.Any(c => c == submission.SubmissionTypeId))
                {
                    submissionPreviewDto.ButtonTitle = "Edit";
                    submissionPreviewDto.IsReadOnly = false;
                }
                else
                {
                    submissionPreviewDto.ButtonTitle = "View";
                    submissionPreviewDto.IsReadOnly = true;
                }
                
                submissionPreviewDto.SubmissionId = submission.Id.ToString();
				submissionPreviewDto.Title = submission.Title;
				submissionPreviewDto.InsuredName = (submission.InsuredName.Length < 25) ? submission.InsuredName : submission.InsuredName.Remove(24);
				submissionPreviewDto.BrokerPseudonym = submission.BrokerPseudonym;
				submissionPreviewDto.BrokerContact = submission.BrokerContact;
				submissionPreviewDto.BrokerCode = submission.BrokerCode;
			    submissionPreviewDto.Comments = submission.UnderwriterNotes;
			    submissionPreviewDto.SubmissionTypeId = submission.SubmissionTypeId;

				if (submission.Options != null && submission.Options.Count > 0)
				{
					var options = submission.Options.Select(o => o);
					var optionVersions = options.SelectMany(ov => ov.OptionVersions);
					var quotes = optionVersions.SelectMany(ov => ov.Quotes).Where(q => q.IsSubscribeMaster == true).ToList();

					if (quotes != null && quotes.Count > 0)
					{
						submissionPreviewDto.QuoteList = quotes.Select(q =>
						{
							return new QuoteDto
							{
								FacilityRef = q.FacilityRef,
                                QuotedPremium = String.Format("{0:0,0}", q.QuotedPremium),
								EntryStatus = q.SubmissionStatus.Take(1).FirstOrDefault().ToString(),
								StatusTooltip = q.SubmissionStatus,
								SubscribeReference = q.SubscribeReference
							};
						}).ToList();

					}

					if (optionVersions != null && optionVersions.Any())
					{
						var mostRecentQuoteSheet = optionVersions.SelectMany(ov => ov.QuoteSheets).OrderByDescending(qs => qs.IssuedDate).FirstOrDefault();

						if (mostRecentQuoteSheet != null)
						{
							submissionPreviewDto.QuoteSheet = new QuoteSheetDto
							{
								Guid = mostRecentQuoteSheet.Guid,
								IssuedBy = mostRecentQuoteSheet.IssuedBy.DomainLogon,
								IssuedDate = mostRecentQuoteSheet.IssuedDate.Value,
								ObjectStore = mostRecentQuoteSheet.ObjectStore,
								Title = mostRecentQuoteSheet.Title
							};
						}
					}

				}
			}

			return submissionPreviewDto;
		}

        public List<CrossSellingCheckDto> CrossSellingCheck(string insuredName, int thisSubmissionId)
        {
            var teamMemberships = _webSiteModuleManager.EnsureCurrentUser().TeamMemberships;
            var currentUserSubmissionTypeIds = teamMemberships.Select(t => t.Team.SubmissionTypeId).ToList();

            var submissions =
              _repository.Query<Submission>(u => u.InsuredName == insuredName && u.Id != thisSubmissionId,
                                            s => s.Options.Select(o => o.OptionVersions.Select(ov => ov.Quotes))).ToList();


            var allcrossSellList = (from submission in submissions
                                    let options = submission.Options.Select(o => o)
                                    let optionVersions = options.SelectMany(ov => ov.OptionVersions)
                                    let anyQuotes = optionVersions.SelectMany(ov => ov.Quotes).Any(q => q.SubmissionStatus != "FIRM ORDER")
                                    where anyQuotes
                                    select new CrossSellingCheckDto
                                    {
                                        SubmissionId = submission.Id.ToString(),
                                        SubmissionTitle = submission.Title,
                                        SubmissionTypeId = submission.SubmissionTypeId,
                                        Underwriter = submission.UnderwriterCode,
                                        QuotingOffice = submission.QuotingOfficeId,
                                        ButtonTitle = "View",
                                        IsReadOnly = true
                                    }).ToList();

            foreach (var crossSellingCheckDto in allcrossSellList.Where(crossSellingCheckDto =>
                    currentUserSubmissionTypeIds.Any(c => c == crossSellingCheckDto.SubmissionTypeId)))
            {
                crossSellingCheckDto.ButtonTitle = "Edit";
                crossSellingCheckDto.IsReadOnly = false;
            }

            return allcrossSellList;
        }

		public void Dispose()
		{
			_repository.Dispose();
		}
    }
}