using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Validus.Console.Data;
using Validus.Models;

namespace Validus.Console.BusinessLogic
{
    public class AuditTrailModule : IAuditTrailModule
    {
        public readonly IConsoleRepository ConsoleRepository;

        public AuditTrailModule(IConsoleRepository consoleRepository)
        {
            this.ConsoleRepository = consoleRepository;
        }

        public void Audit(string source, string reference, string title, string description)
        {
            //todo: need to work on dispose
            using (IConsoleRepository consoleRepository = new ConsoleRepository())
            {
                consoleRepository.Add(new AuditTrail
                    {
                        Source = source,
                        Reference = reference,
                        Title = title,
                        Description = description
                    });
                consoleRepository.SaveChanges();
            }
        }
        public List<AuditTrail> GetAuditTrails(string source, string reference)
        {
            return this.ConsoleRepository.Query<AuditTrail>(at => at.Source == source && at.Reference == reference).OrderByDescending(at=>at.CreatedOn).ToList();
        }
    }
}