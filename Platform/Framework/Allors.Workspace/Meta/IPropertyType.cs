//-------------------------------------------------------------------------------------------------
// <copyright file="IPropertyType.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the RoleType type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Workspace.Meta
{
    using System;

    /// <summary>
    /// A <see cref="IPropertyType"/> can be a <see cref="IAssociationType"/> or a <see cref="IRoleType"/>.
    /// </summary>
    public interface IPropertyType : IMetaObject, IComparable
    {
        string Name { get; }

        string SingularName { get; }

        string PluralName { get; }

        IObjectType ObjectType { get; }

        bool IsOne { get; }

        bool IsMany { get; }
    }
}
