using System;
using Validus.Console.DTO;
using Validus.Models;

namespace Validus.Console.BusinessLogic
{
    public interface IQuoteSheetModule : IDisposable
	{
		string CreateQuoteSheet(CreateQuoteSheetDto dto, out Submission submission);
    }
}