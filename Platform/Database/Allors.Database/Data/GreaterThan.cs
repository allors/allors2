// <copyright file="GreaterThan.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Data
{
    using System.Collections.Generic;

    using Allors.Meta;
    using Allors.Protocol.Data;

    public class GreaterThan : IRolePredicate
    {
        public GreaterThan(IRoleType roleType = null) => this.RoleType = roleType;

        public IRoleType RoleType { get; set; }

        public object Value { get; set; }

        public string Argument { get; set; }

        public Predicate Save() =>
            new Predicate
            {
                Kind = PredicateKind.GreaterThan,
                RoleType = this.RoleType?.Id,
                Value = UnitConvert.ToString(this.Value),
                Argument = this.Argument,
            };

        bool IPredicate.ShouldTreeShake(IDictionary<string, string> parameters) => ((IPredicate)this).HasMissingArguments(parameters);

        bool IPredicate.HasMissingArguments(IDictionary<string, string> parameters) => this.Argument != null && (parameters == null || !parameters.ContainsKey(this.Argument));

        void IPredicate.Build(ISession session, IDictionary<string, string> parameters, Allors.ICompositePredicate compositePredicate)
        {
            var value = this.Argument != null ? parameters[this.Argument] : this.Value;

            compositePredicate.AddGreaterThan(this.RoleType, value);
        }
    }
}
