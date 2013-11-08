using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace Validus.Console.UiTests.TestFW
{
    public static partial class TestConsole
    {
        public static partial class TestSubmission
        {
            public static partial class TestOption
            {


                public static string GetOptionPath
                {
                    get
                    {

                        switch (SubmissionContext.Split("-".ToCharArray())[0] + "-" + SubmissionContext.Split("-".ToCharArray())[1])
                        {
                            case "S:1-O:1":
                                return @"//*[@id=""tab2-_submission__template-option0""]";
                            case "S:1-O:2":
                                return @"//*[@id=""tab2-_submission__template-option1""]";
                            case "S:1-O:3":
                                return @"//*[@id=""tab2-_submission__template-option2""]";
                            case "S:1-O:4":
                                return @"//*[@id=""tab2-_submission__template-option3""]";
                            case "S:1-O:5":
                                return @"//*[@id=""tab2-_submission__template-option4""]";
                            default:
                                throw new Exception(string.Format("Path not defined {0}", SubmissionContext.Split(" - ".ToCharArray())[0] + " - " + SubmissionContext.Split(" - ".ToCharArray())[1]));
                        }

                    }
                }


                public static IWebElement ButtonAddOption
                {
                    get
                    {
                        string strbuttonAddOption;

                        switch (SubmissionContext.Split("-".ToCharArray())[1])
                        {
                            case "O:1":
                                strbuttonAddOption =
                                  GetSubmissionPath + @"/div[3]/div/div/ul/li[2]/a";

                                break;
                            case "O:2":
                                strbuttonAddOption =
                                  GetSubmissionPath + @"/div[3]/div/div/ul/li[3]/a";
                                break;
                            case "O:3":
                                strbuttonAddOption =
                                  GetSubmissionPath + @"/div[3]/div/div/ul/li[4]/a";
                                break;
                            case "O:4":
                                strbuttonAddOption =
                                  GetSubmissionPath + @"/div[3]/div/div/ul/li[5]/a";
                                break;
                            case "O:5":
                                strbuttonAddOption =
                                  GetSubmissionPath + @"/div[3]/div/div/ul/li[6]/a";
                                break;
                            default:
                                throw new Exception(string.Format("Path not defined {0}",
                                    SubmissionContext.Split("-".ToCharArray())[1]));
                        }

                        var buttonAddOption =
                           By.XPath(strbuttonAddOption);
                        return WebDriver.FindElement(buttonAddOption);
                    }

                }
                public static IWebElement ButtonAddVersion
                {
                    get
                    {
                        var buttonAddVersion =
                      WebDriver.FindElement(By.XPath(GetOptionPath + @"/div[1]/div/div/button[1]"));
                        return buttonAddVersion;
                    }
                }
                public static IWebElement ButtonAddQuote
                {
                    get
                    {
                        var buttonAddQuote =
                       WebDriver.FindElement(By.XPath(GetOptionPath + @"/div[1]/div/button[1]"));
                        return buttonAddQuote;
                    }
                }
                public static IWebElement ButtonCopyOption
                {
                    get
                    {
                        var strbuttonCopyOption =
                            GetOptionPath + @"/div[1]/div/button[2]";

                        var buttonCopyOption =
                           By.XPath(strbuttonCopyOption);
                        return WebDriver.FindElement(buttonCopyOption);
                    }
                }


                public static void AddVersion()
                {
                    ButtonAddVersion.Click();
                }

                public static void AddQuote()
                {
                    ButtonAddQuote.Click();
                }

                public static void CopyOption()
                {
                    ButtonCopyOption.Click();
                }

                public static void AddOption()
                {
                    ButtonAddOption.Click();
                }


                public static void SelectionOptionTab()
                {
                    string strtabOption;
                    switch (SubmissionContext.Split("-".ToCharArray())[1])
                    {
                        case "O:1":
                            strtabOption = GetSubmissionPath + @"/div[3]/div/div/ul/li[1]/a/span";
                            break;
                        case "O:2":
                            strtabOption = GetSubmissionPath + @"/div[3]/div/div/ul/li[2]/a/span";
                            break;
                        case "O:3":
                            strtabOption = GetSubmissionPath + @"/div[3]/div/div/ul/li[3]/a/span";
                            break;
                        case "O:4":
                            strtabOption = GetSubmissionPath + @"/div[3]/div/div/ul/li[4]/a/span";
                            break;
                        case "O:5":
                            strtabOption = GetSubmissionPath + @"/div[3]/div/div/ul/li[5]/a/span";
                            break;
                        default:
                            throw new Exception(string.Format("Path not defined {0}",
                                   SubmissionContext.Split("-".ToCharArray())[1]));
                    }

                    var tabOption =
                     WebDriver.FindElement(
                           By.XPath(strtabOption));
                    tabOption.Click();
                }


                public static void SelectionVersion()
                {
                    var buttonSelectVersion =
                      WebDriver.FindElement(By.XPath(GetOptionPath + @"/div[1]/div/div/button[2]/span"));
                    buttonSelectVersion.Click();

                    string strbuttonChangeVersion;
                    string strbuttonVersionName;
                    //*[@id="tab2-_submission__template-option0"]/div[1]/div/div/ul
                    //*[@id="tab2-_submission__template-option0"]/div[1]/div/div/ul/li[2]/a
                    //*[@id="tab2-_submission__template-option0"]/div[1]/div/div/ul/li[1]/a
                    switch (SubmissionContext.Split("-".ToCharArray())[2])
                    {
                        case "V:1":
                            strbuttonChangeVersion =
                              GetOptionPath + @"/div[1]/div/div/ul";
                            strbuttonVersionName = "Version 1";
                            break;
                        case "V:2":
                            strbuttonChangeVersion =
                              GetOptionPath + @"/div[1]/div/div/ul";
                            strbuttonVersionName = "Version 2";
                            break;
                        case "V:3":
                            strbuttonChangeVersion =
                              GetOptionPath + @"/div[1]/div/div/ul";
                            strbuttonVersionName = "Version 3";
                            break;
                        case "V:4":
                            strbuttonChangeVersion =
                              GetOptionPath + @"/div[1]/div/div/ul";
                            strbuttonVersionName = "Version 4";
                            break;
                        case "V:5":
                            strbuttonChangeVersion =
                              GetOptionPath + @"/div[1]/div/div/ul";
                            strbuttonVersionName = "Version 5";
                            break;
                        default:
                            throw new Exception(string.Format("Path not defined {0}",
                                SubmissionContext.Split("-".ToCharArray())[1]));
                    }

                    var buttonChangeVersion =
                       By.XPath(strbuttonChangeVersion + @"/li/a");
                    var buttonChangeVesrionLink = WebDriver.FindElements(buttonChangeVersion).FirstOrDefault(e => e.Text == strbuttonVersionName);
                    if (
                        buttonChangeVesrionLink != null)
                        buttonChangeVesrionLink.Click();
                }

                public static void VerifyFieldStatus(
                   
                     bool addOptionEnabled,
                     bool copyOptionEnabled,
                     bool newVersionEnabled,
                     bool addNewQuote
                     )
                {
                    //Add_Option_Enabled	
                    if (ButtonAddOption.Enabled ^ addOptionEnabled) throw new Exception("Add_Option_Enabled failed status verification");
                    //Copy_Option_Enabled (Only if all mandatory Option Fields on the selected Option have been completed)	
                    if (ButtonCopyOption.Enabled ^ copyOptionEnabled) throw new Exception("Copy_Option_Enabled failed status verification");
                    //New_Version_Enabled	
                    if (ButtonAddVersion.Enabled ^ newVersionEnabled) throw new Exception("New_Version_Enabled failed status verification");
                    //Add_New_Quote
                    if (ButtonAddQuote.Enabled ^ addNewQuote) throw new Exception("Add_New_Quote failed status verification");


                }
            }
        }
    }
}
