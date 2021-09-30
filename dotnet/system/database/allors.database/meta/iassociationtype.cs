// <copyright file="IAssociationType.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the AssociationType type.</summary>

namespace Allors.Database.Meta
{
    /// <summary>
    /// An association type defines the association side of a relation.
    /// This is also called the 'active', 'controlling' or 'owning' side.
    /// AssociationTypes can only have composite <see cref="ObjectType"/>s.
    /// </summary>
    public interface IAssociationType : IPropertyType
    {
        IRelationType RelationType { get; }

        IRoleType RoleType { get; }

        new IComposite ObjectType { get; }
    }
}
