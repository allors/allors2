// <copyright file="IPropertyType.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the RoleType type.</summary>

namespace Allors.Database.Meta
{
    /// <summary>
    /// A <see cref="IPropertyType"/> can be a <see cref="IAssociationType"/> or a <see cref="IRoleType"/>.
    /// </summary>
    public interface IPropertyType : IOperandType
    {
        IObjectType ObjectType { get; }

        string Name { get; }

        string SingularName { get; }

        string SingularFullName { get; }

        string PluralName { get; }

        string PluralFullName { get; }

        bool IsOne { get; }

        bool IsMany { get; }

        object Get(IStrategy strategy, IComposite ofType = null);
    }
}
