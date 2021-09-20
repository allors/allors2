// <copyright file="IPredicate.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Data
{
    using Allors.Protocol.Data;

    public interface IPredicate
    {
        string[] Dependencies { get; set; }

        Predicate ToJson();
    }
}
