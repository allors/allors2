// <copyright file="IObjectType.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IObjectType type.</summary>

namespace Allors.Meta
{
    using System;

    public interface IObjectType : IMetaObject, IComparable
    {
        string SingularName { get; }

        string PluralName { get; }

        bool IsUnit { get; }

        bool IsComposite { get; }

        bool IsInterface { get; }

        bool IsClass { get; }

        string Name { get; }

        Type ClrType { get; }
    }
}
