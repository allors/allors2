// <copyright file="IRolePredicate.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Data
{
    using Allors.Meta;

    public interface IRolePredicate : IPredicate
    {
        IRoleType RoleType { get; set; }
    }
}
