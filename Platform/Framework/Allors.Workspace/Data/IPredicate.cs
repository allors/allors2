// <copyright file="IPredicate.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Data
{
    using System.Collections.Generic;

    using Allors.Protocol.Data;

    public interface IPredicate
    {
        Predicate ToJson();

        bool ShouldTreeShake(IReadOnlyDictionary<string, object> arguments);

        bool HasMissingArguments(IReadOnlyDictionary<string, object> arguments);
    }
}
