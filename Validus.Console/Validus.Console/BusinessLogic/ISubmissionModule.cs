using System;
using System.Collections.Generic;
using Validus.Models;
using Validus.Console.DTO;

namespace Validus.Console.BusinessLogic
{
    public interface ISubmissionModule : IDisposable
    {
        Submission GetSubmissionById(int id);
        List<CrossSellingCheckDto> CrossSellingCheck(string insuredName, int thisSubmissionId);
        T CreateSubmission<T>(T submissionT, out List<String> errorMessages) where T : Submission;
        T UpdateSubmission<T>(T submissionT, out List<String> errorMessages, out List<Quote> userValues) where T : Submission;
        SubmissionPreviewDto GetSubmissionPreviewById(int id);
        object[] GetSubmissions(string sSearch, int iDisplayStart, int iDisplayLength, string sortCol, string sSortDir_0, bool applyProfileFilters, out int iTotalDisplayRecords, out int iTotalRecords);
    }
}
