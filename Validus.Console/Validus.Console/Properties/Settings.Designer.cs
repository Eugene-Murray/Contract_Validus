﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18034
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Validus.Console.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "11.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("talbotdev")]
        public string DomainPrefix {
            get {
                return ((string)(this["DomainPrefix"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=ukroomdev01;Initial Catalog=Subscribe;Integrated Security=True")]
        public string SubscribeSQL {
            get {
                return ((string)(this["SubscribeSQL"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("http://uksp10webdev01:6262/")]
        public string ServicesHostUrl {
            get {
                return ((string)(this["ServicesHostUrl"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Integrated=True;IsPrimaryLogin=True;Authenticate=True;EncryptedPassword=False;Hos" +
            "t=UKWFK2DEV02;Port=5252")]
        public string WorkflowServerConnectionString {
            get {
                return ((string)(this["WorkflowServerConnectionString"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("http://spintranet.talbotdev.com/_vti_bin/search.asmx")]
        public string Validus_Console_SPQueryService_QueryService {
            get {
                return ((string)(this["Validus_Console_SPQueryService_QueryService"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Integrated=True;IsPrimaryLogin=True;Authenticate=True;EncryptedPassword=False;Hos" +
            "t=UKWFK2DEV02;Port=5555")]
        public string WorkflowServerManagementConnectionString {
            get {
                return ((string)(this["WorkflowServerManagementConnectionString"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("http://services.talbotdev.com/FileNetService/DMSService.asmx")]
        public string Validus_Console_DocumentManagementService_DMSService {
            get {
                return ((string)(this["Validus_Console_DocumentManagementService_DMSService"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("http://ukbirsdev01:80/ReportServer/ReportExecution2005.asmx")]
        public string Validus_Console_ReportingService_ReportExecutionService {
            get {
                return ((string)(this["Validus_Console_ReportingService_ReportExecutionService"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("http://services.talbotdev.com/WorldCheck/WorldCheckService.asmx")]
        public string Validus_Console_WorldCheckService_WorldCheckService {
            get {
                return ((string)(this["Validus_Console_WorldCheckService_WorldCheckService"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("http://services.talbotdev.com/Subscribe.asmx")]
        public string Validus_Console_SubscribeSoapService_Subscribe {
            get {
                return ((string)(this["Validus_Console_SubscribeSoapService_Subscribe"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://intranet.globaldev.local/sites/searchcenter/_api/search/query?querytext=\'{" +
            "0}\'&selectproperties=\'{1}\'")]
        public string SP2013RestUrl {
            get {
                return ((string)(this["SP2013RestUrl"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://intranet.globaldev.local/sites/searchcenter")]
        public string SP2013SearchUrl {
            get {
                return ((string)(this["SP2013SearchUrl"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("PolicyDetails,SubscribeInsured,SubscribeBroker,Claims")]
        public string SP2013ContentSources {
            get {
                return ((string)(this["SP2013ContentSources"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"VALBkrCtc,VALBkrCd,VALBpr,VALCcy,VALLdr,VALDescription,VALDol,VALInsdNm,VALLoccLocn,VALPolID,VALSt,VALUwrPsu,VALReserve,VALPaid,HITHIGHLIGHTEDSUMMARY,VALBkrGrpCd,VALBkrNm,VALBkrPsu,VALBkrSeqId,VALAcctgYr,VALCob,VALDivision,VALEntSt,VALIncpDt,VALOrigOff,VALSt,VALContentType,VALInsdId,VALLastYear,VALFirstYear,VALNoOfRisks,VALNoOfLiveRisks,VALNoOfOtherRisks,VALFirstLiveYear,VALLastLiveYear")]
        public string SP2013Properties {
            get {
                return ((string)(this["SP2013Properties"]));
            }
        }
    }
}