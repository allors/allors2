// <copyright file="IChangeSet.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IChangeSet type.</summary>

namespace Allors.Workspace
{
    using System.Collections.Generic;

    using Meta;

    /// <summary>
    /// A change set is created during a checkpoint
    /// and contains all changes that have
    /// occurred in a session starting
    /// from the beginning of the session or from a
    /// previous checkpoint.
    /// A change set can span (multiple) pull request.,
    /// The change set includes the internal changes
    /// that happened in the session and
    /// the changes that are pulled in from a pull request.
    /// </summary>
    public interface IChangeSet
    {
        /// <summary>
        /// Gets the session.
        /// </summary>
        ISession Session { get; }

        /// <summary>
        /// Gets the created objects.
        /// </summary>
        ISet<IStrategy> Created { get; }

        /// <summary>
        /// Gets the instantiated objects.
        /// </summary>
        ISet<IStrategy> Instantiated { get; }

        /// <summary>
        /// Gets the association objects by changed role type.
        /// </summary>
        IDictionary<IRoleType, ISet<IStrategy>> AssociationsByRoleType { get; }

        /// <summary>
        /// Gets the role objects by changed association type.
        /// </summary>
        IDictionary<IAssociationType, ISet<IStrategy>> RolesByAssociationType { get; }
    }
}
