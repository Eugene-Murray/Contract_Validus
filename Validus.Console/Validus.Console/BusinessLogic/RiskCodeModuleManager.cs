using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Xml;
using Validus.Console.Data;
using Validus.Console.SubscribeService;
using Validus.Core.HttpContext;
using Validus.Core.LogHandling;
using Validus.Models;
using Validus.Console.DTO;
using System.Data.Entity.Infrastructure;
using System.Globalization;

namespace Validus.Console.BusinessLogic
{
	public class RiskCodeModuleManager : IRiskCodeModuleManager
	{
	    private readonly IConsoleRepository _repository;
        private readonly ILogHandler _logHandler;

        public RiskCodeModuleManager(IConsoleRepository rep, ILogHandler logHandler)
		{
			_repository = rep;
			_logHandler = logHandler;		    
		}

		public void Dispose()
		{
			_repository.Dispose();
		}

        public List<RiskCode> GetRiskCodesBySubmissionTypeId(string submissionTypeId)
        {
            Team t = _repository.Query<Team>(s => s.SubmissionTypeId == submissionTypeId, s => s.RelatedRisks).FirstOrDefault();
            
            if (t != null)
                return t.RelatedRisks.ToList();
            else
                return null;
        }
    }
}