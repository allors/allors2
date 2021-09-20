// <copyright file="ObjectFactory.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the ObjectBase type.</summary>

namespace Allors.Database
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using Meta;

    /// <summary>
    /// A base implementation for a static <see cref="IObjectFactory"/>.
    /// </summary>
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

        /// <summary>
        /// <see cref="IObjectType"/> by <see cref="IObjectType"/> id cache.
        /// </summary>
        private readonly Dictionary<Guid, IObjectType> objectTypeByObjectTypeId;

        /// <summary>
        /// <see cref="ConstructorInfo"/> by <see cref="IObjectType"/> cache.
        /// </summary>
        private readonly Dictionary<IObjectType, ConstructorInfo> contructorInfoByObjectType;

        /// <summary>
        /// Initializes a new state of the <see cref="ObjectFactory"/> class.
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
            this.Assembly = instance.GetTypeInfo().Assembly;

            var types = this.Assembly.GetTypes()
                .Where(type => type.Namespace != null &&
                               type.Namespace.Equals(instance.Namespace) &&
                               type.GetTypeInfo().ImplementedInterfaces.Contains(typeof(IObject)))
                .ToArray();

            this.MetaPopulation = metaPopulation;
            this.Namespace = instance.Namespace;

            var validationLog = metaPopulation.Validate();
            if (validationLog.ContainsErrors)
            {
                throw new Exception(validationLog.ToString());
            }

            var extensionMethodsByInterface = (from type in this.Assembly.ExportedTypes
                                               where type.GetTypeInfo().IsSealed && !type.GetTypeInfo().IsGenericType && !type.IsNested
                                               from method in type.GetTypeInfo().DeclaredMethods
                                               let parameterType = method.GetParameters()[0].ParameterType
                                               where method.IsStatic &&
                                                     method.IsDefined(typeof(ExtensionAttribute), false) &&
                                                     parameterType.IsInterface
                                               select new KeyValuePair<Type, MethodInfo>(parameterType, method))
                .GroupBy(kvp => kvp.Key, kvp => kvp.Value)
                .ToDictionary(v => v.Key, v => v.ToArray());

            metaPopulation.Bind(types, extensionMethodsByInterface);

            this.typeByObjectType = new Dictionary<IObjectType, Type>();
            this.objectTypeByType = new Dictionary<Type, IObjectType>();
            this.objectTypeByObjectTypeId = new Dictionary<Guid, IObjectType>();
            this.contructorInfoByObjectType = new Dictionary<IObjectType, ConstructorInfo>();

            var typeByName = types.ToDictionary(type => type.Name, type => type);

            foreach (var objectType in metaPopulation.DatabaseComposites)
            {
                var type = typeByName[objectType.Name];

                this.typeByObjectType[objectType] = type;
                this.objectTypeByType[type] = objectType;
                this.objectTypeByObjectTypeId[objectType.Id] = objectType;

                if (objectType is IClass)
                {
                    var parameterTypes = new[] { typeof(IStrategy) };
                    var constructor = type.GetTypeInfo().GetConstructor(parameterTypes);
                    this.contructorInfoByObjectType[objectType] = constructor ?? throw new ArgumentException(objectType.Name + " has no Allors constructor.");
                }
            }
        }

        /// <summary>
        /// Gets the namespace.
        /// </summary>
        public string Namespace { get; }

        /// <summary>
        /// Gets the assembly.
        /// </summary>
        public Assembly Assembly { get; }

        /// <summary>
        /// Gets the domain.
        /// </summary>
        public IMetaPopulation MetaPopulation { get; }

        /// <summary>
        /// Creates a new <see cref="IObject"/> given the <see cref="IStrategy"/>.
        /// </summary>
        /// <param name="strategy">The <see cref="IStrategy"/> for the new <see cref="IObject"/>.</param>
        /// <returns>The new <see cref="IObject"/>.</returns>
        public IObject Create(IStrategy strategy)
        {
            var objectType = strategy.Class;
            var constructor = this.contructorInfoByObjectType[objectType];
            object[] parameters = { strategy };

            return (IObject)constructor.Invoke(parameters);
        }

        public IObjectType GetObjectType<T>() => this.GetObjectType(typeof(T));

        /// <summary>
        /// Gets the .Net <see cref="Type"/> given the Allors <see cref="IObjectType"/>.
        /// </summary>
        /// <param name="type">The .Net <see cref="Type"/>.</param>
        /// <returns>The Allors <see cref="IObjectType"/>.</returns>
        public IObjectType GetObjectType(Type type) => !this.objectTypeByType.TryGetValue(type, out var objectType) ? null : objectType;

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

        /// <summary>
        /// Gets the .Net <see cref="Type"/> given the Allors <see cref="IObjectType"/>.
        /// </summary>
        /// <param name="objectTypeId">The Allors <see cref="IObjectType"/> id.</param>
        /// <returns>The .Net <see cref="Type"/>.</returns>
        public IObjectType GetObjectType(Guid objectTypeId)
        {
            this.objectTypeByObjectTypeId.TryGetValue(objectTypeId, out var objectType);
            return objectType;
        }
    }
}
