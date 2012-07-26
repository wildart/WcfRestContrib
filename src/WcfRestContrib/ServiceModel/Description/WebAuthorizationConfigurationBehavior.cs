using System;
using System.Collections.ObjectModel;
using System.IdentityModel.Policy;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using WcfRestContrib.DependencyInjection;

namespace WcfRestContrib.ServiceModel.Description
{
    class WebAuthorizationConfigurationBehavior : IServiceBehavior, IContractBehavior
    {
        public WebAuthorizationConfigurationBehavior(Type policy)
        {            
            AuthorizationPolicyType = policy;
        }
        
        public Type AuthorizationPolicyType { get; private set; }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase){}
        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters){}
        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase){}

        public void Validate(ContractDescription contractDescription, ServiceEndpoint endpoint){}
        public void ApplyDispatchBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, DispatchRuntime dispatchRuntime){}
        public void ApplyClientBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, ClientRuntime clientRuntime){}
        public void AddBindingParameters(ContractDescription contractDescription, ServiceEndpoint endpoint, BindingParameterCollection bindingParameters){}
    }
}
