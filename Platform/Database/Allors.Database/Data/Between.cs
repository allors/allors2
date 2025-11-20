// <copyright file="Between.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using Allors.Meta;
    using Allors.Protocol.Data;

    public class Between : IRolePredicate
    {
        public string[] Dependencies { get; set; }

        public Between(IRoleType roleType = null) => this.RoleType = roleType;

        public IRoleType RoleType { get; set; }

        public IEnumerable<object> Values { get; set; }

        public string Parameter { get; set; }

        public Predicate Save() =>
            new Predicate
            {
                kind = PredicateKind.Between,
                roleType = this.RoleType?.Id,
                values = this.Values.Select(UnitConvert.ToString).ToArray(),
                parameter = this.Parameter,
            };

        bool IPredicate.ShouldTreeShake(IDictionary<string, string> parameters) => this.HasMissingDependencies(parameters) || ((IPredicate)this).HasMissingArguments(parameters);

        bool IPredicate.HasMissingArguments(IDictionary<string, string> parameters) => this.Parameter != null && (parameters == null || !parameters.ContainsKey(this.Parameter));

        void IPredicate.Build(ISession session, IDictionary<string, string> parameters, Allors.ICompositePredicate compositePredicate)
        {
            var parameter = this.Parameter != null ? parameters[this.Parameter]?.Split('|').Select(v => UnitConvert.Parse(this.RoleType.ObjectType.Id, v)) : null;
            var values = parameter != null ? parameter.ToArray() : this.Values.ToArray();
            compositePredicate.AddBetween(this.RoleType, values[0], values[1]);
        }
    }
}
