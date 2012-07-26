﻿using System;
using System.Configuration;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;
using WcfRestContrib.ServiceModel.Dispatcher;

namespace WcfRestContrib.ServiceModel.Description
{
    public class OperationAuthenticationBehavior : IOperationBehavior 
    {
        public void ApplyDispatchBehavior(OperationDescription operationDescription, 
            DispatchOperation dispatchOperation)
        {
            var behavior =
                operationDescription.DeclaringContract.FindBehavior
                    <WebAuthenticationConfigurationBehavior,
                     WebAuthenticationConfigurationAttribute>(b => b.BaseBehavior) ??
                dispatchOperation.Parent.ChannelDispatcher.Host.Description.FindBehavior
                    <WebAuthenticationConfigurationBehavior,
                     WebAuthenticationConfigurationAttribute>(b => b.BaseBehavior);

            if (behavior == null)
                throw new ConfigurationErrorsException(
                    "OperationAuthenticationConfigurationBehavior not applied to contract or service. This behavior is required to configure operation authentication.");

            var authorizationBehavior =
                operationDescription.DeclaringContract.FindBehavior
                    <WebAuthorizationConfigurationBehavior,
                    WebAuthorizationConfigurationAttribute>(b => b.BaseBehavior);

            Type authorizationPolicy = null;
            if (authorizationBehavior != null)
                authorizationPolicy = authorizationBehavior.AuthorizationPolicyType;

            dispatchOperation.Invoker = new OperationAuthenticationInvoker(
                dispatchOperation.Invoker,
                behavior.ThrowIfNull().AuthenticationHandler,
                behavior.UsernamePasswordValidatorType,
                behavior.RequireSecureTransport,
                behavior.Source,
                authorizationPolicy);
        }

        public void ApplyClientBehavior(OperationDescription operationDescription, 
            ClientOperation clientOperation) { }
        public void AddBindingParameters(OperationDescription operationDescription, 
            BindingParameterCollection bindingParameters) { }
        public void Validate(OperationDescription operationDescription) { }
    }
}
