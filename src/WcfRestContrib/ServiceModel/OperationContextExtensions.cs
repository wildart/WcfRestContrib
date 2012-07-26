using System;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.IdentityModel.Policy;
using WcfRestContrib.DependencyInjection;
using WcfRestContrib.IdentityModel.Policy;
using System.Collections.ObjectModel;
using WcfRestContrib.ServiceModel.Dispatcher;

namespace WcfRestContrib.ServiceModel
{
    public static class OperationContextExtensions
    {
        public static void ReplacePrimaryIdentity(this OperationContext context, IIdentity identity, Type authorizationPolicyType)
        {
            var incomingMessageProperties = context.IncomingMessageProperties;
            if (incomingMessageProperties != null)
            {
                var security = 
                    incomingMessageProperties.Security ?? 
                    (incomingMessageProperties.Security = new SecurityMessageProperty());

                var policies = security.ServiceSecurityContext.AuthorizationPolicies.ToList();

                IAuthorizationPolicy policy;
                if (authorizationPolicyType == null)
                    policy = new IdentityAuthorizationPolicy(identity);
                else
                {
                    policy = 
                        DependencyResolver.Current.GetOperationService<IAuthorizationPolicy>(
                            OperationContainer.GetCurrent(), 
                            authorizationPolicyType, 
                            identity);                    
                    policies.Add(policy);
                }
                policies.Add(policy);

                var authorizationContext =
                    AuthorizationContext.CreateDefaultAuthorizationContext(policies);

                security.ServiceSecurityContext = new ServiceSecurityContext(
                        authorizationContext,
                        new ReadOnlyCollection<IAuthorizationPolicy>(policies));
            }
        }

        public static bool HasTransportLayerSecurity(this OperationContext context)
        {
            return context.RequestContext.RequestMessage.Headers.To.Scheme.Equals("https", StringComparison.OrdinalIgnoreCase);
        }
    }
}
