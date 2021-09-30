// <copyright file="IChangeSet.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IChangeSet type.</summary>

namespace Allors.Database
{
    using System.Collections.Generic;

    using Meta;

    /// <summary>
    /// A change set is created during a checkpoint
    /// and contains all changes that have
    /// occurred in a <see cref="ITransaction"/> either starting
    /// from the beginning of the transaction or from a
    /// previous checkpoint.
    /// </summary>
    public interface IChangeSet
    {
        /// <summary>
        /// Gets the created objects.
        /// </summary>
        ISet<IObject> Created { get; }

        /// <summary>
        /// Gets the deleted objects.
        /// </summary>
        ISet<IStrategy> Deleted { get; }

        /// <summary>
        /// Gets the changed associations.
        /// </summary>
        ISet<IObject> Associations { get; }

        /// <summary>
        /// Gets the changed roles.
        /// </summary>
        ISet<IObject> Roles { get; }

        /// <summary>
        /// Gets the changed role types by association.
        /// </summary>
        IDictionary<IObject, ISet<IRoleType>> RoleTypesByAssociation { get; }

        /// <summary>
        /// Gets the changed association types by role.
        /// </summary>
        IDictionary<IObject, ISet<IAssociationType>> AssociationTypesByRole { get; }

        /// <summary>
        /// Gets the changed associations by role type.
        /// </summary>
        IDictionary<IRoleType, ISet<IObject>> AssociationsByRoleType { get; }

        /// <summary>
        /// Gets the changed roles by association type.
        /// </summary>
        IDictionary<IAssociationType, ISet<IObject>> RolesByAssociationType { get; }
    }
}
