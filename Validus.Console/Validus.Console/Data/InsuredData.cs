using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Validus.Console.InsuredService;

namespace Validus.Console.Data
{
    public class InsuredData : IInsuredData
    {
        IInsuredService _insuredService;

        public InsuredData(IInsuredService insuredService)
        {
            _insuredService = insuredService;
        }

        public List<InsuredMeasures> ListInsuredMeasures()
        {
            return _insuredService.ListInsuredMeasures();
        }

        public InsuredMeasures GetInsuredMeasuresById(string insuredName)
        {
            return _insuredService.GetInsuredMeasuresById(insuredName);
        }

        public List<InsuredDetails> GetInsuredDetailsByName(string insuredName)
        {
            return _insuredService.GetInsuredDetailsByName(insuredName);
        }

        public List<InsuredDetails> GetInsuredDetailsByNameAndCobs(string insuredName,string cobs)
        {
            return _insuredService.GetInsuredDetailsByNameAndCobs(insuredName,cobs);
        }
    }
}