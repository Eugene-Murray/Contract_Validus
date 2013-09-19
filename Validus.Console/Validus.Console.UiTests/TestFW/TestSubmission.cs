using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Validus.Console.UiTests.Helper;

namespace Validus.Console.UiTests.TestFW
{
    public static partial class TestConsole
    {
        public static partial class TestSubmission
        {
            public static class SubmissionData
            {
                public static string KeyInInputBoxInsuredName { get; set; }
                public static string SelectInputBoxInsuredName { get; set; }


                public static string KeyInInputBoxBrokerName { get; set; }
                public static string SelectInputBoxBrokerName { get; set; }

                public static string KeyInputBoxBrokerContact { get; set; }
                public static string SelectInputBoxBrokerContact { get; set; }

                public static string KeyInInputBoxUnderWriter { get; set; }
                public static string SelectInputBoxUnderWriter { get; set; }

                public static string KeyInInputBoxUnderwriterContact { get; set; }
                public static string SelectInputBoxUnderwriterContact { get; set; }


                public static string KeyInInputOffice { get; set; }
                public static string SelectInputOffice { get; set; }


                public static string KeyInInputBoxDomicile { get; set; }
                public static string SelectInputBoxDomicile { get; set; }


                public static string KeyInInputBoxLeader { get; set; }
                public static string SelectInputBoxLeader { get; set; }

                public static string KeyInInputBoxBrokerage { get; set; }
            }

            public static string SubmissionContext { get; set; }

            public static string Context2String()
            {
                return SubmissionContext.Replace(":", "z").Replace("-", "X");

            }


            static TestSubmission()
            {
                SubmissionContext = "S:1-O:1-V:1-Q:1";
            }

            #region path
            public static string GetSubmissionPath
            {
                get
                {
                    switch (SubmissionContext.Split("-".ToCharArray())[0])
                    {
                        case "S:1":
                            return @"//*[@id=""tab2-_submission__template""]/form/fieldset";
                        default:
                            throw new Exception(string.Format("Path not defined {0}",
                                   SubmissionContext.Split("-".ToCharArray())[0]));

                    }
                }
            }
            #endregion

            #region button to iniitate new submission
            public static IWebElement ButtonHome
            {
                get { return WebDriver.FindElement(By.XPath(@"//*[@id=""tab1-home""]/div[1]/div[1]/div/button"), LongWait3); }
            }
            public static IWebElement ButtonNewSubmission
            {
                get { return WebDriver.FindElement(By.XPath(@"//*[@id=""tab1-home""]/div[1]/div[1]/div/ul/li[1]/a")); }
            }
            #endregion

            #region input controls for submission
            public static IWebElement InputBoxInsuredName
            {
                get { return WebDriver.FindElement(By.XPath(GetSubmissionPath + @"/div[2]/div[1]/div[1]/div/input"), LongWait3); }
            }
            public static IWebElement InputBoxBrokerName
            {
                get { return WebDriver.FindElement(By.XPath(GetSubmissionPath + @"/div[2]/div[1]/div[2]/div/input")); }
            }
            public static IWebElement InputBoxBrokerContact
            {
                get { return WebDriver.FindElement(By.XPath(GetSubmissionPath + @"/div[2]/div[1]/div[3]/div[1]/input")); }
            }
            public static IWebElement InputBoxUnderWriter
            {
                get { return WebDriver.FindElement(By.XPath(GetSubmissionPath + @"/div[2]/div[1]/div[4]/div[1]/input")); }
            }
            public static IWebElement InputBoxUnderwriterContact
            {
                get { return WebDriver.FindElement(By.XPath(GetSubmissionPath + @"/div[2]/div[1]/div[4]/div[2]/input")); }
            }
            public static IWebElement InputOffice
            {
                get { return WebDriver.FindElement(By.XPath(GetSubmissionPath + @"/div[2]/div[1]/div[5]/div[1]/input")); }
            }
            public static IWebElement InputBoxDomicile
            {
                get { return WebDriver.FindElement(By.XPath(GetSubmissionPath + @"/div[2]/div[1]/div[5]/div[2]/input")); }
            }
            public static IWebElement InputBoxLeader
            {
                get { return WebDriver.FindElement(By.XPath(GetSubmissionPath + @"/div[2]/div[1]/div[5]/div[3]/input")); }
            }
            public static IWebElement InputBoxBrokerage
            {
                get { return WebDriver.FindElement(By.XPath(GetSubmissionPath + @"/div[2]/div[1]/div[5]/div[4]/input")); }
            }

            #endregion

            public static void EnterSubmissionDetails()
            {
                //wait before you start.


                Action a = null;
                a = () => InputBoxInsuredName.Clear();
                a.Retry();

                InputBoxInsuredName.SendKeys(SubmissionData.KeyInInputBoxInsuredName);
                //AjaxWait();
                a = () => WebDriver.FindElement(
                   By.CssSelector(GetTypeAheadCssSelector(SubmissionData.SelectInputBoxInsuredName)), LongWait3).Click();
                a.Retry();

                InputBoxBrokerName.Clear();
                InputBoxBrokerName.SendKeys(SubmissionData.KeyInInputBoxBrokerName);
                //AjaxWait();
                a = () => WebDriver.FindElement(By.CssSelector(GetTypeAheadCssSelector(SubmissionData.SelectInputBoxBrokerName)), LongWait3).Click();
                a.Retry();

                InputBoxBrokerContact.Clear();
                InputBoxBrokerContact.SendKeys(SubmissionData.KeyInputBoxBrokerContact);
                //AjaxWait();
                a = () => WebDriver.FindElement(By.CssSelector(GetTypeAheadCssSelector(SubmissionData.SelectInputBoxBrokerContact)), LongWait3).Click();
                a.Retry();

                InputBoxUnderWriter.Clear();
                InputBoxUnderWriter.SendKeys(SubmissionData.KeyInInputBoxUnderWriter);
                //AjaxWait();
                a = () => WebDriver.FindElement(By.CssSelector(GetTypeAheadCssSelector(SubmissionData.SelectInputBoxUnderWriter)), LongWait3).Click();
                a.Retry();

                InputBoxUnderwriterContact.Clear();
                InputBoxUnderwriterContact.SendKeys(SubmissionData.KeyInInputBoxUnderwriterContact);
                //AjaxWait();
                a = () => WebDriver.FindElement(By.CssSelector(GetTypeAheadCssSelector(SubmissionData.KeyInInputBoxUnderwriterContact)), LongWait3).Click();
                a.Retry();

                InputOffice.Clear();
                InputOffice.SendKeys(SubmissionData.KeyInInputOffice);
                //AjaxWait();
                a = () => WebDriver.FindElement(By.CssSelector(GetTypeAheadCssSelector(SubmissionData.KeyInInputOffice)), LongWait3).Click();
                a.Retry();

                InputBoxDomicile.Clear();
                InputBoxDomicile.SendKeys(SubmissionData.KeyInInputBoxDomicile);
                //AjaxWait();
                a = () => WebDriver.FindElement(By.CssSelector(GetTypeAheadCssSelector(SubmissionData.SelectInputBoxDomicile)), LongWait3).Click();
                a.Retry();

                InputBoxLeader.Clear();
                InputBoxLeader.SendKeys(SubmissionData.KeyInInputBoxLeader);
                //AjaxWait();
                a = () => WebDriver.FindElement(By.CssSelector(GetTypeAheadCssSelector(SubmissionData.KeyInInputBoxLeader)), LongWait3).Click();
                a.Retry();

                InputBoxBrokerage.Clear();
                InputBoxBrokerage.SendKeys(SubmissionData.KeyInInputBoxBrokerage);
            }


            public static IWebElement ButtonCanceSubmission
            {
                get
                {
                    var buttonSaveSubmission = By.XPath(GetSubmissionPath + @"/div[4]/div/div/span/button[1]");
                    return WebDriver.FindElement(buttonSaveSubmission);
                }
            }

            public static IWebElement ButtonSaveSubmission
            {
                get
                {
                    var buttonSaveSubmission = By.XPath(GetSubmissionPath + @"/div[4]/div/div/span/button[2]");
                    return WebDriver.FindElement(buttonSaveSubmission);
                }
            }

            public static void SaveSubmission()
            {
                ButtonSaveSubmission.Click();
                AjaxWait();
            }


            public static IWebElement ButtonPrintQuoteSheet
            {
                get
                {
                    var buttonCreateQuoteSheet = By.XPath(GetSubmissionPath + @"/div[4]/div/div/span/button[3]");
                    return WebDriver.FindElement(buttonCreateQuoteSheet);
                }
            }

            public static void CreateQuoteSheet()
            {
                ButtonPrintQuoteSheet.Click();
                AjaxWait();

            }

            public static void SelectQuoteSheetForPrinting(bool select = true)
            {
                var quoteSheetToSelect = string.Format("Option {0} v{1}", SubmissionContext.Split("-".ToCharArray())[1].Split(
                                                           ":".ToCharArray())[1],
                                                       SubmissionContext.Split("-".ToCharArray())[2].Split(
                                                           ":".ToCharArray())[1]);

                var tableSubmissionHeadder = By.XPath(GetSubmissionPath + @"/div[5]/div/table/thead/tr[1]/th");
                var tableSubmissionData = By.XPath(GetSubmissionPath + @"/div[5]/div/table/thead/tr[2]/th");

                var index = 0;
                var i = 0;
                foreach (var webElement in WebDriver.FindElements(tableSubmissionHeadder))
                {
                    i++;
                    var spans = webElement.FindElements(By.XPath(@"./span")).FirstOrDefault();
                    if (spans != null && spans.Text == quoteSheetToSelect)
                    {
                        index = i;
                        break;
                    }
                }

                var j = 0;

                foreach (var webElement in WebDriver.FindElements(tableSubmissionData))
                {
                    j++;
                    if (select)
                    {
                        if (j == index)
                        {
                            var inputs = webElement.FindElements(By.XPath(@"./input")).FirstOrDefault();
                            if (inputs != null && inputs.GetAttribute("checked") != null && !inputs.GetAttribute("checked").Equals("true"))
                            {
                                webElement.FindElement(By.XPath(@"./input")).Click();
                            }
                        }
                    }
                    else
                    {
                        if (j == i)
                        {
                            var inputs = webElement.FindElements(By.XPath(@"./input")).FirstOrDefault();
                            if (inputs != null && inputs.GetAttribute("checked") != null && inputs.GetAttribute("checked").Equals("true"))
                            {
                                webElement.FindElement(By.XPath(@"./input")).Click();
                            }
                        }
                    }

                }

            }

            public static void VerifyFieldStatus(
                    bool cancelEnabled,
                    bool saveEnabled,
                    bool printQuoteEnabled
                    )
            {
                //Cancel_Enabled	
                if (ButtonCanceSubmission.Enabled ^ cancelEnabled) throw new Exception("Cancel_Enabled failed status verification");
                //Save_Enabled
                if (ButtonSaveSubmission.Enabled ^ saveEnabled) throw new Exception("Save_Enabled failed status verification");
                //Print_Quote_Enabled (Includes Dirty Save)	
                if (ButtonPrintQuoteSheet.Enabled ^ printQuoteEnabled) throw new Exception("Print_Quote_Enabled failed status verification");
                //Add_Option_Enabled	


            }
        }
    }
}
