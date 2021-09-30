// <copyright file="GreaterThan.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Data
{
    using System.Collections.Generic;
    using Allors.Protocol.Data;
    using Allors.Workspace.Meta;

    public class GreaterThan : IRolePredicate
    {
        public string[] Dependencies { get; set; }

        public GreaterThan(IRoleType roleType = null) => this.RoleType = roleType;

        public IRoleType RoleType { get; set; }

        public object Value { get; set; }

        public string Parameter { get; set; }

        public Predicate ToJson() =>
            new Predicate
            {
                kind = PredicateKind.GreaterThan,
                dependencies = this.Dependencies,
                roleType = this.RoleType?.Id,
                value = UnitConvert.ToString(this.Value),
                parameter = this.Parameter,
            };
    }
}
