using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Validus.Console.Data;
using Validus.Models;

namespace Validus.ConsoleData
{
    public abstract class TeamSetup:ITeamSetup
    {
// ReSharper disable InconsistentNaming
        protected readonly IConsoleRepository _consoleRepository;
// ReSharper restore InconsistentNaming

        protected TeamSetup(IConsoleRepository consoleRepository)
        {
            _consoleRepository = consoleRepository;
        }
        public string DomainPrefix { get; set; }

        public abstract void SetUpCob();
        public abstract void SetUpTermsNCondition();
        public abstract void SetUpLinks();
        public abstract void SetUpUser();
        public abstract void SetupTeam();

        protected User ResolveUser(string officeId,string fullname, string domainName, string underwriterCode)
        {
            var user = _consoleRepository.Query<User>(u => u.DomainLogon == DomainPrefix + @"\" + domainName).SingleOrDefault();
            if (user == null)
            {
                user = new User
                {
                    DomainLogon = DomainPrefix + @"\" + domainName, //<??>
                    AdditionalOffices = new List<Office> { },
                    AdditionalUsers = new List<User> { },
                    IsActive = true,
                    OpenTabs = new List<Tab> { new Tab { Url = "/Submission/CreateSubmission", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now }, new Tab { Url = "/Submission/_Edit/1", CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now } },
                    
                    DefaultOrigOffice = _consoleRepository.Query<Office>(off => off.Id == officeId).Single(),
                    CreatedBy = "InitialSetup",
                    CreatedOn = DateTime.Now,
                    ModifiedBy = "InitialSetup",
                    ModifiedOn = DateTime.Now
                };

                if (!string.IsNullOrEmpty(underwriterCode))
                {
                    user.UnderwriterCode = underwriterCode; //<??>
                    user.Underwriter =
                        _consoleRepository.Query<Underwriter>(uw => uw.Code == underwriterCode).SingleOrDefault() ??
                        new Underwriter
                            {
                                Name = fullname,
                                Code = underwriterCode,
                                CreatedBy = "InitialSetup",
                                CreatedOn = DateTime.Now,
                                ModifiedBy = "InitialSetup",
                                ModifiedOn = DateTime.Now
                            };
                }
                _consoleRepository.Add(user);
            }  
            return user;
        }

        protected Link ResolveLink(string title, string category, string url)
        {
            var link = _consoleRepository.Query<Link>(lnk => lnk.Title == title).SingleOrDefault();

            if (link == null)
            {
                link = new Link{Category = category,Title = title,Url = url,CreatedBy = "InitialSetup",CreatedOn = DateTime.Now,ModifiedBy = "InitialSetup",ModifiedOn = DateTime.Now};
                _consoleRepository.Add(link);
            }

            return link;

        }

        protected COB ResolveCob(string id, string narrative)
        {
            var cob = _consoleRepository.Query<COB>(c => c.Id == id).SingleOrDefault();
            
            if (cob == null)
            {
                cob =  new COB { Id = id, Narrative = narrative, CreatedBy = "InitialSetup", CreatedOn = DateTime.Now, ModifiedBy = "InitialSetup", ModifiedOn = DateTime.Now };
                _consoleRepository.Add(cob);
            }
            return cob;
        }

        protected TermsNConditionWording ResolveTermsNConditionWording(string title)
        {
            var termsNConditionWording = _consoleRepository.Query<TermsNConditionWording>(tnc => tnc.Title == title).SingleOrDefault();
            if (termsNConditionWording == null)
            {
                termsNConditionWording = new TermsNConditionWording { WordingRefNumber = "", Title = title };
                _consoleRepository.Add(termsNConditionWording);
            }
            return termsNConditionWording;
        }
    }
}
