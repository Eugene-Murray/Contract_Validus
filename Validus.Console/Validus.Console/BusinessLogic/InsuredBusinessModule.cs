using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Validus.Console.Data;
using Validus.Console.InsuredService;
using Validus.Core.LogHandling;
using Validus.Models;

namespace Validus.Console.BusinessLogic
{
    public class InsuredBusinessModule : Validus.Console.BusinessLogic.IInsuredBusinessModule
    {
        public readonly ILogHandler _logHandler;
        public readonly IInsuredData _insuredData;
        public readonly IConsoleRepository _consoleRepository;

        public InsuredBusinessModule(IInsuredData insuredData, ILogHandler logHandler, IConsoleRepository consoleRepository)
        {
            _logHandler = logHandler;
            _insuredData = insuredData;
            _consoleRepository = consoleRepository;
        }

        public List<InsuredMeasures> ListInsuredMeasures()
        {
            return _insuredData.ListInsuredMeasures();
        }

        public InsuredMeasures GetInsuredMeasuresById(string insuredName)
        {
            return _insuredData.GetInsuredMeasuresById(insuredName);
        }

        public List<InsuredDetails> GetInsuredDetailsByName(string insuredName)
        {
            return _insuredData.GetInsuredDetailsByName(insuredName);
        }

        public List<InsuredDetails> GetInsuredDetailsByNameAndCobs(string insuredName)
        {
            // get all of the cob's for the team 
            // get the user
            var userName = HttpContext.Current.User.Identity.Name;
            // get the users teams
            var currentUserTeams = _consoleRepository.Query<TeamMembership>().Where(tm => tm.User.DomainLogon.Contains(userName) && tm.IsCurrent).Select(t => t.Team);
            if ((currentUserTeams != null) && (currentUserTeams.Any()))
            {
                List<COB> cobs = currentUserTeams.SelectMany(t => t.RelatedCOBs).Distinct().ToList();
                if ((cobs != null) && (cobs.Count > 0))
                {
                    // convert the list of cobs to a string
                    var coblist = "";
                    foreach (COB cob in cobs)
                    {
                        if (coblist.Length > 0)
                            coblist += ",";
                        coblist += cob.Id + " - " + cob.Narrative;
                    }

                    if (!string.IsNullOrEmpty(coblist))
                    {
                        return _insuredData.GetInsuredDetailsByNameAndCobs(insuredName, coblist);
                    }
                }
            }

            return new List<InsuredDetails>();
        }
    }
}