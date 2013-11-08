using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validus.ConsoleData
{
    public interface ITeamSetup
    {
        string DomainPrefix{get;set;}

        void SetUpCob();

        void SetUpTermsNCondition();

        void SetUpLinks();

        void SetUpUser();

        void SetupTeam();

    }
}
