// <copyright file="IRolePredicate.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Data
{
    using Meta;

    public interface IRolePredicate : IPredicate
    {
        IRoleType RoleType { get; set; }
    }
}
