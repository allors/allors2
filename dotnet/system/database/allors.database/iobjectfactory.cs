// <copyright file="IObjectFactory.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IObject type.</summary>

namespace Allors.Database
{
    using System;
    using System.Reflection;

    using Meta;

    /// <summary>
    /// A factory for creating new IObject instances.
    /// </summary>
    public interface IObjectFactory
    {
        /// <summary>
        /// Gets the namespace.
        /// </summary>
        string Namespace { get; }

        /// <summary>
        /// Gets the assembly.
        /// </summary>
        Assembly Assembly { get; }

        /// <summary>
        /// Gets the domain.
        /// </summary>
        /// <value>The domain.</value>
        IMetaPopulation MetaPopulation { get; }

        /// <summary>
        /// Create a new IObject state.
        /// </summary>
        /// <param name="strategy">The strategy.</param>
        /// <returns>a new state.</returns>
        IObject Create(IStrategy strategy);

        IObjectType GetObjectType<T>();

        /// <summary>
        /// Gets the IObjectType for the specified Type.
        /// Only works for static domains.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The object type.</returns>
        IObjectType GetObjectType(Type type);

        /// <summary>
        /// Gets the IObjectType with the specified id.
        /// </summary>
        /// <param name="objectTypeId">
        /// The object type id.
        /// </param>
        /// <returns>
        /// The <see cref="IObjectType"/>.
        /// </returns>
        IObjectType GetObjectType(Guid objectTypeId);

        /// <summary>
        /// Gets the Type for the specified IObjectType.
        /// </summary>
        /// <param name="objectType">The object type.</param>
        /// <returns>The type.</returns>
        Type GetType(IObjectType objectType);

    }
}
