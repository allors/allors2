// <copyright file="Filter.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Data
{
    using Meta;
    
    public class Extent : IExtent, IPredicateContainer
    {
        public Extent(IComposite objectType) => this.ObjectType = objectType;

        public IComposite ObjectType { get; set; }

        public IPredicate Predicate { get; set; }

        public Sort[] Sorting { get; set; }

        bool IExtent.HasMissingArguments(IArguments arguments) => this.Predicate != null && this.Predicate.HasMissingArguments(arguments);

        public Database.Extent Build(ITransaction transaction, IArguments arguments = null)
        {
            var extent = transaction.Extent(this.ObjectType);

            if (this.Predicate != null && !this.Predicate.ShouldTreeShake(arguments))
            {
                this.Predicate?.Build(transaction, arguments, extent.Filter);
            }

            if (this.Sorting != null)
            {
                foreach (var sort in this.Sorting)
                {
                    sort.Build(extent);
                }
            }

            return extent;
        }

        void IPredicateContainer.AddPredicate(IPredicate predicate) => this.Predicate = predicate;

        public void Accept(IVisitor visitor) => visitor.VisitExtent(this);
    }
}
