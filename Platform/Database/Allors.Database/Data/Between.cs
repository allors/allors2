// <copyright file="Between.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
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
        public Between(IRoleType roleType = null) => this.RoleType = roleType;

        public IRoleType RoleType { get; set; }

        public IEnumerable<object> Values { get; set; }

        public string Argument { get; set; }

        public Predicate Save() =>
            new Predicate
            {
                Kind = PredicateKind.Between,
                RoleType = this.RoleType?.Id,
                Values = this.Values.Select(UnitConvert.ToString).ToArray(),
                Argument = this.Argument,
            };

        bool IPredicate.ShouldTreeShake(IDictionary<string, string> parameters) => ((IPredicate)this).HasMissingArguments(parameters);

        bool IPredicate.HasMissingArguments(IDictionary<string, string> parameters) => this.Argument != null && (parameters == null || !parameters.ContainsKey(this.Argument));

        void IPredicate.Build(ISession session, IDictionary<string, string> parameters, Allors.ICompositePredicate compositePredicate)
        {
            var parameter = this.Argument != null ? parameters[this.Argument]?.Split('|').Select(v => UnitConvert.Parse(this.RoleType.ObjectType.Id, v)) : null;
            var values = parameter != null ? parameter.ToArray() : this.Values.ToArray();
            compositePredicate.AddBetween(this.RoleType, values[0], values[1]);
        }
    }
}
