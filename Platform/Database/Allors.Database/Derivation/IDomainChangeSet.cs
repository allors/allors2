// <copyright file="IDomainDerivation.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IDomainChangeSet type.</summary>

namespace Allors
{
    using System.Collections.Generic;
    using Allors.Meta;

    public interface IDomainChangeSet
    {
        ISet<IObject> Created { get; }

        ISet<IObject> Deleted { get; }

        ISet<IObject> Associations { get; }

        ISet<IObject> Roles { get; }

        IDictionary<IObject, ISet<IRoleType>> RoleTypesByAssociation { get; }

        IDictionary<IObject, ISet<IAssociationType>> AssociationTypesByRole { get; }

        IDictionary<IRoleType, ISet<IObject>> AssociationsByRoleType { get; }

        IDictionary<IAssociationType, ISet<IObject>> RolesByAssociationType { get; }
    }
}
