// <copyright file="IComposite.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the ObjectType type.</summary>

namespace Allors.Meta
{
    using System.Collections.Generic;

    public interface IComposite : IObjectType
    {
        IEnumerable<IAssociationType> AssociationTypes { get; }

        IEnumerable<IRoleType> RoleTypes { get; }

        IEnumerable<IInterface> DirectSupertypes { get; }

        IEnumerable<IInterface> Supertypes { get; }

        bool ExistSupertype(IInterface @interface);

        bool ExistAssociationType(IAssociationType association);

        bool ExistRoleType(IRoleType roleType);

        bool IsAssignableFrom(IComposite objectType);

        bool ExistClass { get; }

        IEnumerable<IClass> Classes { get; }

        bool ExistExclusiveClass { get; }

        IClass ExclusiveClass { get; }

        bool IsSynced { get; }

        bool AssignedIsSynced { get; set; }
    }
}
