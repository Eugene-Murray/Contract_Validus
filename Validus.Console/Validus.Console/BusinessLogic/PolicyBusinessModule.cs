using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Validus.Console.Data;
using Validus.Console.DTO;
using Validus.Console.SubscribeService;
using Validus.Console.WebPolicy;

namespace Validus.Console.BusinessLogic
{
    public class PolicyBusinessModule : IPolicyBusinessModule
    {
        public readonly IPolicyData _policyData;
        public readonly IPolicyService _subscribeService;
        private readonly IWebSiteModuleManager _webSiteModuleManager;

        public PolicyBusinessModule(IPolicyData policyData, IPolicyService subscribeService,
                                    IWebSiteModuleManager webSiteModuleManager)
        {
            _policyData = policyData;
            _subscribeService = subscribeService;
            _webSiteModuleManager = webSiteModuleManager;
        }

        public RenewalPolicy[] GetRenewalPolicies(DateTime expiryStartDate, DateTime expiryEndDate, string searchTerm,
                                                  string sortCol, string sortDir, int skip, int take,
                                                  bool applyProfileFilters, out int count, out int totalCount)
        {
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
            using (var svc = new PolicyServiceSoapClient())
            {
                // TODO: make only one call...
                var policy = svc.GetPolicy(polId);
                var resp = _subscribeService.GetReference(new GetReferenceRequest() {strPolId = polId});

                var polAsSubscribe = Utility.XmlDeserializeFromString<PolicyContract>(resp.GetReferenceResult.OutputXml);

                var broker = _policyData.GetBroker(polAsSubscribe.BkrSeqId);

                return new {Pol = polAsSubscribe, Ldr = policy.Ldr, Broker = broker};
            }
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
            await Task.Delay(2000);
            return new RiskPreviewDto();
        }
    }
}