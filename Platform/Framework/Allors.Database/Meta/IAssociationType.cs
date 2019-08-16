//-------------------------------------------------------------------------------------------------
// <copyright file="IAssociationType.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the AssociationType type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Meta
{
    /// <summary>
    /// An association type defines the association side of a relation.
    /// This is also called the 'active', 'controlling' or 'owning' side.
    /// AssociationTypes can only have composite <see cref="ObjectType"/>s.
    /// </summary>
    public interface IAssociationType : IPropertyType
    {
        string SingularName { get; }

        string SingularPropertyName { get; }

        string SingularFullName { get; }

        string PluralName { get; }

        string PluralPropertyName { get; }

        string PluralFullName { get; }

        bool IsMany { get; }

        IRelationType RelationType { get; }

        IComposite ObjectType { get; }

        bool IsOne { get; }

        IRoleType RoleType { get; }
    }
}
