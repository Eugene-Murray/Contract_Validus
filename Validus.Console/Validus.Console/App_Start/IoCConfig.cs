using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Validus.Console.BusinessLogic;
using Validus.Console.Controllers;
using Validus.Console.Data;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Validus.Core.HttpContext;
using Validus.Core.LogHandling;
using Validus.Console.SubscribeService;
using Validus.Console.BrokerService;
using Validus.Console.InsuredService;
using Validus.Console.VersionService;

namespace Validus.Console.App_Start
{
    public static class IoCConfig
    {
        static readonly IUnityContainer container = new UnityContainer();

        public static void RegisterIoC(HttpConfiguration config)
        {
            IServiceLocator serviceLocator = new UnityServiceLocator(container);

            EnterpriseLibraryContainer.Current = serviceLocator;
            container.AddNewExtension<EnterpriseLibraryCoreExtension>();
            container.RegisterType<ILogHandler, LogHandler>(new HttpContextLifetimeManager<LogHandler>());
            container.RegisterType<ICurrentHttpContext, CurrentHttpContext>(new HttpContextLifetimeManager<CurrentHttpContext>());

            container.RegisterType<IConsoleRepository, ConsoleRepository>(new HttpContextLifetimeManager<ConsoleRepository>());
            container.RegisterType<IAuditTrailModule, AuditTrailModule>(new HttpContextLifetimeManager<AuditTrailModule>());
            container.RegisterType<IAdminModuleManager, AdminModuleManager>(new HttpContextLifetimeManager<IAdminModuleManager>());
            container.RegisterType<ISubmissionModule, SubmissionModule>(new HttpContextLifetimeManager<ISubmissionModule>());
            container.RegisterType<IWebSiteModuleManager, WebSiteModuleManager>(new HttpContextLifetimeManager<IWebSiteModuleManager>());
            container.RegisterType<IWorkItemData, WorkItemData>(new HttpContextLifetimeManager<IWorkItemData>());
            container.RegisterType<IWorkItemBusinessModule, WorkItemBusinessModule>(new HttpContextLifetimeManager<IWorkItemBusinessModule>());
            container.RegisterType<IPolicyData, PolicyData>(new HttpContextLifetimeManager<IPolicyData>());
            container.RegisterType<IPolicyBusinessModule, PolicyBusinessModule>(new HttpContextLifetimeManager<IPolicyBusinessModule>());
            container.RegisterType<ISearchBusinessModule, SearchBusinessModule>(new HttpContextLifetimeManager<ISearchBusinessModule>());
            container.RegisterType<ISearchData, SearchData>(new HttpContextLifetimeManager<ISearchData>());

            container.RegisterType<IInsuredData, InsuredData>(new HttpContextLifetimeManager<IInsuredData>());
            container.RegisterType<IInsuredService, InsuredServiceClient>(new HttpContextLifetimeManager<IInsuredService>());
            container.RegisterType<InsuredServiceClient>(new InjectionConstructor());
            container.RegisterType<IInsuredBusinessModule, InsuredBusinessModule>(new HttpContextLifetimeManager<IInsuredBusinessModule>());
            
            container.RegisterType<IPolicyService, PolicyServiceClient>(new HttpContextLifetimeManager<IPolicyService>());
            container.RegisterType<PolicyServiceClient>(new InjectionConstructor());
            
            container.RegisterType<IVersionService, VersionServiceClient>(new HttpContextLifetimeManager<IVersionService>());
            container.RegisterType<VersionServiceClient>(new InjectionConstructor());
            
            container.RegisterType<IBrokerModuleManager, BrokerModuleManager>(new HttpContextLifetimeManager<IBrokerModuleManager>());
            container.RegisterType<IRiskCodeModuleManager, RiskCodeModuleManager>(new HttpContextLifetimeManager<IRiskCodeModuleManager>());
            container.RegisterType<IBrokerData, BrokerData>(new HttpContextLifetimeManager<IBrokerData>());
            container.RegisterType<IBrokerService, BrokerServiceClient>(new HttpContextLifetimeManager<IBrokerService>());
            container.RegisterType<BrokerServiceClient>(new InjectionConstructor());

            container.RegisterType<IQuoteSheetModule, QuoteSheetModule>(new HttpContextLifetimeManager<IBrokerModuleManager>());
            container.RegisterType<IQuoteSheetData, QuoteSheetData>(new HttpContextLifetimeManager<IQuoteSheetData>());

			container.RegisterType<IUwDocumentBusinessModule, UwDocumentBusinessModule>(new HttpContextLifetimeManager<IUwDocumentBusinessModule>());
			container.RegisterType<IUwDocumentData, UwDocumentData>(new HttpContextLifetimeManager<IUwDocumentData>());

			container.RegisterType<IWorldCheckBusinessModule, WorldCheckBusinessModule>(new HttpContextLifetimeManager<IWorldCheckBusinessModule>());
			container.RegisterType<IWorldCheckData, WorldCheckData>(new HttpContextLifetimeManager<IWorldCheckData>());

            container.RegisterType<IVersionService, VersionServiceClient>(new HttpContextLifetimeManager<IVersionService>());
            container.RegisterType<VersionServiceClient>(new InjectionConstructor());

            System.Web.Mvc.DependencyResolver.SetResolver(new Unity.Mvc3.UnityDependencyResolver(container));
            config.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);
        }
    }
}