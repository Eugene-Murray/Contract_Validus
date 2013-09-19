using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Validus.Console.DTO;
using Validus.Models;

namespace Validus.Console.Data
{
    public interface IQuoteSheetData
    {
        byte[] CreateQuoteSheetPdf(CreateQuoteSheetDto createQuoteSheetDto);
        Guid SaveQuoteSheetToDMS(QuoteSheet quoteSheet, byte[] reportBytes, Submission submission);
    }
}
