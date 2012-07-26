using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Policy;
using System.IdentityModel.Selectors;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Text;
using WcfRestContrib.Reflection;

namespace WcfRestContrib.ServiceModel.Description
{
    class WebAuthorizationConfigurationAttribute : Attribute, IServiceBehavior
    {
        readonly WebAuthorizationConfigurationBehavior _behavior;


        public WebAuthorizationConfigurationAttribute(Type policy)
        {            
            if (policy != null && !policy.CastableAs<IAuthorizationPolicy>())
                throw new Exception(string.Format("policy {0} must inherit from IAuthorizationPolicy.", policy.Name));

             _behavior = new WebAuthorizationConfigurationBehavior(policy);
        }


        public WebAuthorizationConfigurationBehavior BaseBehavior
        { get { return _behavior; } }

        public void AddBindingParameters(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase, System.Collections.ObjectModel.Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        { _behavior.AddBindingParameters(serviceDescription, serviceHostBase, endpoints, bindingParameters); }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase)
        { _behavior.ApplyDispatchBehavior(serviceDescription, serviceHostBase); }

        public void Validate(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase)
        { _behavior.Validate(serviceDescription, serviceHostBase); }
    }
}
