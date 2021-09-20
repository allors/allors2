// <copyright file="LessThan.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Data
{
    using System.Collections.Generic;

    using Allors.Meta;
    using Allors.Protocol.Data;

    public class LessThan : IRolePredicate
    {
        public string[] Dependencies { get; set; }

        public LessThan(IRoleType roleType = null) => this.RoleType = roleType;

        public IRoleType RoleType { get; set; }

        public object Value { get; set; }

        public string Parameter { get; set; }

        public Predicate Save() =>
            new Predicate
            {
                kind = PredicateKind.LessThan,
                roleType = this.RoleType?.Id,
                value = UnitConvert.ToString(this.Value),
                parameter = this.Parameter,
            };

        bool IPredicate.ShouldTreeShake(IDictionary<string, string> parameters) => this.HasMissingDependencies(parameters) || ((IPredicate)this).HasMissingArguments(parameters);

        bool IPredicate.HasMissingArguments(IDictionary<string, string> parameters) => this.Parameter != null && (parameters == null || !parameters.ContainsKey(this.Parameter));

        void IPredicate.Build(ISession session, IDictionary<string, string> parameters, Allors.ICompositePredicate compositePredicate)
        {
            var value = this.Parameter != null ? UnitConvert.Parse(this.RoleType.ObjectType.Id, parameters[this.Parameter]) : this.Value;

            compositePredicate.AddLessThan(this.RoleType, value);
        }
    }
}
