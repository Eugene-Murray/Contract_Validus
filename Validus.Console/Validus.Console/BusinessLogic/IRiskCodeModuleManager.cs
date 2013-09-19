using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Validus.Models;

namespace Validus.Console.BusinessLogic
{
    public interface IRiskCodeModuleManager
    {
        List<RiskCode> GetRiskCodesBySubmissionTypeId(String submissionTypeId);
    }
}
