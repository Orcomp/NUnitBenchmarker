﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NUnitBenchmarker.UIServiceReference {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="UIServiceReference.IUIService")]
    public interface IUIService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUIService/Ping", ReplyAction="http://tempuri.org/IUIService/PingResponse")]
        string Ping(string message);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUIService/GetImplementations", ReplyAction="http://tempuri.org/IUIService/GetImplementationsResponse")]
        NUnitBenchmarker.Data.TypeSpecification[] GetImplementations(NUnitBenchmarker.Data.TypeSpecification interfaceType);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUIService/LogEvent", ReplyAction="http://tempuri.org/IUIService/LogEventResponse")]
        void LogEvent(string loggingEventString);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUIService/UpdateResult", ReplyAction="http://tempuri.org/IUIService/UpdateResultResponse")]
        void UpdateResult(NUnitBenchmarker.Data.BenchmarkResult result);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IUIServiceChannel : NUnitBenchmarker.UIServiceReference.IUIService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class UIServiceClient : System.ServiceModel.ClientBase<NUnitBenchmarker.UIServiceReference.IUIService>, NUnitBenchmarker.UIServiceReference.IUIService {
        
        public UIServiceClient() {
        }
        
        public UIServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public UIServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public UIServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public UIServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string Ping(string message) {
            return base.Channel.Ping(message);
        }
        
        public NUnitBenchmarker.Data.TypeSpecification[] GetImplementations(NUnitBenchmarker.Data.TypeSpecification interfaceType) {
            return base.Channel.GetImplementations(interfaceType);
        }
        
        public void LogEvent(string loggingEventString) {
            base.Channel.LogEvent(loggingEventString);
        }
        
        public void UpdateResult(NUnitBenchmarker.Data.BenchmarkResult result) {
            base.Channel.UpdateResult(result);
        }
    }
}
