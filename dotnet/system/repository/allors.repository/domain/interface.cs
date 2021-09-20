// <copyright file="Interface.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IObjectType type.</summary>

namespace Allors.Repository.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Inflector;

    public class Interface : Composite
    {
        public Interface(Inflector inflector, Guid id, string name)
            : base(inflector, id, name)
        {
            this.PartialByDomainName = new Dictionary<string, PartialInterface>();
            this.InheritedPropertyByRoleName = new Dictionary<string, Property>();
        }

        public override Interface[] Interfaces
        {
            get
            {
                var interfaces = new HashSet<Interface>(this.ImplementedInterfaces);
                foreach (var implementedInterface in this.ImplementedInterfaces)
                {
                    implementedInterface.AddInterfaces(interfaces);
                }

                return interfaces.ToArray();
            }
        }

        public Dictionary<string, PartialInterface> PartialByDomainName { get; }

        public Dictionary<string, Property> InheritedPropertyByRoleName { get; }

        public Property[] InheritedProperties => this.InheritedPropertyByRoleName.Values.ToArray();

        public override string ToString() => this.SingularName;

        public Property GetImplementedProperty(Property property)
        {
            if (this.PropertyByRoleName.TryGetValue(property.RoleName, out var implementedProperty))
            {
                return implementedProperty;
            }

            foreach (var @interface in this.ImplementedInterfaces)
            {
                implementedProperty = @interface.GetImplementedProperty(property);
                if (implementedProperty != null)
                {
                    return implementedProperty;
                }
            }

            return null;
        }

        public Method GetImplementedMethod(Method method)
        {
            if (this.MethodByName.TryGetValue(method.Name, out var implementedMethod))
            {
                return implementedMethod;
            }

            foreach (var @interface in this.ImplementedInterfaces)
            {
                implementedMethod = @interface.GetImplementedMethod(method);
                if (implementedMethod != null)
                {
                    return implementedMethod;
                }
            }

            return null;
        }

        private void AddInterfaces(ISet<Interface> interfaces)
        {
            interfaces.UnionWith(this.ImplementedInterfaces);
            foreach (var implementedInterface in this.ImplementedInterfaces)
            {
                implementedInterface.AddInterfaces(interfaces);
            }
        }
    }
}
