using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Validus.Models;

namespace Validus.Console.BusinessLogic
{
    public interface IAuditTrailModule
    {
        void Audit(string source, string reference, string title, string description);
        List<AuditTrail> GetAuditTrails(string source, string reference);
    }
}