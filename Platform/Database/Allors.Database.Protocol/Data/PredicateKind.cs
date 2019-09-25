// <copyright file="PredicateKind.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Protocol.Data
{
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PredicateKind
    {
        [EnumMember(Value = "And")]
        And = 1,

        [EnumMember(Value = "Or")]
        Or = 2,

        [EnumMember(Value = "Not")]
        Not = 3,

        [EnumMember(Value = "Instanceof")]
        Instanceof = 4,

        [EnumMember(Value = "Exists")]
        Exists = 5,

        [EnumMember(Value = "Equals")]
        Equals = 6,

        [EnumMember(Value = "Contains")]
        Contains = 7,

        [EnumMember(Value = "ContainedIn")]
        ContainedIn = 8,

        [EnumMember(Value = "Between")]
        Between = 9,

        [EnumMember(Value = "GreaterThan")]
        GreaterThan = 10,

        [EnumMember(Value = "LessThan")]
        LessThan = 11,

        [EnumMember(Value = "Like")]
        Like = 12,
    }
}
