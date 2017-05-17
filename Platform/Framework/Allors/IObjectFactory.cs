//------------------------------------------------------------------------------------------------- 
// <copyright file="IObjectFactory.cs" company="Allors bvba">
// Copyright 2002-2017 Allors bvba.
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
// <summary>Defines the IObject type.</summary>
//-------------------------------------------------------------------------------------------------
namespace Allors
{
    using System;
    using System.Reflection;

    using Allors.Meta;

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
        /// Create a new IObject instance.
        /// </summary>
        /// <param name="strategy">The strategy</param>
        /// <returns>a new instance</returns>
        IObject Create(IStrategy strategy);

        /// <summary>
        /// Gets the IObjectType with the specified id.
        /// </summary>
        /// <param name="objectTypeId">
        /// The object type id.
        /// </param>
        /// <returns>
        /// The <see cref="IObjectType"/>.
        /// </returns>
        IObjectType GetObjectTypeForType(Guid objectTypeId);
        
        /// <summary>
        /// Gets the Type for the specified IObjectType
        /// </summary>
        /// <param name="objectType">The object type</param>
        /// <returns>The type</returns>
        Type GetTypeForObjectType(IObjectType objectType);

        /// <summary>
        /// Gets the IObjectType for the specified Type.
        /// Only works for static domains.
        /// </summary>
        /// <param name="type">The type</param>
        /// <returns>The object type</returns>
        IObjectType GetObjectTypeForType(Type type);
    }
}