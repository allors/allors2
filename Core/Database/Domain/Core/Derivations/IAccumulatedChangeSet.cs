// <copyright file="IDerivation.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

// ReSharper disable StyleCop.SA1121
namespace Allors.Domain.Derivations
{
    using Allors;
    using Allors.Meta;

    public interface IAccumulatedChangeSet : IChangeSet
    {
        bool IsCreated(Object derivable);

        bool HasChangedRole(Object derivable, RoleType roleType);

        bool HasChangedRoles(Object derivable, params RoleType[] roleTypes);

        bool HasChangedAssociation(Object derivable, AssociationType associationType);

        bool HasChangedAssociations(Object derivable, params AssociationType[] associationTypes);

        bool HasChangedRoles(Object derivable, RelationKind relationKind);
    }
}
