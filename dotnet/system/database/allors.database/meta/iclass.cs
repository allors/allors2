// <copyright file="IClass.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IObjectType type.</summary>

namespace Allors.Database.Meta
{
    using System;

    public interface IClass : IComposite
    {
        Action<object, object>[] Actions(IMethodType methodType);

        IRoleType[] OverriddenRequiredRoleTypes { get; set; }

        IRoleType[] OverriddenUniqueRoleTypes { get; set; }

        IRoleType[] RequiredRoleTypes { get; }

        IRoleType[] UniqueRoleTypes { get; }
    }
}
