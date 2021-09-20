// <copyright file="IPropertyType.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the RoleType type.</summary>

namespace Allors.Workspace.Meta
{
    using System;

    /// <summary>
    /// A <see cref="IPropertyType"/> can be a <see cref="IAssociationType"/> or a <see cref="IRoleType"/>.
    /// </summary>
    public interface IMethodType : IOperandType
    {
        string Name { get; }
    }
}
