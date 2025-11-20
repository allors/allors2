// <copyright file="ObjectFactory.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the ObjectBase type.</summary>

namespace Allors.Workspace
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using Allors.Workspace.Meta;

    public class ObjectFactory : IObjectFactory
    {
        /// <summary>
        /// <see cref="Type"/> by <see cref="IObjectType"/> cache.
        /// </summary>
        private readonly Dictionary<IObjectType, Type> typeByObjectType;

        /// <summary>
        /// <see cref="Type"/> by <see cref="IObjectType"/> id cache.
        /// </summary>
        private readonly Dictionary<Type, IObjectType> objectTypeByType;

        private readonly Dictionary<string, IObjectType> objectTypeByName;

        /// <summary>
        /// <see cref="IObjectType"/> by <see cref="IObjectType"/> id cache.
        /// </summary>
        private readonly Dictionary<Guid, IObjectType> objectTypeByObjectTypeId;

        /// <summary>
        /// <see cref="ConstructorInfo"/> by <see cref="IObjectType"/> cache.
        /// </summary>
        private readonly Dictionary<IObjectType, ConstructorInfo> contructorInfoByObjectType;

        private readonly Dictionary<IObjectType, object> emptyArrayByObjectType;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectFactory"/> class.
        /// </summary>
        /// <param name="metaPopulation">
        /// The meta population.
        /// </param>
        /// <param name="assembly">
        /// The assembly.
        /// </param>
        /// <param name="namespace">
        /// The namespace.
        /// </param>
        public ObjectFactory(IMetaPopulation metaPopulation, Type instance)
        {
            var assembly = instance.GetTypeInfo().Assembly;

            var types = assembly.GetTypes()
                .Where(type => type.Namespace != null &&
                               type.Namespace.Equals(instance.Namespace) &&
                               type.GetTypeInfo().ImplementedInterfaces.Contains(typeof(ISessionObject)))
                .ToArray();

            var extensionMethods = (from type in assembly.ExportedTypes
                                    where type.GetTypeInfo().IsSealed && !type.GetTypeInfo().IsGenericType && !type.IsNested
                                    from method in type.GetTypeInfo().DeclaredMethods
                                    where method.IsStatic && method.IsDefined(typeof(ExtensionAttribute), false)
                                    select method).ToArray();

            this.MetaPopulation = metaPopulation;
            this.Namespace = instance.Namespace;

            var validationLog = metaPopulation.Validate();
            if (validationLog.ContainsErrors)
            {
                throw new Exception(validationLog.ToString());
            }

            metaPopulation.Bind(types, extensionMethods);

            this.typeByObjectType = new Dictionary<IObjectType, Type>();
            this.objectTypeByType = new Dictionary<Type, IObjectType>();
            this.objectTypeByName = new Dictionary<string, IObjectType>();
            this.objectTypeByObjectTypeId = new Dictionary<Guid, IObjectType>();
            this.contructorInfoByObjectType = new Dictionary<IObjectType, ConstructorInfo>();
            this.emptyArrayByObjectType = new Dictionary<IObjectType, object>();

            var typeByName = types.ToDictionary(type => type.Name, type => type);

            foreach (var objectType in metaPopulation.Composites)
            {
                var type = typeByName[objectType.Name];

                this.typeByObjectType[objectType] = type;
                this.objectTypeByType[type] = objectType;
                this.objectTypeByName[type.Name] = objectType;
                this.objectTypeByObjectTypeId[objectType.Id] = objectType;

                if (objectType is IClass)
                {
                    var parameterTypes = new[] { typeof(Session) };
                    var constructor = type.GetTypeInfo().GetConstructor(parameterTypes);
                    if (constructor == null)
                    {
                        throw new ArgumentException(objectType.Name + " has no Allors constructor.");
                    }

                    this.contructorInfoByObjectType[objectType] = constructor;
                }

                this.emptyArrayByObjectType.Add(objectType, Array.CreateInstance(type, 0));
            }
        }

        /// <summary>
        /// Gets the namespace.
        /// </summary>
        public string Namespace { get; }

        /// <summary>
        /// Gets the domain.
        /// </summary>
        public IMetaPopulation MetaPopulation { get; }

        /// <summary>
        /// Creates a new <see cref="SessionObject"/> given the <see cref="SessionObject"/>.
        /// </summary>
        /// <param name="session">
        /// The session.
        /// </param>
        /// <param name="objectType">
        /// The object Type.
        /// </param>
        /// <returns>
        /// The new <see cref="SessionObject"/>.
        /// </returns>
        public SessionObject Create(ISession session, IObjectType objectType)
        {
            var constructor = this.contructorInfoByObjectType[objectType];
            object[] parameters = { session };

            return (SessionObject)constructor.Invoke(parameters);
        }

        ISessionObject IObjectFactory.Create(ISession session, IObjectType objectType) => this.Create(session, objectType);

        /// <summary>
        /// Gets the .Net <see cref="Type"/> given the Allors <see cref="IObjectType"/>.
        /// </summary>
        /// <param name="type">The .Net <see cref="Type"/>.</param>
        /// <returns>The Allors <see cref="IObjectType"/>.</returns>
        public IObjectType GetObjectType(Type type) => !this.objectTypeByType.TryGetValue(type, out var objectType) ? null : objectType;

        public IObjectType GetObjectType(string name) => !this.objectTypeByName.TryGetValue(name, out var objectType) ? null : objectType;

        /// <summary>
        /// Gets the .Net <see cref="Type"/> given the Allors <see cref="IObjectType"/>.
        /// </summary>
        /// <param name="objectType">The Allors <see cref="IObjectType"/>.</param>
        /// <returns>The .Net <see cref="Type"/>.</returns>
        public Type GetType(IObjectType objectType)
        {
            this.typeByObjectType.TryGetValue(objectType, out var type);
            return type;
        }

        public object EmptyArray(IObjectType objectType) => this.emptyArrayByObjectType[objectType];

        public IObjectType GetObjectType<T>()
        {
            var typeName = typeof(T).Name;
            return this.GetObjectType(typeName);
        }
    }
}
