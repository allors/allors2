// <copyright file="Not.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Data
{
    using System.Collections.Generic;

    using Allors.Protocol.Data;

    public class Not : ICompositePredicate
    {
        public string[] Dependencies { get; set; }

        public Not(IPredicate operand = null) => this.Operand = operand;

        public IPredicate Operand { get; set; }

        void IPredicateContainer.AddPredicate(IPredicate predicate) => this.Operand = predicate;

        public Predicate ToJson() =>
            new Predicate()
            {
                kind = PredicateKind.Not,
                dependencies = this.Dependencies,
                operand = this.Operand?.ToJson(),
            };
    }
}
