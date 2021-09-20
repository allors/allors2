// <copyright file="IUnit.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IObjectType type.</summary>

namespace Allors.Database.Meta
{
    public interface IUnit : IObjectType
    {
        bool IsBinary { get; }

        bool IsBoolean { get; }

        bool IsDateTime { get; }

        bool IsDecimal { get; }

        bool IsFloat { get; }

        bool IsInteger { get; }

        bool IsString { get; }

        bool IsUnique { get; }
    }
}
