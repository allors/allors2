//------------------------------------------------------------------------------------------------- 
// <copyright file="IChangeSet.cs" company="Allors bvba">
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
// <summary>Defines the IChangeSet type.</summary>
//-------------------------------------------------------------------------------------------------
namespace Allors
{
    using System.Collections.Generic;

    using Allors.Meta;

    /// <summary>
    /// A change set is created during a checkpoint
    /// and contains all changes that have
    /// occurred in a <see cref="ISession"/> either starting
    /// from the beginning of the transaction or from a 
    /// previous checkpoint.
    /// </summary>
    public interface IChangeSet
    {
        /// <summary>
        /// Gets the created objects.
        /// </summary>
        ISet<long> Created { get; }

        /// <summary>
        /// Gets the deleted objects.
        /// </summary>
        ISet<long> Deleted { get; }

        /// <summary>
        /// Gets the changed associations.
        /// </summary>
        ISet<long> Associations { get; }

        /// <summary>
        /// Gets the changed roles.
        /// </summary>
        ISet<long> Roles { get; }

        /// <summary>
        /// Gets the changed role types by association.
        /// </summary>
        IDictionary<long, ISet<IRoleType>> RoleTypesByAssociation { get; }

        /// <summary>
        /// Gets the changed association types by role.
        /// </summary>
        IDictionary<long, ISet<IAssociationType>> AssociationTypesByRole { get; }
    }
}