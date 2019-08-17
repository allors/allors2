//-------------------------------------------------------------------------------------------------
// <copyright file="Not.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
//-------------------------------------------------------------------------------------------------

namespace Allors.Workspace.Data
{
    using System.Collections.Generic;

    using Allors.Protocol.Data;

    public class Not : ICompositePredicate
    {
        public Not(IPredicate operand = null) => this.Operand = operand;

        public IPredicate Operand { get; set; }

        bool IPredicate.ShouldTreeShake(IReadOnlyDictionary<string, object> arguments) => this.Operand == null || this.Operand.ShouldTreeShake(arguments);

        bool IPredicate.HasMissingArguments(IReadOnlyDictionary<string, object> arguments) => this.Operand != null && this.Operand.HasMissingArguments(arguments);

        void IPredicateContainer.AddPredicate(IPredicate predicate) => this.Operand = predicate;

        public Predicate ToJson() =>
            new Predicate()
            {
                Kind = PredicateKind.Not,
                Operand = this.Operand?.ToJson(),
            };
    }
}
