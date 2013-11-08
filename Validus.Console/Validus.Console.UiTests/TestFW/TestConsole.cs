using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using CassiniDev;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using Validus.Console.UiTests.Helper;
using Keys = OpenQA.Selenium.Keys;

namespace Validus.Console.UiTests.TestFW
{
    public static partial class TestConsole
    {
        public const int Wait = 1000;
        public const int LongWait = 2000;
        public const int LongWait2 = 20000;
        public const int LongWait3 = 30000;
        public const int VeryLongWait5 = 50000;
        public const int VeryLongWait8 = 80000;
        private const string ApplicationName = "Validus.Console";
        const int Port = 2020;
        private static Process _iisProcess;
        public static readonly IWebDriver WebDriver;
        private static Desktop _userDesktop;
        
        static TestConsole()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments("--disable-web-security");
            chromeOptions.AddArguments("--start-maximized");
            WebDriver = new ChromeDriver(chromeOptions);


            //WebDriver = new InternetExplorerDriver(new InternetExplorerOptions()
            //    {
            //        IntroduceInstabilityByIgnoringProtectedModeSettings = true,
            //        UnexpectedAlertBehavior = InternetExplorerUnexpectedAlertBehavior.Ignore
            //    });
            WebDriver.Manage().Window.Maximize();
            WebDriver.Manage().Timeouts().ImplicitlyWait(new TimeSpan(VeryLongWait8));
           
         

        }

        public static string CheckConsole()
        {
            return WebDriver.FindElement(By.XPath(@"//*[@id=""val-body""]/div/div[1]/ul/li/a")).Text;
        }

        public static void AjaxWait()
        {
            var
            i = 0;

            while (true)
            {
                var ajaxIsComplete = (bool)((IJavaScriptExecutor) WebDriver).ExecuteScript("return jQuery.active == 0");
                if (ajaxIsComplete)
                    break;
                Thread.Sleep(100);
                i++;
                if (i == 10000) break;
            }
        }

        private static string GetApplicationPath(string applicationName)
        {
            var solutionFolder = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory)));
            return solutionFolder != null ? Path.Combine(solutionFolder, applicationName) : null;
        }

        private static string GetTfsApplicationPath(string applicationName)
        {
            return @"C:\builds\1\Console\bin\Validus.Console\Validus.Console\_PublishedWebsites\Validus.Console";
        }

        private static void StartIis()
        {
            // Reference: http://www.iis.net/learn/extensions/using-iis-express/running-iis-express-from-the-command-line
            var applicationPath = GetApplicationPath(ApplicationName);

            if (!Directory.Exists(applicationPath)) applicationPath = GetTfsApplicationPath(ApplicationName);
           
            var lines = new List<string>(File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + @"\iisexpress.config"));
            int lineIndex = lines.FindIndex(line => line.Contains("{physicalPath}"));
            if (lineIndex != -1)
            {
                lines[lineIndex] = lines[lineIndex].Replace("{physicalPath}", applicationPath);
                File.WriteAllLines(AppDomain.CurrentDomain.BaseDirectory + @"\iisexpress.config", lines);
            }
            var programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
            _iisProcess = new Process
                {
                    StartInfo =
                        {
                            FileName = programFiles + @"\IIS Express\iisexpress.exe",
                            Arguments =
                                string.Format(@" /config:""{0}""", AppDomain.CurrentDomain.BaseDirectory + @"\iisexpress.config")
                        }
                };
            _iisProcess.Start();
        }

        public static void OpenBrowser()
        {
            _userDesktop = new Desktop();
            _userDesktop.BeginInteraction();
            StartIis();
        }

        public static void OpenConsole()
        {
            WebDriver.Url = string.Format(@"http://localhost:{0}", Port);
            WebDriver.Navigate();
            PrintScreen("Open");
        }

        public static void CloseBrowser()
        {
            try
            {
                PrintScreen("Close");
                WebDriver.Close();
            }
            finally
            {
                WebDriver.Quit();
                WebDriver.Dispose();
                if (_iisProcess.HasExited == false)
                {
                    _iisProcess.Kill();
                }
            }
        }

        public static string GetTypeAheadCssSelector(string param)
        {
            return string.Format(@"ul.typeahead.dropdown-menu[style*=""display: block""] > li[data-value=""{0}""]",
                                 param);
        }

        public static void SendMessage(string message)
        {
            ((IJavaScriptExecutor)WebDriver).ExecuteScript("toastr.info('<span style=color:Navy><B>ITC:</B>'+ arguments[0])+'</span>'", message);
        }

        public static void PrintScreen(string screenName)
        {
            var screenShotPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                                string.Format("ConsoleTestBrowser_{1}{0}.png",
                                                              System.DateTime.Now.ToString("yyyyMMddHHmmssffff"), screenName));
            try
            {

                var screenshotDriver = (ITakesScreenshot)WebDriver;
                var screenshot = screenshotDriver.GetScreenshot();
                screenshot.SaveAsFile(screenShotPath, ImageFormat.Png);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            try
            {
                screenShotPath = screenShotPath.Replace("ConsoleTestBrowser_", "ConsoleTestBrowser_WIN");
                var process = Process.GetProcesses().FirstOrDefault(x => x.MainWindowTitle.Contains("Validus") && x.ProcessName == "iexplore");
                if (process != null)
                {
                    var bmp = ScreenCapture.GetWindowCaptureAsBitmap(process.MainWindowHandle);
                    bmp.Save(screenShotPath, ImageFormat.Png);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }
    }
}