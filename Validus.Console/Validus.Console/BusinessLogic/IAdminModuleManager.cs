using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Validus.Console.DTO;
using Validus.Models;

namespace Validus.Console.BusinessLogic
{
    public interface IAdminModuleManager
    {
        // Team
        Team GetTeam(int? teamId);
        TeamDto GetTeamBySubmissionTypeId(string submissionTypeId);
        //List<Team> GetTeams();
        List<TeamDto> GetTeamsFullData();
        List<TeamDto> GetTeamsBasicData();
        RequiredDataCreateTeamDto GetRequiredDataCreateTeam();
        TeamDto CreateTeam(TeamDto team);
        TeamDto EditTeam(TeamDto team);
        string DeleteTeam(Team team);

        // User
        UserDto GetUser(int? userId);
        List<User> GetUsers();
        List<string> GetUsersInTeam(int? teamId);
        int CreateUser(UserDto user);
        string EditUser(UserDto user);
        string DeleteUser(User user);
        UserDto GetSelectedUserByName(string userName);
        UserDto GetUserPersonalSettings();
        List<UserDto> SearchForUserByName(string userName);
        RequiredDataUserDto GetRequiredDataCreateUser();
        List<UserTeamLinkDto> GetUserTeamLinks();
        List<UserTeamQuoteTemplateDto> GetUserTeamQuoteTemplates();

        // Link
        Link GetLink(int? linkId);
        List<Link> GetLinks();
        Link CreateLink(Link link);
        Link EditLink(Link link);
        string DeleteLink(Link link);
        List<Link> GetLinksForTeam(int? teamId);
        string SaveLinksForTeam(TeamLinksDto teamLinksDto);


        //QuoteTemplate
        QuoteTemplate GetQuoteTemplate(int? quoteTemplateId);
        List<QuoteTemplate> GetQuoteTemplates();
        QuoteTemplate CreateQuoteTemplate(QuoteTemplate quoteTemplate);
        QuoteTemplate EditQuoteTemplate(QuoteTemplate quoteTemplate);
        string DeleteQuoteTemplate(QuoteTemplate quoteTemplate);
        List<QuoteTemplate> GetQuoteTemplatesForTeam(int? teamId);
        string SaveQuoteTemplatesForTeam(TeamQuoteTemplatesDto teamQuoteTemplatesDto);

        //Accelerator
        AppAccelerator CreateAccelerator(AppAccelerator accelerator);
        AppAccelerator EditAccelerator(AppAccelerator accelerator);
        string DeleteAccelerator(AppAccelerator accelerator);
        List<AppAccelerator> GetAccelerators();
        List<AppAccelerator> GetAcceleratorsForTeam(int? teamId);
        string SaveAcceleratorsForTeam(TeamAppAcceleratorsDto teamAcceleratorsDto);
        List<UserTeamAcceleratorDto> GetUserTeamAccelerators();
        string GetAcceleratorMetaDataById(string id);

        // Helpers
        void AddTeamFilters(IEnumerable<int> teamIds, User user, Team team);

        //MarketWording
        MarketWording CreateMarketWording(MarketWording marketWording);
        MarketWording EditMarketWording(MarketWording marketWording);
        string DeleteMarketWording(MarketWording marketWording);
        List<MarketWording> GetMarketWordings();

        List<MarketWording> GetMarketWordingsByPaging(string searchTerm, String sortCol, string sortDir, int skip,
                                                      int take, out Int32 count, out Int32 totalCount);
        List<MarketWordingSetting> GetMarketWordingsForTeamOffice(int? teamId, string officeId);
        string SaveMarketWordingsForTeamOffice(TeamMarketWordingsDto teamMarketWordingsDto);

        SubjectToClauseWording CreateSubjectToClauseWording(SubjectToClauseWording subjectToClauseWording);
        SubjectToClauseWording EditSubjectToClauseWording(SubjectToClauseWording subjectToClauseWording);
        string DeleteSubjectToClauseWording(SubjectToClauseWording subjectToClauseWording);
        List<SubjectToClauseWording> GetSubjectToClauseWordings();
        List<SubjectToClauseWordingSetting> GetSubjectToClauseWordingsForTeamOffice(int? teamId, string officeId);
        string SaveSubjectToClauseWordingsForTeamOffice(TeamSubjectToClauseWordingsDto teamSubjectToClauseWordingsDto);


        TermsNConditionWording CreateTermsNConditionWording(TermsNConditionWording termsNConditionWording);
        TermsNConditionWording EditTermsNConditionWording(TermsNConditionWording termsNConditionWording);
        string DeleteTermsNConditionWording(TermsNConditionWording termsNConditionWording);
        List<TermsNConditionWording> GetTermsNConditionWordings();
        List<TermsNConditionWording> GetTermsNConditionWordingsByPaging(string searchTerm, String sortCol, string sortDir, int skip,
                                                             int take, out Int32 count, out Int32 totalCount);
        List<TermsNConditionWordingSetting> GetTermsNConditionWordingsForTeamOffice(int? teamId, string officeId);
        string SaveTermsNConditionWordingsForTeamOffice(TeamTermsNConditionWordingsDto teamTermsNConditionWordingsDto);

        
    }
}
