//-------------------------------------------------------------------------------------------------
// <copyright file="Not.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
//-------------------------------------------------------------------------------------------------

namespace Allors.Data
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

        public Predicate Save() =>
            new Predicate()
            {
                Kind = PredicateKind.Not,
                Operand = this.Operand?.Save(),
            };

        void IPredicate.Build(ISession session, IReadOnlyDictionary<string, object> arguments, Allors.ICompositePredicate compositePredicate)
        {
            var not = compositePredicate.AddNot();

            if (this.Operand != null && !this.Operand.ShouldTreeShake(arguments))
            {
                this.Operand?.Build(session, arguments, not);
            }
        }
    }
}
