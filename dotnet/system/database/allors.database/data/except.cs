// <copyright file="Except.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Data
{
    using System.Linq;
    using Meta;
    

    public class Except : IExtentOperator
    {
        public Except(params IExtent[] operands) => this.Operands = operands;

        public IComposite ObjectType => this.Operands?[0].ObjectType;

        public IExtent[] Operands { get; set; }

        public Sort[] Sorting { get; set; }
        
        bool IExtent.HasMissingArguments(IArguments arguments) => this.Operands.Any(v => v.HasMissingArguments(arguments));

        Database.Extent IExtent.Build(ITransaction transaction, IArguments arguments)
        {
            var extent = transaction.Except(this.Operands[0].Build(transaction, arguments), this.Operands[1].Build(transaction, arguments));

            if (this.Sorting == null)
            {
                return extent;
            }

            foreach (var sort in this.Sorting)
            {
                sort.Build(extent);
            }

            return extent;
        }

        public void Accept(IVisitor visitor) => visitor.VisitExcept(this);
    }
}
