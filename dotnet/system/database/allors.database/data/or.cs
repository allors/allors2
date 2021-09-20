// <copyright file="Or.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Data
{
    using System.Linq;
    using Data;

    public class Or : ICompositePredicate
    {
        public string[] Dependencies { get; set; }

        public Or(params IPredicate[] operands) => this.Operands = operands;

        public IPredicate[] Operands { get; set; }

        bool IPredicate.ShouldTreeShake(IArguments arguments) => this.HasMissingDependencies(arguments) || this.Operands.All(v => v.ShouldTreeShake(arguments));

        bool IPredicate.HasMissingArguments(IArguments arguments) => this.Operands.All(v => v.HasMissingArguments(arguments));

        void IPredicateContainer.AddPredicate(IPredicate predicate) => this.Operands = this.Operands.Append(predicate).ToArray();

        void IPredicate.Build(ITransaction transaction, IArguments arguments, Database.ICompositePredicate compositePredicate)
        {
            var or = compositePredicate.AddOr();
            foreach (var predicate in this.Operands)
            {
                if (!predicate.ShouldTreeShake(arguments))
                {
                    predicate.Build(transaction, arguments, or);
                }
            }
        }

        public void Accept(IVisitor visitor) => visitor.VisitOr(this);
    }
}
