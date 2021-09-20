// <copyright file="IChangeSet.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IChangeSet type.</summary>

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
