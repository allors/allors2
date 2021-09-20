// <copyright file="Not.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Data
{
    public class Not : ICompositePredicate
    {
        public string[] Dependencies { get; set; }

        public Not(IPredicate operand = null) => this.Operand = operand;

        public IPredicate Operand { get; set; }

        bool IPredicate.ShouldTreeShake(IArguments arguments) => this.HasMissingDependencies(arguments) || this.Operand == null || this.Operand.ShouldTreeShake(arguments);

        bool IPredicate.HasMissingArguments(IArguments arguments) => this.Operand != null && this.Operand.HasMissingArguments(arguments);

        void IPredicateContainer.AddPredicate(IPredicate predicate) => this.Operand = predicate;

        void IPredicate.Build(ITransaction transaction, IArguments arguments, Database.ICompositePredicate compositePredicate)
        {
            var not = compositePredicate.AddNot();

            if (this.Operand != null && !this.Operand.ShouldTreeShake(arguments))
            {
                this.Operand?.Build(transaction, arguments, not);
            }
        }

        public void Accept(IVisitor visitor) => visitor.VisitNot(this);
    }
}
