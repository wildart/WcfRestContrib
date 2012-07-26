using System;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;
using WcfRestContrib.ServiceModel.Dispatcher;

namespace WcfRestContrib.ServiceModel.Description
{
    public class ServiceAuthenticationBehavior : IServiceBehavior, IContractBehavior 
    {
        public class ServiceAuthenticationConfigurationMissingException : Exception
        {
            public ServiceAuthenticationConfigurationMissingException() : 
                base("ServiceAuthenticationConfigurationBehavior not applied to contract or service. This behavior is required to configure service authentication.") {}
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase)
        {
            var authenticationBehavior = 
                serviceHostBase.Description.FindBehavior
                        <WebAuthenticationConfigurationBehavior,
                        WebAuthenticationConfigurationAttribute>(b => b.BaseBehavior);

            if (authenticationBehavior == null)
                throw new ServiceAuthenticationConfigurationMissingException();            

            var authenticationHandler = authenticationBehavior.AuthenticationHandler;
            var usernamePasswordValidator = authenticationBehavior.UsernamePasswordValidatorType;

            var authorizationBehavior =
                serviceHostBase.Description.FindBehavior
                    <WebAuthorizationConfigurationBehavior,
                    WebAuthorizationConfigurationAttribute>(b => b.BaseBehavior);

            Type authorizationPolicy = null;
            if (authorizationBehavior != null)
                authorizationPolicy = authorizationBehavior.AuthorizationPolicyType;

            foreach (ChannelDispatcher dispatcher in 
                serviceHostBase.ChannelDispatchers)
                foreach (var endpoint in dispatcher.Endpoints)
                    endpoint.DispatchRuntime.MessageInspectors.Add(
                        new ServiceAuthenticationInspector(
                            authenticationHandler,
                            usernamePasswordValidator,
                            authenticationBehavior.RequireSecureTransport,
                            authenticationBehavior.Source,
                            authorizationPolicy));
        }

        public void Validate(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase) { }
        public void AddBindingParameters(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase, System.Collections.ObjectModel.Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters) { }

        public void ApplyDispatchBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, DispatchRuntime dispatchRuntime)
        {
            var behavior = 
                dispatchRuntime.ChannelDispatcher.Host.Description.FindBehavior
                        <WebAuthenticationConfigurationBehavior,
                         WebAuthenticationConfigurationAttribute>(b => b.BaseBehavior);
            
            if (behavior == null)
                behavior = contractDescription.FindBehavior
                        <WebAuthenticationConfigurationBehavior,
                         WebAuthenticationConfigurationAttribute>(b => b.BaseBehavior);

            if (behavior == null)
                throw new ServiceAuthenticationConfigurationMissingException();

            var authorizationBehavior =
                dispatchRuntime.ChannelDispatcher.Host.Description.FindBehavior
                        <WebAuthorizationConfigurationBehavior,
                        WebAuthorizationConfigurationAttribute>(b => b.BaseBehavior);

            Type authorizationPolicy = null;
            if (authorizationBehavior != null)
                authorizationPolicy = authorizationBehavior.AuthorizationPolicyType;

            foreach (var endpointDispatcher in dispatchRuntime.ChannelDispatcher.Endpoints)
                endpointDispatcher.DispatchRuntime.MessageInspectors.Add(
                    new ServiceAuthenticationInspector(
                        behavior.ThrowIfNull().AuthenticationHandler,
                        behavior.UsernamePasswordValidatorType,
                        behavior.RequireSecureTransport,
                        behavior.Source,
                        authorizationPolicy));
        }

        public void Validate(ContractDescription contractDescription, ServiceEndpoint endpoint) { }
        public void AddBindingParameters(ContractDescription contractDescription, ServiceEndpoint endpoint, BindingParameterCollection bindingParameters) { }
        public void ApplyClientBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, ClientRuntime clientRuntime) { }
    }
}
