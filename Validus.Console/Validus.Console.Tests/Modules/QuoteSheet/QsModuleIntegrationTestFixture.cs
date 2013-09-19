using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Security.Principal;
using System.Threading;
using System.Web;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Validus.Console.BusinessLogic;
using Validus.Console.DTO;
using Validus.Console.Data;
using Validus.Console.Data.DbInitializer;
using Validus.Console.Tests.Helpers;
using Validus.Core.HttpContext;
using Validus.Core.LogHandling;

namespace Validus.Console.Tests.Modules.QuoteSheet
{
    [TestClass]
    public class QsModuleIntegrationTestFixture : IntegrationFixtureBase
    {
        private static IQuoteSheetModule QuoteSheetModule;
        private static IQuoteSheetData QuoteSheetData;

		//[TestInitialize]
		//public void Init()
		//{
		//	IConsoleRepository consoleRepository = new ConsoleRepository();
		//	var mockCurrentHttpContext = new Mock<ICurrentHttpContext>();
		//	mockCurrentHttpContext.Setup(h => h.CurrentUser).Returns(new GenericPrincipal(new GenericIdentity(@"talbotdev\MurrayE"), null));
		//	mockCurrentHttpContext.Setup(h => h.Context).Returns(MvcMockHelpers.FakeHttpContextWithSession());

		//	ILogHandler logHandler = new LogHandler();
		//	QuoteSheetData = new QuoteSheetData(logHandler, mockCurrentHttpContext.Object);

		//	QuoteSheetModule = new QuoteSheetModule(consoleRepository, QuoteSheetData, logHandler, mockCurrentHttpContext.Object);
		//}

		//[TestMethod]
		//public void CreateQuoteSheet_Success()
		//{
		//	// Assign

		//	var createQuoteSheetDto = new CreateQuoteSheetDto
		//		{
		//			SubmissionId = 1,
		//			OptionList =
		//				new List<OptionDto> { new OptionDto { OptionId = 1, OptionVersionNumberList = new List<int> { 0, 1, 2 } }, new OptionDto { OptionId = 2, OptionVersionNumberList = new List<int> { 0, 1, 2, 4, 5 } } }
		//		};

		//	// Act
		//	var actualQuoteSheetResult = QuoteSheetModule.CreateQuoteSheet(createQuoteSheetDto);

		//	// Assert
		//	Assert.IsNotNull(actualQuoteSheetResult);
		//}
    }
}
