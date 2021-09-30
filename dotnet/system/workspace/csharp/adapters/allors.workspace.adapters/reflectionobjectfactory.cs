// <copyright file="StaticObjectFactory.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the ObjectBase type.</summary>

namespace Allors.Workspace.Adapters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Meta;

    public class ReflectionObjectFactory : IObjectFactory
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
        private readonly Dictionary<string, IObjectType> objectTypeByObjectTypeTag;

        /// <summary>
        /// <see cref="ConstructorInfo"/> by <see cref="IObjectType"/> cache.
        /// </summary>
        private readonly Dictionary<IObjectType, ConstructorInfo> contructorInfoByObjectType;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReflectionObjectFactory"/> class.
        /// </summary>
        /// <param name="metaPopulation">
        /// The meta databaseOrigin.
        /// </param>
        /// <param name="instance"></param>
        /// <exception cref="ArgumentException"></exception>
        public ReflectionObjectFactory(IMetaPopulation metaPopulation, Type instance)
        {
            var assembly = instance.GetTypeInfo().Assembly;

            var types = assembly.GetTypes()
                .Where(type => type.Namespace?.Equals(instance.Namespace) == true &&
                               type.GetTypeInfo().ImplementedInterfaces.Contains(typeof(IObject)))
                .ToArray();

            metaPopulation.Bind(types);

            this.typeByObjectType = new Dictionary<IObjectType, Type>();
            this.objectTypeByType = new Dictionary<Type, IObjectType>();
            this.objectTypeByName = new Dictionary<string, IObjectType>();
            this.objectTypeByObjectTypeTag = new Dictionary<string, IObjectType>();
            this.contructorInfoByObjectType = new Dictionary<IObjectType, ConstructorInfo>();

            var typeByName = types.ToDictionary(type => type.Name, type => type);

            foreach (var objectType in metaPopulation.Composites)
            {
                var type = typeByName[objectType.SingularName];

                this.typeByObjectType[objectType] = type;
                this.objectTypeByType[type] = objectType;
                this.objectTypeByName[type.Name] = objectType;
                this.objectTypeByObjectTypeTag[objectType.Tag] = objectType;

                if (objectType is IClass)
                {
                    var parameterTypes = new[] { typeof(IStrategy) };
                    var databaseParameterTypes = new[] { typeof(IStrategy) };
                    this.contructorInfoByObjectType[objectType] = type.GetTypeInfo().GetConstructor(parameterTypes)
                                                                  ?? type.GetTypeInfo().GetConstructor(databaseParameterTypes)
                                                                  ?? throw new ArgumentException($"{objectType.SingularName} has no Allors constructor.");
                }
            }
        }

        /// <summary>
        /// Creates a new <see cref="SessionObject"/> given the <see cref="SessionObject"/>.
        /// </summary>
        /// <param name="strategy">
        /// The strategy.
        /// </param>
        /// <returns>
        /// The new <see cref="SessionObject"/>.
        /// </returns>
        public IObject Create(IStrategy strategy)
        {
            var constructor = this.contructorInfoByObjectType[strategy.Class];
            object[] parameters = { strategy };

            return (IObject)constructor.Invoke(parameters);
        }

        IObject IObjectFactory.Create(IStrategy strategy) => this.Create(strategy);

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

        public IObjectType GetObjectType<T>()
        {
            var typeName = typeof(T).Name;
            return this.GetObjectType(typeName);
        }
    }
}
