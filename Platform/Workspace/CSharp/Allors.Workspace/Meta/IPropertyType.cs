// <copyright file="IPropertyType.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the RoleType type.</summary>

namespace Allors.Workspace.Meta
{
    using System;

    /// <summary>
    /// A <see cref="IPropertyType"/> can be a <see cref="IAssociationType"/> or a <see cref="IRoleType"/>.
    /// </summary>
    public interface IPropertyType : IOperandType
    {
        string Name { get; }

        string SingularName { get; }

        string PluralName { get; }

        IObjectType ObjectType { get; }

        bool IsOne { get; }

        bool IsMany { get; }
    }
}
