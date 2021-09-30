// <copyright file="PredicateKind.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Protocol.Json.Data
{
    public enum PredicateKind
    {
        And = 1,

        Or = 2,

        Not = 3,

        InstanceOf = 4,

        Exists = 5,

        Equals = 6,

        Contains = 7,

        ContainedIn = 8,

        Between = 9,

        GreaterThan = 10,

        LessThan = 11,

        Like = 12,
    }
}
