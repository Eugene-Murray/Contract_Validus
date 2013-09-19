using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Validus.Console.DTO;
using Validus.Console.WebPolicy;

namespace Validus.Console.BusinessLogic
{
    public interface IPolicyBusinessModule
    {
        RenewalPolicy[] GetRenewalPolicies(DateTime expiryStartDate, DateTime expiryEndDate, string searchTerm, String sortCol, string sortDir, int skip, int take, bool applyProfileFilters, out Int32 count, out Int32 totalCount);

        RenewalPolicyDetailed[] GetRenewalPoliciesDetailed(DateTime expiryStartDate, DateTime expiryEndDate, string searchTerm, String sortCol, string sortDir, int skip, int take, bool applyProfileFilters, out Int32 count, out Int32 totalCount);

        RenewalPolicyDetailed GetRenewalPolicyDetailsByPolId(string polId);

        Object GetPolicyDetailsByPolId(string polId);
    }
}
