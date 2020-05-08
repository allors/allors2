// <copyright file="And.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using Allors.Protocol.Data;

    public class And : ICompositePredicate
    {
        public string[] Dependencies { get; set; }

        public And(params IPredicate[] operands) => this.Operands = operands;

        public IPredicate[] Operands { get; set; }

        bool IPredicate.ShouldTreeShake(IDictionary<string, string> parameters) => this.HasMissingDependencies(parameters) || this.Operands.All(v => v.ShouldTreeShake(parameters));

        bool IPredicate.HasMissingArguments(IDictionary<string, string> parameters) => this.Operands.All(v => v.HasMissingArguments(parameters));

        public Predicate Save() =>
            new Predicate()
            {
                Kind = PredicateKind.And,
                Operands = this.Operands.Select(v => v.Save()).ToArray(),
            };

        void IPredicate.Build(ISession session, IDictionary<string, string> parameters, Allors.ICompositePredicate compositePredicate)
        {
            var and = compositePredicate.AddAnd();
            foreach (var predicate in this.Operands)
            {
                if (!predicate.ShouldTreeShake(parameters))
                {
                    predicate.Build(session, parameters, and);
                }
            }
        }

        public void AddPredicate(IPredicate predicate) => this.Operands = this.Operands.Append(predicate).ToArray();
    }
}
