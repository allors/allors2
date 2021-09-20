// <copyright file="Or.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using Allors.Protocol.Data;

    public class Or : ICompositePredicate
    {
        public string[] Dependencies { get; set; }

        public Or(params IPredicate[] operands) => this.Operands = operands;

        public IPredicate[] Operands { get; set; }

        void IPredicateContainer.AddPredicate(IPredicate predicate) => this.Operands = new List<IPredicate>(this.Operands) { predicate }.ToArray();

        public Predicate ToJson() =>
            new Predicate()
            {
                kind = PredicateKind.Or,
                dependencies = this.Dependencies,
                operands = this.Operands.Select(v => v.ToJson()).ToArray(),
            };
    }
}
