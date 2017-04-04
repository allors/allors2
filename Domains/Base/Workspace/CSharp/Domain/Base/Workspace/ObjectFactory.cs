//------------------------------------------------------------------------------------------------- 
// <copyright file="ObjectFactory.cs" company="Allors bvba">
// Copyright 2002-2016 Allors bvba.
// 
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// 
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// <summary>Defines the ObjectBase type.</summary>
//-------------------------------------------------------------------------------------------------

using Allors.Meta;

namespace Allors.Workspace
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;


    public class ObjectFactory
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
        /// The namespace
        /// </param>
        public ObjectFactory(IMetaPopulation metaPopulation, Type[] types, MethodInfo[] methods, string @namespace)
        {
            this.MetaPopulation = metaPopulation;
            this.Namespace = @namespace;

            var validationLog = metaPopulation.Validate();
            if (validationLog.ContainsErrors)
            {
                throw new Exception(validationLog.ToString());
            }

            metaPopulation.Bind(types, methods);

            this.typeByObjectType = new Dictionary<IObjectType, Type>();
            this.objectTypeByType = new Dictionary<Type, IObjectType>();
            this.objectTypeByName = new Dictionary<string, IObjectType>();
            this.objectTypeByObjectTypeId = new Dictionary<Guid, IObjectType>();
            this.contructorInfoByObjectType = new Dictionary<IObjectType, ConstructorInfo>();

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
                    this.contructorInfoByObjectType[objectType] = constructor ?? throw new ArgumentException(objectType.Name + " has no Allors constructor.");
                }
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
        /// Creates a new <see cref="IObject"/> given the <see cref="IStrategy"/>.
        /// </summary>
        /// <param name="session">
        /// The session.
        /// </param>
        /// <param name="objectType">
        /// The object Type.
        /// </param>
        /// <returns>
        /// The new <see cref="IObject"/>.
        /// </returns>
        public SessionObject Create(Session session, ObjectType objectType)
        {
            var constructor = this.contructorInfoByObjectType[objectType];
            object[] parameters = { session };

            return (SessionObject)constructor.Invoke(parameters);
        }

        /// <summary>
        /// Gets the .Net <see cref="Type"/> given the Allors <see cref="IObjectType"/>.
        /// </summary>
        /// <param name="type">The .Net <see cref="Type"/>.</param>
        /// <returns>The Allors <see cref="IObjectType"/>.</returns>
        public IObjectType GetObjectTypeForType(Type type)
        {
            IObjectType objectType;
            return !this.objectTypeByType.TryGetValue(type, out objectType) ? null : objectType;
        }

        public IObjectType GetObjectTypeForTypeName(string name)
        {
            IObjectType objectType;
            return !this.objectTypeByName.TryGetValue(name, out objectType) ? null : objectType;
        }

        /// <summary>
        /// Gets the .Net <see cref="Type"/> given the Allors <see cref="IObjectType"/>.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The Allors <see cref="IObjectType"/>.
        /// </returns>
        public IObjectType GetObjectTypeForObjectTypeId(Guid id)
        {
            IObjectType objectType;
            return !this.objectTypeByObjectTypeId.TryGetValue(id, out objectType) ? null : objectType;
        }

        /// <summary>
        /// Gets the .Net <see cref="Type"/> given the Allors <see cref="IObjectType"/>.
        /// </summary>
        /// <param name="objectType">The Allors <see cref="IObjectType"/>.</param>
        /// <returns>The .Net <see cref="Type"/>.</returns>
        public Type GetTypeForObjectType(IObjectType objectType)
        {
            Type type;
            this.typeByObjectType.TryGetValue(objectType, out type);
            return type;
        }

        /// <summary>
        /// Gets the .Net <see cref="Type"/> given the Allors <see cref="IObjectType"/>.
        /// </summary>
        /// <param name="objectTypeId">The Allors <see cref="IObjectType"/> id.</param>
        /// <returns>The .Net <see cref="Type"/>.</returns>
        public IObjectType GetObjectTypeForType(Guid objectTypeId)
        {
            IObjectType objectType;
            this.objectTypeByObjectTypeId.TryGetValue(objectTypeId, out objectType);
            return objectType;
        }
    }
}