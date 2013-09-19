using System;
using System.Collections.Generic;
using Validus.Console.InsuredService;

namespace Validus.Console.Data
{
    public interface IInsuredData
    {
        List<InsuredDetails> GetInsuredDetailsByNameAndCobs(string insuredName, string cobs);
        List<InsuredDetails> GetInsuredDetailsByName(string insuredName);
        InsuredMeasures GetInsuredMeasuresById(string insuredName);
        List<InsuredMeasures> ListInsuredMeasures();
    }
}
