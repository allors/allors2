// <copyright file="IRoleType.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the RoleType type.</summary>

namespace Allors.Workspace.Meta
{
    /// <summary>
    /// A <see cref="IRoleType"/> defines the role side of a relation.
    /// This is also called the 'passive' side.
    /// RoleTypes can have composite and unit <see cref="ObjectType"/>s.
    /// </summary>
    public interface IRoleType : IPropertyType
    {
        string PropertyName { get; }

        string SingularPropertyName { get; }

        string PluralPropertyName { get; }

        string SingularFullName { get; }

        string PluralFullName { get; }

        IAssociationType AssociationType { get; }

        IRelationType RelationType { get; }

        int? Size { get; }

        int? Precision { get; }

        int? Scale { get; }
        bool IsRequired { get; }
        string DisplayName { get; }
    }
}
