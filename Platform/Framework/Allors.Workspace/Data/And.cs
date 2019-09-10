// <copyright file="And.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using Allors.Protocol.Data;

    public class And : ICompositePredicate
    {
        public And(params IPredicate[] operands) => this.Operands = operands;

        public IPredicate[] Operands { get; set; }

        bool IPredicate.ShouldTreeShake(IReadOnlyDictionary<string, object> arguments) => this.Operands.All(v => v.ShouldTreeShake(arguments));

        bool IPredicate.HasMissingArguments(IReadOnlyDictionary<string, object> arguments) => this.Operands.All(v => v.HasMissingArguments(arguments));

        public Predicate ToJson() =>
            new Predicate()
            {
                Kind = PredicateKind.And,
                Operands = this.Operands.Select(v => v.ToJson()).ToArray(),
            };

        public void AddPredicate(IPredicate predicate) => this.Operands = new List<IPredicate>(this.Operands) { predicate }.ToArray();
    }
}
