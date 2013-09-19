using System;
using System.Collections.Generic;
using Validus.Console.InsuredService;
namespace Validus.Console.BusinessLogic
{
    public interface IInsuredBusinessModule
    {
        List<InsuredDetails> GetInsuredDetailsByNameAndCobs(string insuredName);
        List<InsuredDetails> GetInsuredDetailsByName(string insuredName);
        InsuredMeasures GetInsuredMeasuresById(string insuredName);
        List<InsuredMeasures> ListInsuredMeasures();
    }
}
