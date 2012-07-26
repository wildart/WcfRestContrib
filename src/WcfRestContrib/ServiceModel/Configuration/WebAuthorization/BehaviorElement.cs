using System;
using System.Configuration;
using System.IdentityModel.Policy;
using System.ServiceModel.Configuration;
using WcfRestContrib.ServiceModel.Description;
using WcfRestContrib.Reflection;

namespace WcfRestContrib.ServiceModel.Configuration.WebAuthorization
{
    class BehaviorElement : BehaviorExtensionElement
    {        
        private const string PolicyTypeElement = "authorizationPolicyType";

        public override Type BehaviorType
        {
            get { return typeof(WebAuthorizationConfigurationBehavior); }
        }

        protected override object CreateBehavior()
        {     
            Type policy = null;
            if (!string.IsNullOrEmpty(PolicyTypeName))
                try
                {
                    policy = PolicyTypeName.GetType<IAuthorizationPolicy>();
                }
                catch (Exception e)
                {
                    throw new Exception(string.Format("Invalid policyType specified in webAuthorization behavior element. {0}", e));
                }

            return new WebAuthorizationConfigurationBehavior(policy);
        }        

        [ConfigurationProperty(PolicyTypeElement, IsRequired = false, DefaultValue = null)]
        public string PolicyTypeName
        {
            get
            { return (string)base[PolicyTypeElement]; }
            set
            { base[PolicyTypeElement] = value; }
        }

    }
}
