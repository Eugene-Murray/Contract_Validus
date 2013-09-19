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
            public static partial class TestOption
            {

                public static class TestQuote
                {

                    public static class QuoteMasterData
                    {
                        public static string KeyInInputBoxCob { get; set; }
                        public static string SelectInputBoxCob { get; set; }

                        public static string KeyInInputBoxMoa { get; set; }
                        public static string SelectInputBoxMoa { get; set; }

                        public static string KeyInputBoxQuotingOffice { get; set; }
                        public static string SelectInputBoxQuotingOffice { get; set; }

                    }

                    public static class QuoteSlaveData
                    {
                        public static string KeyInInputBoxCurrency { get; set; }
                        public static string SelectInputBoxCurrency { get; set; }

                        public static string KeyInInputBoxLimitCurrency { get; set; }
                        public static string SelectInputBoxLimitCurrency { get; set; }

                        public static string KeyInInputBoxExcessCurrency { get; set; }
                        public static string SelectInputBoxExcessCurrency { get; set; }

                    }

                    public static string GetMasterData()
                    {
                        return InputBoxCob.GetAttribute("value") +
                               InputBoxMoa.GetAttribute("value") +
                               InputBoxQuotingOffice.GetAttribute("value") +
                               SelectBoxPolicyType.GetAttribute("value") +
                               SelectBoxEntryStatus.GetAttribute("value") +
                               InputBoxInspectionDate.GetAttribute("value") +
                               InputBoxExpiryDate.GetAttribute("value") +
                               InputBoxAccountYear.GetAttribute("value");
                    }

                    public static string GetSlaveData()
                    {
                        return InputBoxQuoteExpiryDate.GetAttribute("value") +
                               InputBoxCurrency.GetAttribute("value") +
                               InputBoxTechnicalPremium.GetAttribute("value") +
                               InputBoxBenchmarkPremium.GetAttribute("value") +
                               SelectBoxPricingMethod.GetAttribute("value") +
                               SelectBoxBind.GetAttribute("value") +
                               SelectBoxAmt.GetAttribute("value") +
                               InputBoxLimitCurrency.GetAttribute("value") +
                               InputBoxLimitAmount.GetAttribute("value") +
                               InputBoxExcessCurrency.GetAttribute("value") +
                               InputBoxExcessAmount.GetAttribute("value") +
                               InputBoxQuotedPremium.GetAttribute("value");
                    }

                    public static string GetQuotePath
                    {
                        get
                        {
                            switch (SubmissionContext.Split("-".ToCharArray())[3])
                            {
                                case "Q:1":
                                    return GetOptionPath + @"/div[2]/div/div/div/div[1]/div[1]/div[2]";
                                case "Q:2":
                                    return GetOptionPath + @"/div[2]/div/div/div/div[2]/div[1]/div[2]";
                                case "Q:3":
                                    return GetOptionPath + @"/div[2]/div/div/div/div[3]/div[1]/div[2]";
                                default:
                                    throw new Exception(string.Format("Path not defined {0}",
                                    SubmissionContext.Split("-".ToCharArray())[3]));

                            }

                        }
                    }


                    public static void SelectionQuote()
                    {
                        string strcarouselOption;
                        switch (SubmissionContext.Split("-".ToCharArray())[3])
                        {
                            case "Q:1":
                                strcarouselOption = GetOptionPath + @"/div[2]/div/div/ol/li[1]";
                                break;
                            case "Q:2":
                                strcarouselOption = GetOptionPath + @"/div[2]/div/div/ol/li[2]";
                                break;
                            case "Q:3":
                                strcarouselOption = GetOptionPath + @"/div[2]/div/div/ol/li[3]";
                                break;
                            default:
                                throw new Exception(string.Format("Path not defined {0}",
                                    SubmissionContext.Split("-".ToCharArray())[3]));

                        }

                        var carouselOption =
                         WebDriver.FindElement(
                               By.XPath(strcarouselOption));
                        carouselOption.Click();
                    }


                    public static IWebElement StarIsPrimary
                    {
                        get
                        {
                            var strstarIsPrimary =
                                GetQuotePath + @"/div[1]/div[5]/i";

                            return WebDriver.FindElement(By.XPath(strstarIsPrimary));

                        }
                    }

                    public static IWebElement SelectBoxSubmissionStatus {
                        get
                        {
                            var strinputBoxCob =
                                GetQuotePath + @"/div[1]/div[2]/select";

                            return WebDriver.FindElement(By.XPath(strinputBoxCob));

                        }
                    }
                    public static IWebElement InputBoxFacilityReference
                    {
                        get
                        {
                            var strinputBoxCob =
                                GetQuotePath + @"/div[1]/div[3]/input";

                            return WebDriver.FindElement(By.XPath(strinputBoxCob));

                        }
                    }

                    public static IWebElement InputBoxCob
                    {
                        get
                        {
                            var strinputBoxCob =
                                GetQuotePath + @"/div[2]/div[1]/input";

                            return WebDriver.FindElement(By.XPath(strinputBoxCob));

                        }
                    }
                    public static IWebElement InputBoxMoa
                    {
                        get
                        {
                            var strinputBoxMoa =
                                GetQuotePath + @"/div[2]/div[2]/input";
                            return WebDriver.FindElement(By.XPath(strinputBoxMoa));

                        }
                    }
                    public static IWebElement InputBoxQuotingOffice
                    {
                        get
                        {
                            var strinputBoxMoa =
                                GetQuotePath + @"/div[2]/div[3]/input";
                            return WebDriver.FindElement(By.XPath(strinputBoxMoa));

                        }
                    }
                    public static IWebElement SelectBoxPolicyType
                    {
                        get
                        {
                            var strinputBoxCob =
                                GetQuotePath + @"/div[2]/div[4]/select";

                            return WebDriver.FindElement(By.XPath(strinputBoxCob));

                        }
                    }
                    public static IWebElement SelectBoxEntryStatus
                    {
                        get
                        {
                            var strinputBoxCob =
                                GetQuotePath + @"/div[2]/div[5]/select";

                            return WebDriver.FindElement(By.XPath(strinputBoxCob));

                        }
                    }

                    public static IWebElement InputBoxInspectionDate
                    {
                        get
                        {
                            var strinputBoxInspectionDate = 
                                GetQuotePath + @"/div[3]/div[1]/input";
                            return WebDriver.FindElement(By.XPath(strinputBoxInspectionDate));
                        }
                    }
                    public static IWebElement ButtonBoxInspectionDate
                    {
                        get
                        {
                            var strinputBoxInspectionDate =
                                GetQuotePath + @"/div[3]/div[1]/button";
                            return WebDriver.FindElement(By.XPath(strinputBoxInspectionDate));
                        }
                    }
                    public static IWebElement InputBoxExpiryDate
                    {
                        get
                        {
                            var strinputBoxExpiryDate = 
                                GetQuotePath + @"/div[3]/div[2]/input";
                            return WebDriver.FindElement(By.XPath(strinputBoxExpiryDate));
                        }
                    }
                    public static IWebElement ButtonBoxExpiryDate
                    {
                        get
                        {
                            var xpath =
                                GetQuotePath + @"/div[3]/div[2]/button";
                            return WebDriver.FindElement(By.XPath(xpath));
                        }
                    }
                    public static IWebElement InputBoxAccountYear
                    {
                        get
                        {
                            var strinputBoxExpiryDate =
                                GetQuotePath + @"/div[3]/div[3]/input";
                            return WebDriver.FindElement(By.XPath(strinputBoxExpiryDate));
                        }
                    }
                  
                 
                    public static IWebElement InputBoxQuoteExpiryDate
                    {
                        get
                        {
                            var strinputBoxQuoteExpiryDate = 
                                GetQuotePath + @"/div[3]/div[4]/input";
                            return WebDriver.FindElement(By.XPath(strinputBoxQuoteExpiryDate));
                        }
                    }
                    public static IWebElement ButtonBoxQuoteExpiryDate
                    {
                        get
                        {
                            var xpath =
                                GetQuotePath + @"/div[3]/div[4]/button";
                            return WebDriver.FindElement(By.XPath(xpath));
                        }
                    }

                    public static IWebElement InputBoxCurrency
                    {
                        get
                        {
                            var xpath = 
                                GetQuotePath + @"/div[4]/div[1]/input";
                            return WebDriver.FindElement(By.XPath(xpath));
                        }
                    }
                    public static IWebElement InputBoxTechnicalPremium
                    {
                        get
                        {
                            var xpath =
                                GetQuotePath + @"/div[4]/div[2]/input";
                            return WebDriver.FindElement(By.XPath(xpath));
                        }
                    }
                    public static IWebElement InputBoxBenchmarkPremium
                    {
                        get
                        {
                            var xpath =
                                GetQuotePath + @"/div[4]/div[3]/input";
                            return WebDriver.FindElement(By.XPath(xpath));
                        }
                    }
                    public static IWebElement SelectBoxPricingMethod
                    {
                        get
                        {
                            var xpath =
                                GetQuotePath + @"/div[4]/div[4]/select";
                            return WebDriver.FindElement(By.XPath(xpath));
                        }
                    }
                    public static IWebElement SelectBoxBind
                    {
                        get
                        {
                            var xpath =
                                GetQuotePath + @"/div[4]/div[5]/select";
                            return WebDriver.FindElement(By.XPath(xpath));
                        }
                    }
                    public static IWebElement SelectBoxAmt
                    {
                        get
                        {
                            var xpath =
                                GetQuotePath + @"/div[4]/div[6]/select";
                            return WebDriver.FindElement(By.XPath(xpath));
                        }
                    }



                    public static IWebElement InputBoxLimitCurrency
                    {
                        get
                        {
                            var strinputBoxLimitCurrency = 
                                GetQuotePath + @"/div[5]/div[1]/input";
                            return WebDriver.FindElement(By.XPath(strinputBoxLimitCurrency));
                        }
                    }
                    public static IWebElement InputBoxLimitAmount
                    {
                        get
                        {
                            var xpath =
                                GetQuotePath + @"/div[5]/div[2]/input";
                            return WebDriver.FindElement(By.XPath(xpath));
                        }
                    }
                    public static IWebElement InputBoxExcessCurrency
                    {
                        get
                        {
                            var strinputBoxExcessCurrency = 
                                GetQuotePath + @"/div[5]/div[3]/input";
                            return WebDriver.FindElement(By.XPath(strinputBoxExcessCurrency));
                        }
                    }
                    public static IWebElement InputBoxExcessAmount
                    {
                        get
                        {
                            var xpath =
                                GetQuotePath + @"/div[5]/div[4]/input";
                            return WebDriver.FindElement(By.XPath(xpath));
                        }
                    }
                    public static IWebElement InputBoxQuotedPremium
                    {
                        get
                        {
                            var xpath =
                                GetQuotePath + @"/div[5]/div[5]/input";
                            return WebDriver.FindElement(By.XPath(xpath));
                        }
                    }

                    public static void EnterQuoteDetails_Master(bool exculdeCob=false)
                    {
                        Action a = null;
                        if (!exculdeCob)
                        {
                            InputBoxCob.Clear();
                            InputBoxCob.SendKeys(QuoteMasterData.KeyInInputBoxCob);
                            //AjaxWait();
                            a =
                                () =>
                                WebDriver.FindElement(
                                    By.CssSelector(GetTypeAheadCssSelector(QuoteMasterData.SelectInputBoxCob)),
                                    LongWait3).Click();
                            a.Retry();
                        }
                        InputBoxMoa.Clear();
                        InputBoxMoa.SendKeys(QuoteMasterData.KeyInInputBoxMoa);
                        //AjaxWait();
                        a = () => WebDriver.FindElement(By.CssSelector(GetTypeAheadCssSelector(QuoteMasterData.SelectInputBoxMoa)), LongWait3).Click();
                        a.Retry();

                        InputBoxQuotingOffice.Clear();
                        InputBoxQuotingOffice.SendKeys(QuoteMasterData.KeyInputBoxQuotingOffice);
                        //AjaxWait();
                        a = () => WebDriver.FindElement(By.CssSelector(GetTypeAheadCssSelector(QuoteMasterData.SelectInputBoxQuotingOffice)), LongWait3).Click();
                        a.Retry();

                        InputBoxInspectionDate.Click();
                        InputBoxInspectionDate.SendKeys(Keys.Enter);

                        InputBoxExpiryDate.Click();
                        InputBoxExpiryDate.SendKeys(Keys.Enter);

                        ((IJavaScriptExecutor)WebDriver).ExecuteScript("arguments[0].removeAttribute('readonly')", InputBoxQuoteExpiryDate);
                        var expiryDate = DateTime.Now.Add(new TimeSpan(150, 0, 0, 0)).ToString("yyyy-MM-dd");
                        InputBoxQuoteExpiryDate.Clear();
                        InputBoxQuoteExpiryDate.SendKeys(expiryDate);
                        InputBoxQuoteExpiryDate.Click();
                        InputBoxQuoteExpiryDate.SendKeys(Keys.Enter);
                        ((IJavaScriptExecutor)WebDriver).ExecuteScript("arguments[0].setAttribute('readonly','readonly')", InputBoxQuoteExpiryDate);
                        
                    }

                    public static void EnterQuoteDetails_Slave()
                    {
                        Action a = null;

                        InputBoxCurrency.Clear();
                        InputBoxCurrency.SendKeys(QuoteSlaveData.KeyInInputBoxCurrency);
                        //AjaxWait();
                        a = () => WebDriver.FindElement(By.CssSelector(GetTypeAheadCssSelector(QuoteSlaveData.SelectInputBoxCurrency)), LongWait3).Click();
                        a.Retry();

                        InputBoxLimitCurrency.Clear();
                        InputBoxLimitCurrency.SendKeys(QuoteSlaveData.KeyInInputBoxLimitCurrency);
                        //AjaxWait();
                        a = () => WebDriver.FindElement(By.CssSelector(GetTypeAheadCssSelector(QuoteSlaveData.SelectInputBoxLimitCurrency)), LongWait3).Click();
                        a.Retry();

                        InputBoxExcessCurrency.Clear();
                        InputBoxExcessCurrency.SendKeys(QuoteSlaveData.KeyInInputBoxExcessCurrency);
                        //AjaxWait();
                        a = () => WebDriver.FindElement(By.CssSelector(GetTypeAheadCssSelector(QuoteSlaveData.SelectInputBoxExcessCurrency)), LongWait3).Click();
                        a.Retry();
                    }

                    public static string GetSubscribeReference()
                    {
                        var inputBoxSubscribeReference = By.XPath(GetQuotePath + @"/div[1]/div[1]/input");
                        var subscribeReference = WebDriver.FindElement(inputBoxSubscribeReference).GetAttribute("value");
                        return subscribeReference;

                    }



                    public static void VerifyFieldStatus(
                        bool cobFieldEnabled,
                        bool masterFieldsEnabled,
                        bool slaveFieldsEnabled,
                        bool slaveUpdated
                        )
                    {


                        //COB_field_Enabled	
                        if (InputBoxCob.Enabled ^ cobFieldEnabled) //throw new Exception("COB_field_Enabled failed status verification");
                        if (InputBoxCob.GetAttribute("readonly")==null || false ^ cobFieldEnabled) throw new Exception("COB_field_Enabled failed status verification");
                        //Master_Fields_Enabled	
                        if (InputBoxMoa.Enabled ^ masterFieldsEnabled) throw new Exception("Master_Fields_Enabled failed status verification");
                        if (InputBoxQuotingOffice.Enabled ^ masterFieldsEnabled) throw new Exception("Master_Fields_Enabled failed status verification");
                        if (SelectBoxPolicyType.Enabled ^ masterFieldsEnabled) throw new Exception("Master_Fields_Enabled failed status verification");
                        if (SelectBoxEntryStatus.Enabled ^ masterFieldsEnabled) throw new Exception("Master_Fields_Enabled failed status verification");

                        if (ButtonBoxInspectionDate.Enabled ^ masterFieldsEnabled) throw new Exception("Master_Fields_Enabled failed status verification");
                        if (ButtonBoxExpiryDate.Enabled ^ masterFieldsEnabled) throw new Exception("Master_Fields_Enabled failed status verification");
                        if (InputBoxAccountYear.Enabled ^ masterFieldsEnabled) throw new Exception("Master_Fields_Enabled failed status verification");
                       
                        //Slave_fields_Enabled	
                        if (ButtonBoxQuoteExpiryDate.Enabled ^ slaveFieldsEnabled) throw new Exception("Slave Fields Enabled status verification");

                        if (InputBoxCurrency.Enabled ^ slaveFieldsEnabled) throw new Exception("Slave Fields Enabled status verification");
                        if (InputBoxTechnicalPremium.Enabled ^ slaveFieldsEnabled) throw new Exception("Slave Fields Enabled status verification");
                        if (InputBoxBenchmarkPremium.Enabled ^ slaveFieldsEnabled) throw new Exception("Slave Fields Enabled status verification");
                        if (SelectBoxPricingMethod.Enabled ^ slaveFieldsEnabled) throw new Exception("Slave Fields Enabled status verification");
                        if (SelectBoxBind.Enabled ^ slaveFieldsEnabled) throw new Exception("Slave Fields Enabled status verification");
                        if (SelectBoxAmt.Enabled ^ slaveFieldsEnabled) throw new Exception("Slave Fields Enabled status verification");

                        if (InputBoxLimitCurrency.Enabled ^ slaveFieldsEnabled) throw new Exception("Slave Fields Enabled status verification");
                        if (InputBoxLimitAmount.Enabled ^ slaveFieldsEnabled) throw new Exception("Slave Fields Enabled status verification");
                        if (InputBoxExcessCurrency.Enabled ^ slaveFieldsEnabled) throw new Exception("Slave Fields Enabled status verification");
                        if (InputBoxExcessAmount.Enabled ^ slaveFieldsEnabled) throw new Exception("Slave Fields Enabled status verification");
                        if (InputBoxQuotedPremium.Enabled ^ slaveFieldsEnabled) throw new Exception("Slave Fields Enabled status verification");
                        //Slave_Updated	
                    }

                }

            }
        }
    }

}
