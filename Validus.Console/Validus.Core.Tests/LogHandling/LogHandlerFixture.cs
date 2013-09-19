using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using System.IO;
using Validus.Core.LogHandling;
using System.Configuration;

namespace Validus.Core.Tests.LogHandling
{
    [TestClass]
    public class LogHandlerFixture
    {
        public static IUnityContainer _container;
        public static ILogHandler _logHandler;
        private static string _logFileLocation;
        
        [TestInitialize]
        public void Init()
        {
            _container = new UnityContainer();
            _container.AddNewExtension<EnterpriseLibraryCoreExtension>();
            _logHandler = _container.Resolve<LogHandler>();

            // Note: this path is also set in "Rolling Flat File Trace Listener" section
            _logFileLocation = ConfigurationManager.AppSettings["logFileLocation"].ToString();
        }
        
        
        [TestMethod]
        public void LogTo_RollingFlatFile_Success()
        {
            // Assign

            // Act
            _logHandler.WriteLog("Unit Test Message", LogSeverity.Error, LogCategory.BusinessComponent);
     
            // Assert
            Assert.IsTrue(File.Exists(_logFileLocation));
        }

        [TestCleanup]
        public void Cleanup()
        {

        }
    }
}

