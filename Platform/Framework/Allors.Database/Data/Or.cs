//-------------------------------------------------------------------------------------------------
// <copyright file="Or.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
//-------------------------------------------------------------------------------------------------

namespace Allors.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using Allors.Protocol.Data;

    public class Or : ICompositePredicate
    {
        public Or(params IPredicate[] operands) => this.Operands = operands;

        public IPredicate[] Operands { get; set; }

        bool IPredicate.ShouldTreeShake(IReadOnlyDictionary<string, object> arguments) => this.Operands.All(v => v.ShouldTreeShake(arguments));

        bool IPredicate.HasMissingArguments(IReadOnlyDictionary<string, object> arguments) => this.Operands.All(v => v.HasMissingArguments(arguments));

        void IPredicateContainer.AddPredicate(IPredicate predicate) => this.Operands = this.Operands.Append(predicate).ToArray();

        public Predicate Save() =>
            new Predicate()
            {
                Kind = PredicateKind.Or,
                Operands = this.Operands.Select(v => v.Save()).ToArray(),
            };

        void IPredicate.Build(ISession session, IReadOnlyDictionary<string, object> arguments, Allors.ICompositePredicate compositePredicate)
        {
            var or = compositePredicate.AddOr();
            foreach (var predicate in this.Operands)
            {
                if (!predicate.ShouldTreeShake(arguments))
                {
                    predicate.Build(session, arguments, or);
                }
            }
        }
    }
}
