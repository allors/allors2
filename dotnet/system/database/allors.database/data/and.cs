// <copyright file="And.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Data
{
    using System.Linq;

    public class And : ICompositePredicate
    {
        public string[] Dependencies { get; set; }

        public And(params IPredicate[] operands) => this.Operands = operands;

        public IPredicate[] Operands { get; set; }

        bool IPredicate.ShouldTreeShake(IArguments arguments) => this.HasMissingDependencies(arguments) || this.Operands.All(v => v.ShouldTreeShake(arguments));

        bool IPredicate.HasMissingArguments(IArguments arguments) => this.Operands.All(v => v.HasMissingArguments(arguments));

        void IPredicate.Build(ITransaction transaction, IArguments arguments, Database.ICompositePredicate compositePredicate)
        {
            var and = compositePredicate.AddAnd();
            foreach (var predicate in this.Operands)
            {
                if (!predicate.ShouldTreeShake(arguments))
                {
                    predicate.Build(transaction, arguments, and);
                }
            }
        }

        public void AddPredicate(IPredicate predicate) => this.Operands = this.Operands.Append(predicate).ToArray();

        public void Accept(IVisitor visitor) => visitor.VisitAnd(this);
    }
}
