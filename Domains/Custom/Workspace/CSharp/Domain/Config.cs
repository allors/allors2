namespace Tests
{
    using System;
    using Allors.Workspace;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using Allors.Meta;
    using Allors.Workspace.Domain;

    public class Config
    {
        public ObjectFactory ObjectFactory;

        public Config()
        {
            var assembly = typeof(User).GetTypeInfo().Assembly;
            var ns = typeof(User).Namespace;

            Type[] types = assembly.GetTypes()
                .Where(type =>
                    type.Namespace != null &&
                    type.Namespace.Equals(ns) &&
                    type.GetTypeInfo().ImplementedInterfaces.Contains(typeof(ISessionObject)))
                .ToArray();

            MethodInfo[] extensionMethods = (from type in assembly.ExportedTypes
                where type.GetTypeInfo().IsSealed && !type.GetTypeInfo().IsGenericType && !type.IsNested
                from method in type.GetTypeInfo().DeclaredMethods
                where method.IsStatic && method.IsDefined(typeof(ExtensionAttribute), false)
                select method).ToArray();


            this.ObjectFactory = new ObjectFactory(MetaPopulation.Instance, types, extensionMethods, ns);
        }
    }
}