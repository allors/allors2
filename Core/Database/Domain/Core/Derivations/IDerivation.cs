// <copyright file="IDerivation.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

// ReSharper disable StyleCop.SA1121
namespace Allors.Domain
{
    using System;
    using System.Collections.Generic;

    using Allors;
    using Allors.Meta;

    public interface IDerivation
    {
        Guid Id { get; }

        DateTime TimeStamp { get; }

        ISession Session { get; }

        IValidation Validation { get; }

        IChangeSet ChangeSet { get; }

        int Generation { get; }

        ISet<Object> DerivedObjects { get; }

        object this[string name] { get; set; }

        IValidation Derive(params IObject[] forced);

        void Add(Object derivable);

        void Add(IEnumerable<Object> derivables);

        /// <summary>
        /// The dependee is derived before the dependent object.
        /// </summary>
        /// <param name="dependent">The dependent object.</param>
        /// <param name="dependee">The dependee object.</param>
        void AddDependency(Object dependent, Object dependee);

        /// <summary>
        /// Gets or sets a value indicating if this derivable object is modified.
        /// The object is considered modified if.
        /// <ul>
        /// <li>it has been created</li>
        /// <li>it has changed roles</li>
        /// </ul>
        /// </summary>
        /// <param name="derivable">The derivable object.</param>
        /// <returns>a value indicating if this derivable object is modified.</returns>
        bool IsModified(Object derivable);

        bool IsModified(Object derivable, RelationKind kind);

        bool IsCreated(Object derivable);

        bool InDependency(Object derivable);

        bool IsForced(Object derivable);

        bool HasChangedRole(Object derivable, RoleType roleType);

        bool HasChangedRoles(Object derivable, params RoleType[] roleTypes);

        bool HasChangedAssociation(Object derivable, AssociationType associationType);

        bool HasChangedAssociations(Object derivable, params AssociationType[] associationTypes);

        bool HasChangedRoles(Object derivable, RelationKind relationKind);
    }
}
