using System;

namespace WcfRestContrib.DependencyInjection
{
    public class DefaultDependencyResolver : IDependencyResolver
    {
        public object GetInfrastructureService(Type serviceType)
        {
            return CreateInstance(serviceType);
        }

        public object CreateOperationContainer() { return null; }

        public object GetOperationService(object container, Type serviceType, params object[] args)
        {
            return CreateInstance(serviceType, args);
        }

        public object GetOperationService(object container, Type serviceType)
        {
            return CreateInstance(serviceType);
        }

        public void OperationError(object container, Exception exception) { }

        public void ReleaseOperationContainer(object container) { }

        private static object CreateInstance(Type type)
        {
            return type.IsAbstract || type.IsInterface ? null : Activator.CreateInstance(type);
        }

        private static object CreateInstance(Type type, params object[] args)
        {
            return type.IsAbstract || type.IsInterface ? null : Activator.CreateInstance(type, args);
        }
    }
}
