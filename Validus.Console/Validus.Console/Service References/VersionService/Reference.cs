﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18034
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Validus.Console.VersionService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Versions", Namespace="http://schemas.datacontract.org/2004/07/Validus.Services.DataContract.Common")]
    [System.SerializableAttribute()]
    public partial class Versions : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string BusinessLogicVersionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DataAccessVersionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DataContractVersionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string WCFVersionField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string BusinessLogicVersion {
            get {
                return this.BusinessLogicVersionField;
            }
            set {
                if ((object.ReferenceEquals(this.BusinessLogicVersionField, value) != true)) {
                    this.BusinessLogicVersionField = value;
                    this.RaisePropertyChanged("BusinessLogicVersion");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string DataAccessVersion {
            get {
                return this.DataAccessVersionField;
            }
            set {
                if ((object.ReferenceEquals(this.DataAccessVersionField, value) != true)) {
                    this.DataAccessVersionField = value;
                    this.RaisePropertyChanged("DataAccessVersion");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string DataContractVersion {
            get {
                return this.DataContractVersionField;
            }
            set {
                if ((object.ReferenceEquals(this.DataContractVersionField, value) != true)) {
                    this.DataContractVersionField = value;
                    this.RaisePropertyChanged("DataContractVersion");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string WCFVersion {
            get {
                return this.WCFVersionField;
            }
            set {
                if ((object.ReferenceEquals(this.WCFVersionField, value) != true)) {
                    this.WCFVersionField = value;
                    this.RaisePropertyChanged("WCFVersion");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="VersionService.IVersionService")]
    public interface IVersionService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IVersionService/GetVersion", ReplyAction="http://tempuri.org/IVersionService/GetVersionResponse")]
        Validus.Console.VersionService.Versions GetVersion();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IVersionService/GetVersion", ReplyAction="http://tempuri.org/IVersionService/GetVersionResponse")]
        System.Threading.Tasks.Task<Validus.Console.VersionService.Versions> GetVersionAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IVersionServiceChannel : Validus.Console.VersionService.IVersionService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class VersionServiceClient : System.ServiceModel.ClientBase<Validus.Console.VersionService.IVersionService>, Validus.Console.VersionService.IVersionService {
        
        public VersionServiceClient() {
        }
        
        public VersionServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public VersionServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public VersionServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public VersionServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public Validus.Console.VersionService.Versions GetVersion() {
            return base.Channel.GetVersion();
        }
        
        public System.Threading.Tasks.Task<Validus.Console.VersionService.Versions> GetVersionAsync() {
            return base.Channel.GetVersionAsync();
        }
    }
}
