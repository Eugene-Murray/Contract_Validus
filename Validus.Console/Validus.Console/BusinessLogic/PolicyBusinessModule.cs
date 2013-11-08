using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Validus.Console.Data;
using Validus.Console.DTO;
using Validus.Console.DocumentManagementService;
using Validus.Console.WebPolicy;
using Validus.Core.LogHandling;
using SubscribeService = Validus.Console.SubscribeService;
using PolicyDataService = Validus.Console.PolicyDataService;

namespace Validus.Console.BusinessLogic
{
    public class PolicyBusinessModule : IPolicyBusinessModule
    {
        public readonly IPolicyData _policyData;
        public readonly SubscribeService.IPolicyService _subscribeService;
        public readonly PolicyDataService.IPolicyService _policyDataService;
        private readonly IWebSiteModuleManager _webSiteModuleManager;
        private readonly ILogHandler _logHandler;

        public PolicyBusinessModule(IPolicyData policyData, 
            SubscribeService.IPolicyService subscribeService,
            PolicyDataService.IPolicyService policyDataService,
            IWebSiteModuleManager webSiteModuleManager, ILogHandler logHandler)
        {
            _policyData = policyData;
            _subscribeService = subscribeService;
            _policyDataService = policyDataService;
            _webSiteModuleManager = webSiteModuleManager;
            _logHandler = logHandler;
        }

        public RenewalPolicy[] GetRenewalPolicies(DateTime expiryStartDate, DateTime expiryEndDate, string searchTerm,
                                                  string sortCol, string sortDir, int skip, int take,
                                                  bool applyProfileFilters, out int count, out int totalCount, List<Tuple<String, String, String>> extraFilters)
        {
            Tuple<String, String, String> cobEF = null;
            Tuple<String, String, String> yearEF = null;
            Tuple<String, String, String> descEF = null;
            Tuple<String, String, String> insdEF = null;
            Tuple<String, String, String> bkrEF = null;
            Tuple<String, String, String> uwrEF = null;

            if (extraFilters != null)
            {
                cobEF = extraFilters.FirstOrDefault(f => f.Item1 == "COBId");
                yearEF = extraFilters.FirstOrDefault(f => f.Item1 == "AccountYear");
                descEF = extraFilters.FirstOrDefault(f => f.Item1 == "Description");
                insdEF = extraFilters.FirstOrDefault(f => f.Item1 == "InsuredName");
                bkrEF = extraFilters.FirstOrDefault(f => f.Item1 == "BrokerPseudonym");
                uwrEF = extraFilters.FirstOrDefault(f => f.Item1 == "UnderwriterCode");
            }
            String cobExtraFilter = cobEF == null ? String.Empty : cobEF.Item3;
            Int32 yearParse;
            Int32? yearExtraFilter = null;
            if (Int32.TryParse(yearEF == null ? String.Empty : yearEF.Item3, out yearParse))
                yearExtraFilter = yearParse;

            String descExtraFilter = descEF == null ? String.Empty : descEF.Item3;
            String insdExtraFilter = insdEF == null ? String.Empty : insdEF.Item3;
            String bkrExtraFilter = bkrEF == null ? String.Empty : bkrEF.Item3;
            String uwrExtraFilter = uwrEF == null ? String.Empty : uwrEF.Item3;

            var pols = _policyData.GetRenewalPolicies(false);

            var sortProp = typeof (RenewalPolicy).GetProperty(sortCol);
            var filteredPols = default(IEnumerable<RenewalPolicy>);

            if (applyProfileFilters)
            {
                var currentUser = _webSiteModuleManager.EnsureCurrentUser();

                var filterCOBs = (currentUser.FilterCOBs != null
                                      ? currentUser.FilterCOBs.Select(w => w.Id).ToArray()
                                      : new string[0]);
                var filterOffices = (currentUser.FilterOffices != null
                                         ? currentUser.FilterOffices.Select(w => w.Id).ToArray()
                                         : new string[0]);
                var filterMembers = (currentUser.FilterMembers != null
                                         ? currentUser.FilterMembers.Where(w => w.UnderwriterCode != null)
                                                      .Select(w => w.UnderwriterCode)
                                                      .ToArray()
                                         : new string[0]);
                var additionalCOBs = (currentUser.AdditionalCOBs != null
                                          ? currentUser.AdditionalCOBs.Select(w => w.Id).ToArray()
                                          : new string[0]);
                var additionalOffices = (currentUser.AdditionalOffices != null
                                             ? currentUser.AdditionalOffices.Select(w => w.Id).ToArray()
                                             : new string[0]);
                var additionalUsers = (currentUser.AdditionalUsers != null
                                           ? currentUser.AdditionalUsers.Where(w => w.UnderwriterCode != null)
                                                        .Select(w => w.UnderwriterCode)
                                                        .ToArray()
                                           : new string[0]);

                filteredPols = from pol in pols
                               where
                                   (
                                       (
                                           (
                                               filterCOBs.Contains(pol.COB) &&
                                               filterMembers.Contains(pol.Underwriter) &&
                                               filterOffices.Contains(pol.OriginatingOffice)
                                           ) ||
                                           additionalCOBs.Contains(pol.COB) ||
                                           additionalUsers.Contains(pol.Underwriter) ||
                                           additionalOffices.Contains(pol.OriginatingOffice)
                                       ) &&
                                       (
                                           string.IsNullOrEmpty(searchTerm) ||
                                           pol.InsuredName.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) != -1 ||
                                           pol.Broker.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) != -1 ||
                                           pol.PolicyId.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) != -1
                                       ) &&
                                       pol.ExpiryDate >= expiryStartDate &&
                                       pol.ExpiryDate <= expiryEndDate
                                       &&   //  TODO: search against brokerpsu
                                        ((String.IsNullOrEmpty(insdExtraFilter) || pol.InsuredName.IndexOf(insdExtraFilter, StringComparison.OrdinalIgnoreCase) != -1)
                                        && (String.IsNullOrEmpty(bkrExtraFilter) || pol.Broker.IndexOf(bkrExtraFilter, StringComparison.OrdinalIgnoreCase) != -1)
                                        && (String.IsNullOrEmpty(uwrExtraFilter) || pol.Underwriter == uwrExtraFilter)
                                        && (String.IsNullOrEmpty(descExtraFilter) || pol.Description.IndexOf(descExtraFilter, StringComparison.OrdinalIgnoreCase) != -1)
                                        && (String.IsNullOrEmpty(cobExtraFilter) || pol.COB == cobExtraFilter)
                                        && (!yearExtraFilter.HasValue || pol.InceptionDate.Year == yearExtraFilter)
                                        )
                                   )
                               select pol;
            }
            else
            {
                filteredPols = from pol in pols
                               where
                                   (
                                       pol.ExpiryDate >= expiryStartDate &&
                                       pol.ExpiryDate <= expiryEndDate &&
                                       (
                                           string.IsNullOrEmpty(searchTerm) ||
                                           pol.InsuredName.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) != -1 ||
                                           pol.Broker.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) != -1 ||
                                           pol.PolicyId.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) != -1
                                       )
                                       &&
                                       (    //  TODO: search against brokerpsu
                                            (String.IsNullOrEmpty(insdExtraFilter) || pol.InsuredName.IndexOf(insdExtraFilter, StringComparison.OrdinalIgnoreCase) != -1)
                                            && (String.IsNullOrEmpty(bkrExtraFilter) || pol.Broker.IndexOf(bkrExtraFilter, StringComparison.OrdinalIgnoreCase) != -1)
                                            && (String.IsNullOrEmpty(uwrExtraFilter) || pol.Underwriter == uwrExtraFilter)
                                            && (String.IsNullOrEmpty(descExtraFilter) || pol.Description.IndexOf(descExtraFilter, StringComparison.OrdinalIgnoreCase) != -1)
                                            && (String.IsNullOrEmpty(cobExtraFilter) || pol.COB == cobExtraFilter)
                                            && (!yearExtraFilter.HasValue || pol.InceptionDate.Year == yearExtraFilter)
                                       )
                                   )
                               select pol;
            }

            totalCount = count = filteredPols.Count();

            return string.Equals(sortDir, "ASC", StringComparison.OrdinalIgnoreCase)
                       ? filteredPols.OrderBy(w => sortProp.GetValue(w, null)).Skip(skip).Take(take).ToArray()
                       : filteredPols.OrderByDescending(w => sortProp.GetValue(w, null)).Skip(skip).Take(take).ToArray();
        }

        public RenewalPolicyDetailed[] GetRenewalPoliciesDetailed(DateTime expiryStartDate, DateTime expiryEndDate,
                                                                  string searchTerm, string sortCol, string sortDir,
                                                                  int skip, int take, bool applyProfileFilters,
                                                                  out int count, out int totalCount)
        {
            var currentUser = _webSiteModuleManager.EnsureCurrentUser();
            var teamSubmissionTypes =
                currentUser.TeamMemberships.Where(t => t.Team.SubmissionTypeId != null)
                           .Select(s => s.Team.SubmissionTypeId)
                           .ToList();

            var pols = _policyData.GetRenewalPolicies(false);

            if (teamSubmissionTypes != null)
            {
                foreach (var pol in pols)
                {
                    pol.RenewalData = new RenewalDataDto
                        {
                            PolicyId = pol.PolicyId,
                            TeamSubmissionTypes = teamSubmissionTypes
                        };
                }
            }


            var sortProp = typeof (RenewalPolicy).GetProperty(sortCol);
            var filteredPols = default(IEnumerable<RenewalPolicyDetailed>);

            if (applyProfileFilters)
            {
                var filterCOBs = (currentUser.FilterCOBs != null
                                      ? currentUser.FilterCOBs.Select(w => w.Id).ToArray()
                                      : new string[0]);
                var filterOffices = (currentUser.FilterOffices != null
                                         ? currentUser.FilterOffices.Select(w => w.Id).ToArray()
                                         : new string[0]);
                var filterMembers = (currentUser.FilterMembers != null
                                         ? currentUser.FilterMembers.Where(w => w.UnderwriterCode != null)
                                                      .Select(w => w.UnderwriterCode)
                                                      .ToArray()
                                         : new string[0]);
                var additionalCOBs = (currentUser.AdditionalCOBs != null
                                          ? currentUser.AdditionalCOBs.Select(w => w.Id).ToArray()
                                          : new string[0]);
                var additionalOffices = (currentUser.AdditionalOffices != null
                                             ? currentUser.AdditionalOffices.Select(w => w.Id).ToArray()
                                             : new string[0]);
                var additionalUsers = (currentUser.AdditionalUsers != null
                                           ? currentUser.AdditionalUsers.Where(w => w.UnderwriterCode != null)
                                                        .Select(w => w.UnderwriterCode)
                                                        .ToArray()
                                           : new string[0]);


                filteredPols = from pol in pols
                               where
                                   (
                                       (
                                           filterCOBs.Contains(pol.COB) &&
                                           filterMembers.Contains(pol.Underwriter) &&
                                           filterOffices.Contains(pol.OriginatingOffice) &&
                                           pol.ExpiryDate >= expiryStartDate &&
                                           pol.ExpiryDate <= expiryEndDate
                                       ) ||
                                       additionalCOBs.Contains(pol.COB) ||
                                       additionalUsers.Contains(pol.Underwriter) ||
                                       additionalOffices.Contains(pol.OriginatingOffice)
                                   )
                               select pol;

                filteredPols = RenewalPolicyDetailedsSearch(searchTerm, filteredPols);

            }
            else
            {
                filteredPols = from pol in pols
                               where
                                   (
                                       pol.ExpiryDate >= expiryStartDate &&
                                       pol.ExpiryDate <= expiryEndDate
                                   )
                               select pol;

                filteredPols = RenewalPolicyDetailedsSearch(searchTerm, filteredPols);
            }

            totalCount = count = filteredPols.Count();

            return string.Equals(sortDir, "ASC", StringComparison.OrdinalIgnoreCase)
                       ? filteredPols.OrderBy(w => sortProp.GetValue(w, null)).Skip(skip).Take(take).ToArray()
                       : filteredPols.OrderByDescending(w => sortProp.GetValue(w, null)).Skip(skip).Take(take).ToArray();
        }

        public RenewalPolicyDetailed GetRenewalPolicyDetailsByPolId(string polId)
        {
            return _policyData.GetRenewalPolicies(false).Single(p => p.PolicyId == polId);
        }

        public Object GetPolicyDetailsByPolId(string polId)
        {
            var policy = _policyDataService.GetPolicyAsync(polId).Result;
            var resp = _subscribeService.GetReference(new SubscribeService.GetReferenceRequest() { strPolId = polId });

            var polAsSubscribe = Utility.XmlDeserializeFromString<SubscribeService.PolicyContract>(resp.GetReferenceResult.OutputXml);
            var cob = _policyData.GetCOB(polAsSubscribe.COB);
            polAsSubscribe.COB = cob.Id + " : " + cob.Narrative;
            var broker = _policyData.GetBroker(polAsSubscribe.BkrSeqId);

            return new { Pol = polAsSubscribe, Ldr = policy.Ldr,LdrNo = policy.LdrNo, Broker = broker };


        }

        private static IEnumerable<RenewalPolicyDetailed> RenewalPolicyDetailedsSearch(string searchTerm, IEnumerable<RenewalPolicyDetailed> filteredPols)
        {
            if (!String.IsNullOrEmpty(searchTerm))
            {
                filteredPols = filteredPols.Where(
                    p =>
                    p.InsuredName.Contains(searchTerm) || p.InsuredName.Contains(searchTerm.ToUpper())
                    || p.Broker.Contains(searchTerm) || p.Broker.Contains(searchTerm.ToUpper())
                    || p.PolicyId.Contains(searchTerm) || p.PolicyId.Contains(searchTerm.ToUpper())).ToList();
            }
            return filteredPols;
         }

        public async Task<RiskPreviewDto> RiskPreview(string polId)
        {
            if (string.IsNullOrEmpty(polId)) return new RiskPreviewDto();

            var response = await _policyDataService.GetPolicyAsync(polId);

            if (response == null) return new RiskPreviewDto();

            using (var docService = new DocumentService.DocumentServiceClient())
            {
                var count = await docService.GetDocumentsCountForPolicyAsync(response.PolId);

                var quote = _policyData.GetQuote(response.PolId);

                return new RiskPreviewDto
                    {
                        PolicyId = response.PolId,
                        InsuredName = response.InsdNm,
                        Description = response.Dsc, 
                        EntryStatus = response.EntSt,
                        PolicyStatus = response.St,
                        SubmissionStatus = response.SubmSt,
                        BrokerGroupCode = response.BkrGrpCd,
                        PSU = response.BkrPsu,
                        CD = response.BkrCd,
                        Contact = response.BkrCtc,
                        UMR = response.UMR,
                        Underwriter = response.Uwr,
                        SubscribeNotes = response.Nt,
                        ConsoleQuoteNotes = (quote != null) ? quote.Description : string.Empty,
                        DMSCount = count.ToString()
                    };
            }

        }
    }
}