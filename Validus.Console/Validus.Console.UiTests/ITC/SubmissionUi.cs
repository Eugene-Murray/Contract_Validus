using System;
using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using Validus.Console.UiTests.Helper;
using Validus.Console.UiTests.TestFW;

namespace Validus.Console.UiTests.ITC
{
    [TestClass]
    [CodedUITest]
    public class SubmissionUi
    {

        [ClassInitialize]
        [TestCategory("DontRunInContinualIntegration")]
        public static void TestClassInitialise(TestContext tc)
        {
            TestConsole.OpenBrowser();
        }

        [ClassCleanup]
        [TestCategory("DontRunInContinualIntegration")]
        public static void TestClassTeardown()
        {
            TestConsole.CloseBrowser();
        }


        private Action _a = null;
        private string _masterSz1XOz1XVz1XQz1 = string.Empty;
        private string _masterSz1XOz1XVz1XQz2 = string.Empty;
        private string _masterSz1XOz2XVz1XQz1 = string.Empty;
        private string _masterSz1XOz2XVz1XQz2 = string.Empty;
        private string _masterSz1XOz3XVz1XQz1 = string.Empty;
        private string _masterSz1XOz3XVz1XQz2 = string.Empty;
        private string _masterSz1XOz4XVz1XQz1 = string.Empty;
        private string _masterSz1XOz4XVz1XQz2 = string.Empty;
        private string _masterSz1XOz4XVz1XQz3 = string.Empty;
        private string _masterSz1XOz4XVz2XQz1 = string.Empty;
        private string _masterSz1XOz4XVz2XQz2 = string.Empty;
        private string _masterSz1XOz4XVz2XQz3 = string.Empty;
        private string _masterSz1XOz4XVz3XQz1 = string.Empty;
        private string _masterSz1XOz4XVz3XQz2 = string.Empty;
        private string _masterSz1XOz4XVz3XQz3 = string.Empty;
        private string _masterSz1XOz5XVz1XQz1 = string.Empty;
        private string _masterSz1XOz5XVz1XQz2 = string.Empty;


        private string _slaveSz1XOz1XVz1XQz1 = string.Empty;
        private string _slaveSz1XOz1XVz1XQz2 = string.Empty;
        private string _slaveSz1XOz2XVz1XQz1 = string.Empty;
        private string _slaveSz1XOz2XVz1XQz2 = string.Empty;
        private string _slaveSz1XOz3XVz1XQz1 = string.Empty;
        private string _slaveSz1XOz3XVz1XQz2 = string.Empty;
        private string _slaveSz1XOz4XVz1XQz1 = string.Empty;
        private string _slaveSz1XOz4XVz1XQz2 = string.Empty;
        private string _slaveSz1XOz4XVz1XQz3 = string.Empty;
        private string _slaveSz1XOz4XVz2XQz1 = string.Empty;
        private string _slaveSz1XOz4XVz2XQz2 = string.Empty;
        private string _slaveSz1XOz4XVz2XQz3 = string.Empty;
        private string _slaveSz1XOz4XVz3XQz1 = string.Empty;
        private string _slaveSz1XOz4XVz3XQz2 = string.Empty;
        private string _slaveSz1XOz4XVz3XQz3 = string.Empty;
        private string _slaveSz1XOz5XVz1XQz1 = string.Empty;
        private string _slaveSz1XOz5XVz1XQz2 = string.Empty;

        [TestMethod]
        [TestCategory("DontRunInContinualIntegration")]
        public void Console()
        {
            TestConsole.OpenConsole();
            var strTest = TestConsole.CheckConsole();
            Assert.IsTrue(strTest == "Home Page");
        }

        [TestMethod]
        [TestCategory("DontRunInContinualIntegration")]
        public void Test01_New_Submission()
        {
            TestConsole.OpenConsole();
            TestConsole.TestSubmission.ButtonHome.Click();
            TestConsole.TestSubmission.ButtonNewSubmission.Click();

            #region new
            TestConsole.SendMessage("New submission");
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:1-V:1-Q:1";
            #region submission data
            TestConsole.TestSubmission.SubmissionData.KeyInInputBoxInsuredName = "PRETORIA";
            TestConsole.TestSubmission.SubmissionData.SelectInputBoxInsuredName = "PRETORIA PORTLAND CEMENT";

            TestConsole.TestSubmission.SubmissionData.KeyInInputBoxBrokerName = "HOLMAN";
            TestConsole.TestSubmission.SubmissionData.SelectInputBoxBrokerName = "0669";

            TestConsole.TestSubmission.SubmissionData.KeyInputBoxBrokerContact = "NEIL DEXTER";
            TestConsole.TestSubmission.SubmissionData.SelectInputBoxBrokerContact = "NEIL DEXTER";

            TestConsole.TestSubmission.SubmissionData.KeyInInputBoxUnderWriter = "AED";
            TestConsole.TestSubmission.SubmissionData.SelectInputBoxUnderWriter = "AED";

            TestConsole.TestSubmission.SubmissionData.KeyInInputBoxUnderwriterContact = "JAC";
            TestConsole.TestSubmission.SubmissionData.SelectInputBoxUnderwriterContact = "JAC";

            TestConsole.TestSubmission.SubmissionData.KeyInInputOffice = "SNG";
            TestConsole.TestSubmission.SubmissionData.SelectInputOffice = "SNG";

            TestConsole.TestSubmission.SubmissionData.KeyInInputBoxDomicile = "UNITED KINGDOM";
            TestConsole.TestSubmission.SubmissionData.SelectInputBoxDomicile = "UK";

            TestConsole.TestSubmission.SubmissionData.KeyInInputBoxLeader = "TAL";
            TestConsole.TestSubmission.SubmissionData.SelectInputBoxLeader = "TAL";

            TestConsole.TestSubmission.SubmissionData.KeyInInputBoxBrokerage = "10";
            #endregion
            TestConsole.TestSubmission.EnterSubmissionDetails();
            #region quote master data
            TestConsole.TestSubmission.TestOption.TestQuote.QuoteMasterData.KeyInInputBoxCob = "Direct - Casualty - F";
            TestConsole.TestSubmission.TestOption.TestQuote.QuoteMasterData.SelectInputBoxCob = "CF";


            TestConsole.TestSubmission.TestOption.TestQuote.QuoteMasterData.KeyInInputBoxMoa = "FA";
            TestConsole.TestSubmission.TestOption.TestQuote.QuoteMasterData.SelectInputBoxMoa = "FA";

            TestConsole.TestSubmission.TestOption.TestQuote.QuoteMasterData.KeyInputBoxQuotingOffice = "SNG";
            TestConsole.TestSubmission.TestOption.TestQuote.QuoteMasterData.SelectInputBoxQuotingOffice = "SNG";

            #endregion
            TestConsole.TestSubmission.TestOption.TestQuote.EnterQuoteDetails_Master();
            #region quote slave data
            TestConsole.TestSubmission.TestOption.TestQuote.QuoteSlaveData.KeyInInputBoxCurrency = "GBP";
            TestConsole.TestSubmission.TestOption.TestQuote.QuoteSlaveData.SelectInputBoxCurrency = "GBP";


            TestConsole.TestSubmission.TestOption.TestQuote.QuoteSlaveData.KeyInInputBoxLimitCurrency = "GBP";
            TestConsole.TestSubmission.TestOption.TestQuote.QuoteSlaveData.SelectInputBoxLimitCurrency = "GBP";

            TestConsole.TestSubmission.TestOption.TestQuote.QuoteSlaveData.KeyInInputBoxExcessCurrency = "US D";
            TestConsole.TestSubmission.TestOption.TestQuote.QuoteSlaveData.SelectInputBoxExcessCurrency = "USD";
            #endregion
            TestConsole.TestSubmission.TestOption.TestQuote.EnterQuoteDetails_Slave();
            TestConsole.SendMessage("Add Option");
            TestConsole.TestSubmission.TestOption.AddOption();
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:2-V:1-Q:1";
            TestConsole.TestSubmission.TestOption.TestQuote.EnterQuoteDetails_Master();
            TestConsole.TestSubmission.TestOption.TestQuote.EnterQuoteDetails_Slave();
            #endregion

            #region assert
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:1-V:1-Q:1";
            TestConsole.TestSubmission.VerifyFieldStatus(
                                                                    cancelEnabled: true,
                                                                    saveEnabled: true,
                                                                    printQuoteEnabled: false);
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                    addOptionEnabled: true,
                                                                    copyOptionEnabled: true,
                                                                    newVersionEnabled: false,
                                                                    addNewQuote: true);
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: true,
                                                                    masterFieldsEnabled: true,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:2-V:1-Q:1";
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                    addOptionEnabled: true,
                                                                    copyOptionEnabled: true,
                                                                    newVersionEnabled: false,
                                                                    addNewQuote: true);
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: true,
                                                                    masterFieldsEnabled: true,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);
            #endregion
            //Final context,
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:2-V:1-Q:1";
        }

        [TestMethod]
        [TestCategory("DontRunInContinualIntegration")]
        public void Test02_Add_one_Quote_to_each()
        {
            Test01_New_Submission();

            #region Add one Quote to each
            TestConsole.SendMessage("Add one Quote to each");
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:1-V:1-Q:1";
            TestConsole.TestSubmission.TestOption.SelectionOptionTab();
            TestConsole.TestSubmission.TestOption.AddQuote();
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:1-V:1-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.EnterQuoteDetails_Master();
            TestConsole.TestSubmission.TestOption.TestQuote.EnterQuoteDetails_Slave();
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:2-V:1-Q:1";
            TestConsole.TestSubmission.TestOption.SelectionOptionTab();
            TestConsole.TestSubmission.TestOption.AddQuote();
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:2-V:1-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.EnterQuoteDetails_Master();
            TestConsole.TestSubmission.TestOption.TestQuote.EnterQuoteDetails_Slave();
            #endregion

            #region assert
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:1-V:1-Q:1";
            TestConsole.TestSubmission.VerifyFieldStatus(
                                                                    cancelEnabled: true,
                                                                    saveEnabled: true,
                                                                    printQuoteEnabled: false);
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                    addOptionEnabled: true,
                                                                    copyOptionEnabled: true,
                                                                    newVersionEnabled: false,
                                                                    addNewQuote: true);
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: true,
                                                                    masterFieldsEnabled: true,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:1-V:1-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: true,
                                                                    masterFieldsEnabled: true,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:2-V:1-Q:1";
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                    addOptionEnabled: true,
                                                                    copyOptionEnabled: true,
                                                                    newVersionEnabled: false,
                                                                    addNewQuote: true);
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: true,
                                                                    masterFieldsEnabled: true,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:2-V:1-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: true,
                                                                    masterFieldsEnabled: true,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);
            #endregion
            //Final context,
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:2-V:1-Q:2";
        }

        [TestMethod]
        [TestCategory("DontRunInContinualIntegration")]
        public void Test03_Copy_Option2_to_Option3()
        {
            Test02_Add_one_Quote_to_each();

            #region Copy Option2 to Option3
            TestConsole.SendMessage("Copy Option2 to Option3");
            // TestConsole.TestSubmission.SaveSubmission();
            TestConsole.TestSubmission.TestOption.CopyOption();
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:3-V:1-Q:1";
            TestConsole.TestSubmission.TestOption.SelectionOptionTab();
            TestConsole.TestSubmission.TestOption.TestQuote.SelectionQuote();
            TestConsole.TestSubmission.TestOption.TestQuote.EnterQuoteDetails_Slave();
            #endregion

            #region assert
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:1-V:1-Q:1";
            TestConsole.TestSubmission.VerifyFieldStatus(
                                                                   cancelEnabled: true,
                                                                   saveEnabled: true,
                                                                   printQuoteEnabled: false);
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                    addOptionEnabled: true,
                                                                    copyOptionEnabled: true,
                                                                    newVersionEnabled: false,
                                                                    addNewQuote: true);
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: true,
                                                                    masterFieldsEnabled: true,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:1-V:1-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: true,
                                                                    masterFieldsEnabled: true,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:2-V:1-Q:1";
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                    addOptionEnabled: true,
                                                                    copyOptionEnabled: true,
                                                                    newVersionEnabled: false,
                                                                    addNewQuote: true
                                                                    );
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: true,
                                                                    masterFieldsEnabled: true,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:2-V:1-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: true,
                                                                    masterFieldsEnabled: true,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);

            TestConsole.TestSubmission.SubmissionContext = "S:1-O:3-V:1-Q:1";
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                    addOptionEnabled: true,
                                                                    copyOptionEnabled: true,
                                                                    newVersionEnabled: false,
                                                                    addNewQuote: true
                                                                    );
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:3-V:1-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);


            #endregion
            //Final context,
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:3-V:1-Q:1";
        }

        [TestMethod]
        [TestCategory("DontRunInContinualIntegration")]
        public void Test04_Modify_Option_1_Quote_1()
        {
            Test03_Copy_Option2_to_Option3();

            #region Modify Option 1 Quote 1 (No Impact to other Quotes)
            TestConsole.SendMessage("Modify Option 1 Quote 1 (No Impact to other Quotes)");
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:1-V:1-Q:1";
            TestConsole.TestSubmission.TestOption.SelectionOptionTab();
            TestConsole.TestSubmission.TestOption.TestQuote.SelectionQuote();

            #region quote master data
            TestConsole.TestSubmission.TestOption.TestQuote.QuoteMasterData.KeyInInputBoxCob = "Direct - Casualty - F";
            TestConsole.TestSubmission.TestOption.TestQuote.QuoteMasterData.SelectInputBoxCob = "CF";
            TestConsole.TestSubmission.TestOption.TestQuote.QuoteMasterData.KeyInInputBoxMoa = "FA";
            TestConsole.TestSubmission.TestOption.TestQuote.QuoteMasterData.SelectInputBoxMoa = "FA";
            TestConsole.TestSubmission.TestOption.TestQuote.QuoteMasterData.KeyInputBoxQuotingOffice = "LON";
            TestConsole.TestSubmission.TestOption.TestQuote.QuoteMasterData.SelectInputBoxQuotingOffice = "LON";
            #endregion
            TestConsole.TestSubmission.TestOption.TestQuote.InputBoxQuotingOffice.Clear();
            TestConsole.TestSubmission.TestOption.TestQuote.InputBoxQuotingOffice.SendKeys(TestConsole.TestSubmission.TestOption.TestQuote.QuoteMasterData.KeyInputBoxQuotingOffice);
            _a = () => TestConsole.WebDriver.FindElement(By.CssSelector(TestConsole.GetTypeAheadCssSelector(TestConsole.TestSubmission.TestOption.TestQuote.QuoteMasterData.SelectInputBoxQuotingOffice)), TestConsole.LongWait3).Click();
            _a.Retry();

            var checkData01 = TestConsole.TestSubmission.TestOption.TestQuote.InputBoxQuotingOffice.GetAttribute("value");
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:3-V:1-Q:1";
            TestConsole.TestSubmission.TestOption.SelectionOptionTab();
            TestConsole.TestSubmission.TestOption.TestQuote.SelectionQuote();
            var checkData02 = TestConsole.TestSubmission.TestOption.TestQuote.InputBoxQuotingOffice.GetAttribute("value");
            TestConsole.SendMessage(string.Format("Debug: checkData01:{0},checkData02{1}", checkData01, checkData02));
            Assert.IsFalse(checkData01 == checkData02, "Modify Option 1 Quote 1 (No Impact to other Quotes) - Failed");
            TestConsole.SendMessage("Modify Option 1 Quote 1 (No Impact to other Quotes) - Success");
            #endregion

            #region assert
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:1-V:1-Q:1";
            TestConsole.TestSubmission.VerifyFieldStatus(
                                                                   cancelEnabled: true,
                                                                   saveEnabled: true,
                                                                   printQuoteEnabled: false);
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                    addOptionEnabled: true,
                                                                    copyOptionEnabled: true,
                                                                    newVersionEnabled: false,
                                                                    addNewQuote: true);
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: true,
                                                                    masterFieldsEnabled: true,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:1-V:1-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: true,
                                                                    masterFieldsEnabled: true,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:2-V:1-Q:1";
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                    addOptionEnabled: true,
                                                                    copyOptionEnabled: true,
                                                                    newVersionEnabled: false,
                                                                    addNewQuote: true
                                                                    );
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: true,
                                                                    masterFieldsEnabled: true,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:2-V:1-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: true,
                                                                    masterFieldsEnabled: true,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);

            TestConsole.TestSubmission.SubmissionContext = "S:1-O:3-V:1-Q:1";
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                    addOptionEnabled: true,
                                                                    copyOptionEnabled: true,
                                                                    newVersionEnabled: false,
                                                                    addNewQuote: true
                                                                    );
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:3-V:1-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);


            #endregion
            //Final context,
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:3-V:1-Q:1";
        }

        [TestMethod]
        [TestCategory("DontRunInContinualIntegration")]
        public void Test05_Modify_Master_Fields_on_Option_2_Quote_1()
        {
            Test04_Modify_Option_1_Quote_1();

            #region Modify Master Fields on Option 2 Quote 1 (Master details of Option 3 Quote 1 Change In line)
            TestConsole.SendMessage("Modify Master Fields on Option 2 Quote 1 (Master details of Option 3 Quote 1 Change In line)");
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:2-V:1-Q:1";
            TestConsole.TestSubmission.TestOption.SelectionOptionTab();
            TestConsole.TestSubmission.TestOption.TestQuote.SelectionQuote();

            #region quote master data
            TestConsole.TestSubmission.TestOption.TestQuote.QuoteMasterData.KeyInInputBoxCob = "Direct - Casualty - F";
            TestConsole.TestSubmission.TestOption.TestQuote.QuoteMasterData.SelectInputBoxCob = "CF";
            TestConsole.TestSubmission.TestOption.TestQuote.QuoteMasterData.KeyInInputBoxMoa = "FA";
            TestConsole.TestSubmission.TestOption.TestQuote.QuoteMasterData.SelectInputBoxMoa = "FA";
            TestConsole.TestSubmission.TestOption.TestQuote.QuoteMasterData.KeyInputBoxQuotingOffice = "LON";
            TestConsole.TestSubmission.TestOption.TestQuote.QuoteMasterData.SelectInputBoxQuotingOffice = "LON";
            #endregion
            
            TestConsole.TestSubmission.TestOption.TestQuote.InputBoxQuotingOffice.Clear();
            TestConsole.TestSubmission.TestOption.TestQuote.InputBoxQuotingOffice.SendKeys(TestConsole.TestSubmission.TestOption.TestQuote.QuoteMasterData.KeyInputBoxQuotingOffice);
            _a = () => TestConsole.WebDriver.FindElement(By.CssSelector(TestConsole.GetTypeAheadCssSelector(TestConsole.TestSubmission.TestOption.TestQuote.QuoteMasterData.SelectInputBoxQuotingOffice)), TestConsole.LongWait3).Click();
            _a.Retry();

            TestConsole.TestSubmission.TestOption.TestQuote.InputBoxLimitCurrency.Clear();
            TestConsole.TestSubmission.TestOption.TestQuote.InputBoxLimitCurrency.SendKeys("AUD");
            _a = () => TestConsole.WebDriver.FindElement(By.CssSelector(TestConsole.GetTypeAheadCssSelector("AUD")), TestConsole.LongWait3).Click();
            _a.Retry();

            _masterSz1XOz2XVz1XQz1 = TestConsole.TestSubmission.TestOption.TestQuote.GetMasterData();
            _slaveSz1XOz2XVz1XQz1 = TestConsole.TestSubmission.TestOption.TestQuote.GetMasterData();
            TestConsole.PrintScreen(TestConsole.TestSubmission.Context2String());
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:3-V:1-Q:1";
            TestConsole.TestSubmission.TestOption.SelectionOptionTab();
            TestConsole.TestSubmission.TestOption.TestQuote.SelectionQuote();
            _masterSz1XOz3XVz1XQz1 = TestConsole.TestSubmission.TestOption.TestQuote.GetMasterData();
            _slaveSz1XOz3XVz1XQz1 = TestConsole.TestSubmission.TestOption.TestQuote.GetMasterData();
            TestConsole.PrintScreen(TestConsole.TestSubmission.Context2String());
            Assert.IsTrue(_masterSz1XOz2XVz1XQz1 == _masterSz1XOz3XVz1XQz1, "Modify Master Fields on Option 2 Quote 1 (Master details of Option 3 Quote 1 Change In line)");
            Assert.IsTrue(_slaveSz1XOz2XVz1XQz1 == _slaveSz1XOz3XVz1XQz1, "Slave updated");
            

            #endregion

            #region assert
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:1-V:1-Q:1";
            TestConsole.TestSubmission.VerifyFieldStatus(
                                                                   cancelEnabled: true,
                                                                   saveEnabled: true,
                                                                   printQuoteEnabled: false);
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                    addOptionEnabled: true,
                                                                    copyOptionEnabled: true,
                                                                    newVersionEnabled: false,
                                                                    addNewQuote: true);
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: true,
                                                                    masterFieldsEnabled: true,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:1-V:1-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: true,
                                                                    masterFieldsEnabled: true,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:2-V:1-Q:1";
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                    addOptionEnabled: true,
                                                                    copyOptionEnabled: true,
                                                                    newVersionEnabled: false,
                                                                    addNewQuote: true
                                                                    );
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: true,
                                                                    masterFieldsEnabled: true,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:2-V:1-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: true,
                                                                    masterFieldsEnabled: true,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);

            TestConsole.TestSubmission.SubmissionContext = "S:1-O:3-V:1-Q:1";
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                    addOptionEnabled: true,
                                                                    copyOptionEnabled: true,
                                                                    newVersionEnabled: false,
                                                                    addNewQuote: true
                                                                    );
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:3-V:1-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);


            #endregion
            //Final context,
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:3-V:1-Q:1";

        }

        [TestMethod]
        [TestCategory("DontRunInContinualIntegration")]
        public void Test06_Modify_Slave_Fields_on_Option_3_Quote_2()
        {
            Test05_Modify_Master_Fields_on_Option_2_Quote_1();

            #region Modify Slave Fields on Option 3 Quote 2 (Change Occurs Independantly)
            TestConsole.SendMessage("Modify Slave Fields on Option 3 Quote 2 (Change Occurs Independantly)");
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:3-V:1-Q:2";
            TestConsole.TestSubmission.TestOption.SelectionOptionTab();
            TestConsole.TestSubmission.TestOption.TestQuote.SelectionQuote();
            TestConsole.PrintScreen(TestConsole.TestSubmission.Context2String());
            #region quote slave data
            TestConsole.TestSubmission.TestOption.TestQuote.QuoteSlaveData.KeyInInputBoxCurrency = "US D";
            TestConsole.TestSubmission.TestOption.TestQuote.QuoteSlaveData.SelectInputBoxCurrency = "USD";

            TestConsole.TestSubmission.TestOption.TestQuote.QuoteSlaveData.KeyInInputBoxLimitCurrency = "US D";
            TestConsole.TestSubmission.TestOption.TestQuote.QuoteSlaveData.SelectInputBoxLimitCurrency = "USD";

            TestConsole.TestSubmission.TestOption.TestQuote.QuoteSlaveData.KeyInInputBoxExcessCurrency = "US D";
            TestConsole.TestSubmission.TestOption.TestQuote.QuoteSlaveData.SelectInputBoxExcessCurrency = "USD";
            #endregion
            TestConsole.TestSubmission.TestOption.TestQuote.EnterQuoteDetails_Slave();
            var checkData01 = TestConsole.TestSubmission.TestOption.TestQuote.InputBoxCurrency.GetAttribute("value");
            TestConsole.PrintScreen(TestConsole.TestSubmission.Context2String());
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:3-V:1-Q:1";
            TestConsole.TestSubmission.TestOption.SelectionOptionTab();
            TestConsole.TestSubmission.TestOption.TestQuote.SelectionQuote();
            var checkData02 = TestConsole.TestSubmission.TestOption.TestQuote.InputBoxCurrency.GetAttribute("value");
            TestConsole.PrintScreen(TestConsole.TestSubmission.Context2String());
            TestConsole.SendMessage(string.Format("Debug: checkData01:{0},checkData02{1}", checkData01, checkData02));
            TestConsole.PrintScreen(TestConsole.TestSubmission.Context2String());
            Assert.IsFalse(checkData01 == checkData02, "Modify Slave Fields on Option 3 Quote 2 (Change Occurs Independantly)");
            TestConsole.SendMessage("Modify Slave Fields on Option 3 Quote 2 (Change Occurs Independantly) - Success");
            #endregion

            #region assert
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:1-V:1-Q:1";
            TestConsole.TestSubmission.VerifyFieldStatus(
                                                                   cancelEnabled: true,
                                                                   saveEnabled: true,
                                                                   printQuoteEnabled: false);
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                    addOptionEnabled: true,
                                                                    copyOptionEnabled: true,
                                                                    newVersionEnabled: false,
                                                                    addNewQuote: true);
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: true,
                                                                    masterFieldsEnabled: true,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:1-V:1-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: true,
                                                                    masterFieldsEnabled: true,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:2-V:1-Q:1";
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                    addOptionEnabled: true,
                                                                    copyOptionEnabled: true,
                                                                    newVersionEnabled: false,
                                                                    addNewQuote: true
                                                                    );
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: true,
                                                                    masterFieldsEnabled: true,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:2-V:1-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: true,
                                                                    masterFieldsEnabled: true,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);

            TestConsole.TestSubmission.SubmissionContext = "S:1-O:3-V:1-Q:1";
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                    addOptionEnabled: true,
                                                                    copyOptionEnabled: true,
                                                                    newVersionEnabled: false,
                                                                    addNewQuote: true
                                                                    );
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:3-V:1-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);


            #endregion
            //Final context,
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:3-V:1-Q:1";
        }

        [TestMethod]
        [TestCategory("DontRunInContinualIntegration")]
        public void Test07_Set_Option_3_Quote_1_to_be_the_Master_for_this_quote()
        {
            Test06_Modify_Slave_Fields_on_Option_3_Quote_2();

            #region Set Option 3 Quote 1 to be the Master for this quote
            TestConsole.SendMessage("Set Option 3 Quote 1 to be the Master for this quote");
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:3-V:1-Q:1";
            TestConsole.TestSubmission.TestOption.SelectionOptionTab();
            TestConsole.TestSubmission.TestOption.TestQuote.SelectionQuote();
            TestConsole.PrintScreen(TestConsole.TestSubmission.Context2String());
            TestConsole.TestSubmission.TestOption.TestQuote.StarIsPrimary.Click();
            TestConsole.SendMessage("Set Option 3 Quote 1 to be the Master for this quote-completed");
            TestConsole.PrintScreen(TestConsole.TestSubmission.Context2String());
            #endregion

            #region assert
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:1-V:1-Q:1";
            TestConsole.TestSubmission.VerifyFieldStatus(
                                                                   cancelEnabled: true,
                                                                   saveEnabled: true,
                                                                   printQuoteEnabled: false);
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                    addOptionEnabled: true,
                                                                    copyOptionEnabled: true,
                                                                    newVersionEnabled: false,
                                                                    addNewQuote: true);
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: true,
                                                                    masterFieldsEnabled: true,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:1-V:1-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: true,
                                                                    masterFieldsEnabled: true,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:2-V:1-Q:1";
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                    addOptionEnabled: true,
                                                                    copyOptionEnabled: true,
                                                                    newVersionEnabled: false,
                                                                    addNewQuote: true
                                                                    );
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:2-V:1-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: true,
                                                                    masterFieldsEnabled: true,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);

            TestConsole.TestSubmission.SubmissionContext = "S:1-O:3-V:1-Q:1";
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                    addOptionEnabled: true,
                                                                    copyOptionEnabled: true,
                                                                    newVersionEnabled: false,
                                                                    addNewQuote: true
                                                                    );
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: true,
                                                                    masterFieldsEnabled: true,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:3-V:1-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);


            #endregion
            //Final context,
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:3-V:1-Q:1";
        }

        [TestMethod]
        [TestCategory("DontRunInContinualIntegration")]
        public void Test08_Save_the_Submission()
        {
            Test07_Set_Option_3_Quote_1_to_be_the_Master_for_this_quote();

            #region Save the Submission
            TestConsole.SendMessage("Save the Submission");
            TestConsole.TestSubmission.SaveSubmission();

            TestConsole.TestSubmission.SubmissionContext = "S:1-O:1-V:1-Q:1";
            var subscribeReferenceO1 = TestConsole.TestSubmission.TestOption.TestQuote.GetSubscribeReference();
            Assert.IsFalse(string.IsNullOrEmpty(subscribeReferenceO1));
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:2-V:1-Q:1";
            var subscribeReferenceO2 = TestConsole.TestSubmission.TestOption.TestQuote.GetSubscribeReference();
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:3-V:1-Q:1";
            var subscribeReferenceO3 = TestConsole.TestSubmission.TestOption.TestQuote.GetSubscribeReference();
            Assert.IsFalse(string.IsNullOrEmpty(subscribeReferenceO2));
            Assert.IsFalse(subscribeReferenceO1 == subscribeReferenceO2);
            Assert.IsTrue(subscribeReferenceO2 == subscribeReferenceO3);
            #endregion

            #region assert
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:1-V:1-Q:1";
            TestConsole.TestSubmission.VerifyFieldStatus(
                                                                   cancelEnabled: true,
                                                                   saveEnabled: true,//failed
                                                                   printQuoteEnabled: true);
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                    addOptionEnabled: true,
                                                                    copyOptionEnabled: true,
                                                                    newVersionEnabled: false,
                                                                    addNewQuote: true);
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: true,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:1-V:1-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: true,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:2-V:1-Q:1";
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                    addOptionEnabled: true,
                                                                    copyOptionEnabled: true,
                                                                    newVersionEnabled: false,
                                                                    addNewQuote: true
                                                                    );
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:2-V:1-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: true,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);

            TestConsole.TestSubmission.SubmissionContext = "S:1-O:3-V:1-Q:1";
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                    addOptionEnabled: true,
                                                                    copyOptionEnabled: true,
                                                                    newVersionEnabled: false,
                                                                    addNewQuote: true
                                                                    );
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: true,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:3-V:1-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);


            #endregion
            //Final context,
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:3-V:1-Q:1";
        }

        [TestMethod]
        [TestCategory("DontRunInContinualIntegration")]
        public void Test09_Copy_Option_3_to_Create_Option_4()
        {
            Test08_Save_the_Submission();

            #region Copy Option 3 to Create Option 4
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:3-V:1-Q:1";
            TestConsole.TestSubmission.TestOption.SelectionOptionTab();
            TestConsole.TestSubmission.TestOption.TestQuote.SelectionQuote();
            TestConsole.TestSubmission.TestOption.CopyOption();
            #endregion

            #region assert
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:1-V:1-Q:1";
            TestConsole.TestSubmission.VerifyFieldStatus(
                                                                   cancelEnabled: true,
                                                                   saveEnabled: true,
                                                                   printQuoteEnabled: true);
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                    addOptionEnabled: true,
                                                                    copyOptionEnabled: true,
                                                                    newVersionEnabled: false,
                                                                    addNewQuote: true);
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: true,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:1-V:1-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: true,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:2-V:1-Q:1";
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                    addOptionEnabled: true,
                                                                    copyOptionEnabled: true,
                                                                    newVersionEnabled: false,
                                                                    addNewQuote: true
                                                                    );
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:2-V:1-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: true,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);

            TestConsole.TestSubmission.SubmissionContext = "S:1-O:3-V:1-Q:1";
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                    addOptionEnabled: true,
                                                                    copyOptionEnabled: true,
                                                                    newVersionEnabled: false,
                                                                    addNewQuote: true
                                                                    );
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: true,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:3-V:1-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);

            TestConsole.TestSubmission.SubmissionContext = "S:1-O:4-V:1-Q:1";
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                    addOptionEnabled: true,
                                                                    copyOptionEnabled: true,
                                                                    newVersionEnabled: false,
                                                                    addNewQuote: true);
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:4-V:1-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);


            #endregion
            //Final context,
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:3-V:1-Q:1";
        }

        [TestMethod]
        [TestCategory("DontRunInContinualIntegration")]
        public void Test10_New_Quote_on_Option_4()
        {
            Test09_Copy_Option_3_to_Create_Option_4();

            #region New Quote on Option 4
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:4-V:1-Q:1";
            TestConsole.TestSubmission.TestOption.AddQuote();
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:4-V:1-Q:3";
            TestConsole.TestSubmission.TestOption.SelectionOptionTab();
            TestConsole.TestSubmission.TestOption.TestQuote.SelectionQuote();
            TestConsole.TestSubmission.TestOption.TestQuote.EnterQuoteDetails_Master();
            TestConsole.TestSubmission.TestOption.TestQuote.EnterQuoteDetails_Slave();

            #endregion
            #region assert
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:1-V:1-Q:1";
            TestConsole.TestSubmission.VerifyFieldStatus(
                                                                   cancelEnabled: true,
                                                                   saveEnabled: true,
                                                                   printQuoteEnabled: false);
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                    addOptionEnabled: true,
                                                                    copyOptionEnabled: true,
                                                                    newVersionEnabled: false,
                                                                    addNewQuote: true);
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: true,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:1-V:1-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: true,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:2-V:1-Q:1";
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                    addOptionEnabled: true,
                                                                    copyOptionEnabled: true,
                                                                    newVersionEnabled: false,
                                                                    addNewQuote: true
                                                                    );
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:2-V:1-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: true,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);

            TestConsole.TestSubmission.SubmissionContext = "S:1-O:3-V:1-Q:1";
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                    addOptionEnabled: true,
                                                                    copyOptionEnabled: true,
                                                                    newVersionEnabled: false,
                                                                    addNewQuote: true
                                                                    );
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: true,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:3-V:1-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);

            TestConsole.TestSubmission.SubmissionContext = "S:1-O:4-V:1-Q:1";
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                    addOptionEnabled: true,
                                                                    copyOptionEnabled: true,
                                                                    newVersionEnabled: false,
                                                                    addNewQuote: true);
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:4-V:1-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:4-V:1-Q:3";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: true,
                                                                    masterFieldsEnabled: true,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);


            #endregion
            //Final context,
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:4-V:1-Q:3";
        }

        [TestMethod]
        [TestCategory("DontRunInContinualIntegration")]
        public void Test11_Save()
        {
            Test10_New_Quote_on_Option_4();

            TestConsole.SendMessage("Save the Submission");
            TestConsole.TestSubmission.SaveSubmission();
            #region assert
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:1-V:1-Q:1";
            TestConsole.TestSubmission.VerifyFieldStatus(
                                                                   cancelEnabled: true,
                                                                   saveEnabled: true,
                                                                   printQuoteEnabled: true);
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                    addOptionEnabled: true,
                                                                    copyOptionEnabled: true,
                                                                    newVersionEnabled: false,
                                                                    addNewQuote: true);
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: true,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:1-V:1-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: true,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:2-V:1-Q:1";
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                    addOptionEnabled: true,
                                                                    copyOptionEnabled: true,
                                                                    newVersionEnabled: false,
                                                                    addNewQuote: true
                                                                    );
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:2-V:1-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: true,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);

            TestConsole.TestSubmission.SubmissionContext = "S:1-O:3-V:1-Q:1";
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                    addOptionEnabled: true,
                                                                    copyOptionEnabled: true,
                                                                    newVersionEnabled: false,
                                                                    addNewQuote: true
                                                                    );
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: true,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:3-V:1-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);

            TestConsole.TestSubmission.SubmissionContext = "S:1-O:4-V:1-Q:1";
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                    addOptionEnabled: true,
                                                                    copyOptionEnabled: true,
                                                                    newVersionEnabled: false,
                                                                    addNewQuote: true);
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:4-V:1-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:4-V:1-Q:3";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: true,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);


            #endregion
            //Final context,
            //Final context,
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:4-V:1-Q:3";
        }

        [TestMethod]
        [TestCategory("DontRunInContinualIntegration")]
        public void Test12_Create_Quote_Sheet_for_Printing_Including_All_Options()
        {
            Test11_Save();

            #region Create Quote Sheet for Printing Including All Options
            TestConsole.TestSubmission.CreateQuoteSheet();
            #endregion

            #region assert
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:1-V:1-Q:1";
            TestConsole.TestSubmission.VerifyFieldStatus(
                                                                   cancelEnabled: true,
                                                                   saveEnabled: true,
                                                                   printQuoteEnabled: true);
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                    addOptionEnabled: true,
                                                                    copyOptionEnabled: true,
                                                                    newVersionEnabled: true,
                                                                    addNewQuote: false);
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: false,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:1-V:1-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: false,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:2-V:1-Q:1";
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                   addOptionEnabled: true,
                                                                   copyOptionEnabled: true,
                                                                   newVersionEnabled: true,
                                                                   addNewQuote: false
                                                                   );
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: false,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:2-V:1-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: false,
                                                                    slaveUpdated: true);

            TestConsole.TestSubmission.SubmissionContext = "S:1-O:3-V:1-Q:1";
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                    addOptionEnabled: true,
                                                                    copyOptionEnabled: true,
                                                                    newVersionEnabled: true,
                                                                    addNewQuote: false
                                                                    );
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: false,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:3-V:1-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: false,
                                                                    slaveUpdated: true);

            TestConsole.TestSubmission.SubmissionContext = "S:1-O:4-V:1-Q:1";
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                    addOptionEnabled: true,
                                                                    copyOptionEnabled: true,
                                                                    newVersionEnabled: true,
                                                                    addNewQuote: false);
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: false,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:4-V:1-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: false,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:4-V:1-Q:3";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: false,
                                                                    slaveUpdated: true);


            #endregion
            //Final context,
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:4-V:1-Q:3";
        }


        [TestMethod]
        [TestCategory("DontRunInContinualIntegration")]
        public void Test13_New_Version_of_Option_4()
        {
            Test12_Create_Quote_Sheet_for_Printing_Including_All_Options();

            #region New Version of Option 4 (Any Master Records are stolen but Slaves remain Slaves)
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:4-V:1-Q:3";
            TestConsole.TestSubmission.TestOption.SelectionOptionTab();
            TestConsole.TestSubmission.TestOption.TestQuote.SelectionQuote();
            TestConsole.TestSubmission.TestOption.AddVersion();
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:4-V:2-Q:3";
            TestConsole.TestSubmission.TestOption.SelectionOptionTab();
            TestConsole.TestSubmission.TestOption.SelectionVersion();
            TestConsole.TestSubmission.TestOption.TestQuote.SelectionQuote();

            TestConsole.TestSubmission.TestOption.TestQuote.EnterQuoteDetails_Master(exculdeCob: true);
            TestConsole.TestSubmission.TestOption.TestQuote.EnterQuoteDetails_Slave();
            //TestConsole.SendMessage("Change to version 1");
            //TestConsole.TestSubmission.SubmissionContext = "S:1-O:4-V:1-Q:1";
            //TestConsole.TestSubmission.TestOption.SelectionVersion();
            //TestConsole.TestSubmission.TestOption.TestQuote.SelectionQuote();
            //TestConsole.SendMessage("Changed to version 1");

            #endregion

            #region assert
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:1-V:1-Q:1";
            TestConsole.TestSubmission.VerifyFieldStatus(
                                                                   cancelEnabled: true,
                                                                   saveEnabled: true,
                                                                   printQuoteEnabled: true);
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                    addOptionEnabled: true,
                                                                    copyOptionEnabled: true,
                                                                    newVersionEnabled: true,
                                                                    addNewQuote: false);
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: false,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:1-V:1-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: false,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:2-V:1-Q:1";
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                   addOptionEnabled: true,
                                                                   copyOptionEnabled: true,
                                                                   newVersionEnabled: true,
                                                                   addNewQuote: false
                                                                   );
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: false,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:2-V:1-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: false,
                                                                    slaveUpdated: true);

            TestConsole.TestSubmission.SubmissionContext = "S:1-O:3-V:1-Q:1";
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                    addOptionEnabled: true,
                                                                    copyOptionEnabled: true,
                                                                    newVersionEnabled: true,
                                                                    addNewQuote: false
                                                                    );
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: false,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:3-V:1-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: false,
                                                                    slaveUpdated: true);

            TestConsole.TestSubmission.SubmissionContext = "S:1-O:4-V:1-Q:1";
            TestConsole.SendMessage("Change to version");
            TestConsole.TestSubmission.TestOption.SelectionVersion();
            TestConsole.TestSubmission.TestOption.TestQuote.SelectionQuote();
            TestConsole.SendMessage("Changed to version");
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                    addOptionEnabled: true,
                                                                    copyOptionEnabled: true,
                                                                    newVersionEnabled: true,
                                                                    addNewQuote: false);
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: false,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:4-V:1-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: false,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:4-V:1-Q:3";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: false,
                                                                    slaveUpdated: true);

            TestConsole.TestSubmission.SubmissionContext = "S:1-O:4-V:2-Q:1";
            TestConsole.SendMessage("Change to version");
            TestConsole.TestSubmission.TestOption.SelectionVersion();
            TestConsole.TestSubmission.TestOption.TestQuote.SelectionQuote();
            TestConsole.SendMessage("Changed to version");
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                    addOptionEnabled: true,
                                                                    copyOptionEnabled: true,
                                                                    newVersionEnabled: false,
                                                                    addNewQuote: false);
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:4-V:2-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:4-V:2-Q:3";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: true,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);


            #endregion

            //Final context,
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:4-V:1-Q:1";
        }

        [TestMethod]
        [TestCategory("DontRunInContinualIntegration")]
        public void Test14_New_Option_and_2_New_Quotes()
        {
            Test13_New_Version_of_Option_4();

            #region New Option and 2 New Quotes

            TestConsole.TestSubmission.SubmissionContext = "S:1-O:4-V:2-Q:3";
            TestConsole.TestSubmission.TestOption.SelectionOptionTab();
            TestConsole.TestSubmission.TestOption.TestQuote.SelectionQuote();
            TestConsole.TestSubmission.TestOption.AddOption();
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:5-V:1-Q:1";
            TestConsole.TestSubmission.TestOption.TestQuote.EnterQuoteDetails_Master();
            TestConsole.TestSubmission.TestOption.TestQuote.EnterQuoteDetails_Slave();
            TestConsole.TestSubmission.TestOption.AddQuote();
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:5-V:1-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.EnterQuoteDetails_Master();
            TestConsole.TestSubmission.TestOption.TestQuote.EnterQuoteDetails_Slave();

            #endregion

            #region assert
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:1-V:1-Q:1";
            TestConsole.TestSubmission.VerifyFieldStatus(
                                                                   cancelEnabled: true,
                                                                   saveEnabled: true,
                                                                   printQuoteEnabled: false);
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                    addOptionEnabled: true,
                                                                    copyOptionEnabled: true,
                                                                    newVersionEnabled: true,
                                                                    addNewQuote: false);
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: false,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:1-V:1-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: false,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:2-V:1-Q:1";
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                   addOptionEnabled: true,
                                                                   copyOptionEnabled: true,
                                                                   newVersionEnabled: true,
                                                                   addNewQuote: false
                                                                   );
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: false,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:2-V:1-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: false,
                                                                    slaveUpdated: true);

            TestConsole.TestSubmission.SubmissionContext = "S:1-O:3-V:1-Q:1";
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                    addOptionEnabled: true,
                                                                    copyOptionEnabled: true,
                                                                    newVersionEnabled: true,
                                                                    addNewQuote: false
                                                                    );
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: false,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:3-V:1-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: false,
                                                                    slaveUpdated: true);

            TestConsole.TestSubmission.SubmissionContext = "S:1-O:4-V:1-Q:1";
            TestConsole.TestSubmission.TestOption.SelectionOptionTab();
            TestConsole.SendMessage("Change to version");
            TestConsole.TestSubmission.TestOption.SelectionVersion();
            TestConsole.TestSubmission.TestOption.TestQuote.SelectionQuote();
            TestConsole.SendMessage("Changed to version");
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                    addOptionEnabled: true,
                                                                    copyOptionEnabled: true,
                                                                    newVersionEnabled: true,
                                                                    addNewQuote: false);
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: false,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:4-V:1-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: false,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:4-V:1-Q:3";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: false,
                                                                    slaveUpdated: true);

            TestConsole.TestSubmission.SubmissionContext = "S:1-O:4-V:2-Q:1";
            TestConsole.TestSubmission.TestOption.SelectionOptionTab();
            TestConsole.SendMessage("Change to version");
            TestConsole.TestSubmission.TestOption.SelectionVersion();
            TestConsole.TestSubmission.TestOption.TestQuote.SelectionQuote();
            TestConsole.SendMessage("Changed to version");
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                    addOptionEnabled: true,
                                                                    copyOptionEnabled: true,
                                                                    newVersionEnabled: false,
                                                                    addNewQuote: false);
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:4-V:2-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:4-V:2-Q:3";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: true,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);


            TestConsole.TestSubmission.SubmissionContext = "S:1-O:5-V:1-Q:1";
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                    addOptionEnabled: true,
                                                                    copyOptionEnabled: true,
                                                                    newVersionEnabled: false,
                                                                    addNewQuote: true);
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: true,
                                                                    masterFieldsEnabled: true,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:5-V:1-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: true,
                                                                    masterFieldsEnabled: true,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);

            #endregion
            //Final context,
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:5-V:1-Q:2";
        }

        [TestMethod]
        [TestCategory("DontRunInContinualIntegration")]
        public void Test15_Save_the_Submission()
        {
            Test14_New_Option_and_2_New_Quotes();
            #region savesubmission
            TestConsole.SendMessage("Save the Submission");
            TestConsole.TestSubmission.SaveSubmission();
            #endregion

            #region assert
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:1-V:1-Q:1";
            TestConsole.TestSubmission.VerifyFieldStatus(
                                                                   cancelEnabled: true,
                                                                   saveEnabled: true,
                                                                   printQuoteEnabled: true);
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                    addOptionEnabled: true,
                                                                    copyOptionEnabled: true,
                                                                    newVersionEnabled: true,
                                                                    addNewQuote: false);
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: false,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:1-V:1-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: false,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:2-V:1-Q:1";
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                   addOptionEnabled: true,
                                                                   copyOptionEnabled: true,
                                                                   newVersionEnabled: true,
                                                                   addNewQuote: false
                                                                   );
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: false,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:2-V:1-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: false,
                                                                    slaveUpdated: true);

            TestConsole.TestSubmission.SubmissionContext = "S:1-O:3-V:1-Q:1";
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                    addOptionEnabled: true,
                                                                    copyOptionEnabled: true,
                                                                    newVersionEnabled: true,
                                                                    addNewQuote: false
                                                                    );
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: false,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:3-V:1-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: false,
                                                                    slaveUpdated: true);

            TestConsole.TestSubmission.SubmissionContext = "S:1-O:4-V:1-Q:1";
            TestConsole.TestSubmission.TestOption.SelectionOptionTab();
            TestConsole.SendMessage("Change to version");
            TestConsole.TestSubmission.TestOption.SelectionVersion();
            TestConsole.TestSubmission.TestOption.TestQuote.SelectionQuote();
            TestConsole.SendMessage("Changed to version");
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                    addOptionEnabled: true,
                                                                    copyOptionEnabled: true,
                                                                    newVersionEnabled: true,
                                                                    addNewQuote: false);
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: false,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:4-V:1-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: false,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:4-V:1-Q:3";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: false,
                                                                    slaveUpdated: true);

            TestConsole.TestSubmission.SubmissionContext = "S:1-O:4-V:2-Q:1";
            TestConsole.TestSubmission.TestOption.SelectionOptionTab();
            TestConsole.SendMessage("Change to version");
            TestConsole.TestSubmission.TestOption.SelectionVersion();
            TestConsole.TestSubmission.TestOption.TestQuote.SelectionQuote();
            TestConsole.SendMessage("Changed to version");
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                    addOptionEnabled: true,
                                                                    copyOptionEnabled: true,
                                                                    newVersionEnabled: false,
                                                                    addNewQuote: false);
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:4-V:2-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:4-V:2-Q:3";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: true,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);


            TestConsole.TestSubmission.SubmissionContext = "S:1-O:5-V:1-Q:1";
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                    addOptionEnabled: true,
                                                                    copyOptionEnabled: true,
                                                                    newVersionEnabled: false,
                                                                    addNewQuote: true);
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: true,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:5-V:1-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: true,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);

            #endregion
            //Final context,
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:5-V:1-Q:2";
        }

        [TestMethod]
        [TestCategory("DontRunInContinualIntegration")]
        public void Test16_Create_Quote_Sheet_for_Printing_Including_Only_Options()
        {
            Test15_Save_the_Submission();

            #region Create Quote Sheet for Printing Including Only Options 1,2,3, 4 v1 and 5
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:1-V:1-Q:1";
            TestConsole.TestSubmission.TestOption.SelectionOptionTab();
            TestConsole.TestSubmission.TestOption.TestQuote.SelectionQuote();
            TestConsole.TestSubmission.SelectQuoteSheetForPrinting();

            TestConsole.TestSubmission.SubmissionContext = "S:1-O:2-V:1-Q:1";
            TestConsole.TestSubmission.TestOption.SelectionOptionTab();
            TestConsole.TestSubmission.TestOption.TestQuote.SelectionQuote();
            TestConsole.TestSubmission.SelectQuoteSheetForPrinting();

            TestConsole.TestSubmission.SubmissionContext = "S:1-O:3-V:1-Q:1";
            TestConsole.TestSubmission.TestOption.SelectionOptionTab();
            TestConsole.TestSubmission.TestOption.TestQuote.SelectionQuote();
            TestConsole.TestSubmission.SelectQuoteSheetForPrinting();

            TestConsole.TestSubmission.SubmissionContext = "S:1-O:4-V:1-Q:1";
            TestConsole.TestSubmission.TestOption.SelectionOptionTab();
            TestConsole.TestSubmission.TestOption.SelectionVersion();
            TestConsole.TestSubmission.TestOption.TestQuote.SelectionQuote();
            TestConsole.TestSubmission.SelectQuoteSheetForPrinting();

            TestConsole.TestSubmission.SubmissionContext = "S:1-O:4-V:2-Q:1";
            TestConsole.TestSubmission.TestOption.SelectionOptionTab();
            TestConsole.TestSubmission.TestOption.SelectionVersion();
            TestConsole.TestSubmission.TestOption.TestQuote.SelectionQuote();
            TestConsole.TestSubmission.SelectQuoteSheetForPrinting(select: false);

            TestConsole.TestSubmission.SubmissionContext = "S:1-O:5-V:1-Q:1";
            TestConsole.TestSubmission.TestOption.SelectionOptionTab();
            TestConsole.TestSubmission.TestOption.TestQuote.SelectionQuote();
            TestConsole.TestSubmission.SelectQuoteSheetForPrinting();
            TestConsole.TestSubmission.CreateQuoteSheet();

            #endregion
            #region assert
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:1-V:1-Q:1";
            TestConsole.TestSubmission.VerifyFieldStatus(
                                                                   cancelEnabled: true,
                                                                   saveEnabled: true,
                                                                   printQuoteEnabled: true);
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                    addOptionEnabled: true,
                                                                    copyOptionEnabled: true,
                                                                    newVersionEnabled: true,
                                                                    addNewQuote: false);
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: false,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:1-V:1-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: false,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:2-V:1-Q:1";
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                   addOptionEnabled: true,
                                                                   copyOptionEnabled: true,
                                                                   newVersionEnabled: true,
                                                                   addNewQuote: false
                                                                   );
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: false,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:2-V:1-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: false,
                                                                    slaveUpdated: true);

            TestConsole.TestSubmission.SubmissionContext = "S:1-O:3-V:1-Q:1";
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                    addOptionEnabled: true,
                                                                    copyOptionEnabled: true,
                                                                    newVersionEnabled: true,
                                                                    addNewQuote: false
                                                                    );
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: false,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:3-V:1-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: false,
                                                                    slaveUpdated: true);

            TestConsole.TestSubmission.SubmissionContext = "S:1-O:4-V:1-Q:1";
            TestConsole.TestSubmission.TestOption.SelectionOptionTab();
            TestConsole.SendMessage("Change to version");
            TestConsole.TestSubmission.TestOption.SelectionVersion();
            TestConsole.TestSubmission.TestOption.TestQuote.SelectionQuote();
            TestConsole.SendMessage("Changed to version");
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                    addOptionEnabled: true,
                                                                    copyOptionEnabled: true,
                                                                    newVersionEnabled: true,
                                                                    addNewQuote: false);
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: false,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:4-V:1-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: false,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:4-V:1-Q:3";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: false,
                                                                    slaveUpdated: true);

            TestConsole.TestSubmission.SubmissionContext = "S:1-O:4-V:2-Q:1";
            TestConsole.TestSubmission.TestOption.SelectionOptionTab();
            TestConsole.SendMessage("Change to version");
            TestConsole.TestSubmission.TestOption.SelectionVersion();
            TestConsole.TestSubmission.TestOption.TestQuote.SelectionQuote();
            TestConsole.SendMessage("Changed to version");
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                    addOptionEnabled: true,
                                                                    copyOptionEnabled: true,
                                                                    newVersionEnabled: false,
                                                                    addNewQuote: false);
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:4-V:2-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:4-V:2-Q:3";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: true,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);


            TestConsole.TestSubmission.SubmissionContext = "S:1-O:5-V:1-Q:1";
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                    addOptionEnabled: true,
                                                                    copyOptionEnabled: true,
                                                                    newVersionEnabled: true,
                                                                    addNewQuote: false);
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: false,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:5-V:1-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: false,
                                                                    slaveUpdated: true);

            #endregion
            //Final context,
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:5-V:1-Q:1";
        }

        [TestMethod]
        [TestCategory("DontRunInContinualIntegration")]
        public void Test17_Create_a_new_Version_of_Option_4_Version_1()
        {
            Test16_Create_Quote_Sheet_for_Printing_Including_Only_Options();

            #region Create a new Version of Option 4 Version 1
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:4-V:1-Q:1";
            TestConsole.TestSubmission.TestOption.SelectionOptionTab();
            TestConsole.TestSubmission.TestOption.SelectionVersion();
            TestConsole.TestSubmission.TestOption.TestQuote.SelectionQuote();
            TestConsole.TestSubmission.TestOption.AddVersion();
            #endregion
            #region assert
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:1-V:1-Q:1";
            TestConsole.TestSubmission.VerifyFieldStatus(
                                                                   cancelEnabled: true,
                                                                   saveEnabled: true,
                                                                   printQuoteEnabled: true);
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                    addOptionEnabled: true,
                                                                    copyOptionEnabled: true,
                                                                    newVersionEnabled: true,
                                                                    addNewQuote: false);
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: false,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:1-V:1-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: false,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:2-V:1-Q:1";
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                   addOptionEnabled: true,
                                                                   copyOptionEnabled: true,
                                                                   newVersionEnabled: true,
                                                                   addNewQuote: false
                                                                   );
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: false,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:2-V:1-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: false,
                                                                    slaveUpdated: true);

            TestConsole.TestSubmission.SubmissionContext = "S:1-O:3-V:1-Q:1";
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                    addOptionEnabled: true,
                                                                    copyOptionEnabled: true,
                                                                    newVersionEnabled: true,
                                                                    addNewQuote: false
                                                                    );
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: false,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:3-V:1-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: false,
                                                                    slaveUpdated: true);

            TestConsole.TestSubmission.SubmissionContext = "S:1-O:4-V:1-Q:1";
            TestConsole.TestSubmission.TestOption.SelectionOptionTab();
            TestConsole.SendMessage("Change to version");
            TestConsole.TestSubmission.TestOption.SelectionVersion();
            TestConsole.TestSubmission.TestOption.TestQuote.SelectionQuote();
            TestConsole.SendMessage("Changed to version");
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                    addOptionEnabled: true,
                                                                    copyOptionEnabled: true,
                                                                    newVersionEnabled: true,
                                                                    addNewQuote: false);
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: false,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:4-V:1-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: false,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:4-V:1-Q:3";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: false,
                                                                    slaveUpdated: true);

            TestConsole.TestSubmission.SubmissionContext = "S:1-O:4-V:2-Q:1";
            TestConsole.TestSubmission.TestOption.SelectionOptionTab();
            TestConsole.SendMessage("Change to version");
            TestConsole.TestSubmission.TestOption.SelectionVersion();
            TestConsole.TestSubmission.TestOption.TestQuote.SelectionQuote();
            TestConsole.SendMessage("Changed to version");
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                    addOptionEnabled: true,
                                                                    copyOptionEnabled: true,
                                                                    newVersionEnabled: false,
                                                                    addNewQuote: false);
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:4-V:2-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:4-V:2-Q:3";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: true,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);
            //---

            TestConsole.TestSubmission.SubmissionContext = "S:1-O:4-V:3-Q:1";
            TestConsole.TestSubmission.TestOption.SelectionOptionTab();
            TestConsole.SendMessage("Change to version");
            TestConsole.TestSubmission.TestOption.SelectionVersion();
            TestConsole.TestSubmission.TestOption.TestQuote.SelectionQuote();
            TestConsole.SendMessage("Changed to version");
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                    addOptionEnabled: true,
                                                                    copyOptionEnabled: true,
                                                                    newVersionEnabled: false,
                                                                    addNewQuote: false);
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:4-V:3-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:4-V:3-Q:3";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);
            //--

            TestConsole.TestSubmission.SubmissionContext = "S:1-O:5-V:1-Q:1";
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                    addOptionEnabled: true,
                                                                    copyOptionEnabled: true,
                                                                    newVersionEnabled: true,
                                                                    addNewQuote: false);
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: false,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:5-V:1-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: false,
                                                                    slaveUpdated: true);

            #endregion
            //Final context,
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:4-V:1-Q:1";

        }

        [TestMethod]
        [TestCategory("DontRunInContinualIntegration")]
        public void Test18_SaveSubmission()
        {
            Test17_Create_a_new_Version_of_Option_4_Version_1();

            #region save
            TestConsole.SendMessage("Save the Submission");
            TestConsole.TestSubmission.SaveSubmission();
            #endregion

            #region assert
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:1-V:1-Q:1";
            TestConsole.TestSubmission.VerifyFieldStatus(
                                                                   cancelEnabled: true,
                                                                   saveEnabled: true,
                                                                   printQuoteEnabled: true);
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                    addOptionEnabled: true,
                                                                    copyOptionEnabled: true,
                                                                    newVersionEnabled: true,
                                                                    addNewQuote: false);
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: false,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:1-V:1-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: false,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:2-V:1-Q:1";
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                   addOptionEnabled: true,
                                                                   copyOptionEnabled: true,
                                                                   newVersionEnabled: true,
                                                                   addNewQuote: false
                                                                   );
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: false,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:2-V:1-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: false,
                                                                    slaveUpdated: true);

            TestConsole.TestSubmission.SubmissionContext = "S:1-O:3-V:1-Q:1";
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                    addOptionEnabled: true,
                                                                    copyOptionEnabled: true,
                                                                    newVersionEnabled: true,
                                                                    addNewQuote: false
                                                                    );
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: false,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:3-V:1-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: false,
                                                                    slaveUpdated: true);

            TestConsole.TestSubmission.SubmissionContext = "S:1-O:4-V:1-Q:1";
            TestConsole.TestSubmission.TestOption.SelectionOptionTab();
            TestConsole.SendMessage("Change to version");
            TestConsole.TestSubmission.TestOption.SelectionVersion();
            TestConsole.TestSubmission.TestOption.TestQuote.SelectionQuote();
            TestConsole.SendMessage("Changed to version");
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                    addOptionEnabled: true,
                                                                    copyOptionEnabled: true,
                                                                    newVersionEnabled: true,
                                                                    addNewQuote: false);
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: false,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:4-V:1-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: false,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:4-V:1-Q:3";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: false,
                                                                    slaveUpdated: true);

            TestConsole.TestSubmission.SubmissionContext = "S:1-O:4-V:2-Q:1";
            TestConsole.TestSubmission.TestOption.SelectionOptionTab();
            TestConsole.SendMessage("Change to version");
            TestConsole.TestSubmission.TestOption.SelectionVersion();
            TestConsole.TestSubmission.TestOption.TestQuote.SelectionQuote();
            TestConsole.SendMessage("Changed to version");
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                    addOptionEnabled: true,
                                                                    copyOptionEnabled: true,
                                                                    newVersionEnabled: false,
                                                                    addNewQuote: false);
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:4-V:2-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:4-V:2-Q:3";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: true,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);
            //---

            TestConsole.TestSubmission.SubmissionContext = "S:1-O:4-V:3-Q:1";
            TestConsole.TestSubmission.TestOption.SelectionOptionTab();
            TestConsole.SendMessage("Change to version");
            TestConsole.TestSubmission.TestOption.SelectionVersion();
            TestConsole.TestSubmission.TestOption.TestQuote.SelectionQuote();
            TestConsole.SendMessage("Changed to version");
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                    addOptionEnabled: true,
                                                                    copyOptionEnabled: true,
                                                                    newVersionEnabled: false,
                                                                    addNewQuote: false);
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:4-V:3-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:4-V:3-Q:3";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: true,
                                                                    slaveUpdated: true);
            //--

            TestConsole.TestSubmission.SubmissionContext = "S:1-O:5-V:1-Q:1";
            TestConsole.TestSubmission.TestOption.VerifyFieldStatus(
                                                                    addOptionEnabled: true,
                                                                    copyOptionEnabled: true,
                                                                    newVersionEnabled: true,
                                                                    addNewQuote: false);
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: false,
                                                                    slaveUpdated: true);
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:5-V:1-Q:2";
            TestConsole.TestSubmission.TestOption.TestQuote.VerifyFieldStatus(
                                                                    cobFieldEnabled: false,
                                                                    masterFieldsEnabled: false,
                                                                    slaveFieldsEnabled: false,
                                                                    slaveUpdated: true);

            #endregion
            //Final context,
            TestConsole.TestSubmission.SubmissionContext = "S:1-O:4-V:1-Q:1";

        }

        [TestMethod]
        [TestCategory("DontRunInContinualIntegration")]
        public void TestSubmission()
        {
            Test18_SaveSubmission();

        }

    }
}
