// <copyright file="Filter.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Data
{
    using Meta;

    public class Filter : Extent, IPredicateContainer
    {
        public Filter(IComposite objectType) => this.ObjectType = objectType;

        public IComposite ObjectType { get; set; }

        public IPredicate Predicate { get; set; }

        public Sort[] Sorting { get; set; }

        void IPredicateContainer.AddPredicate(IPredicate predicate) => this.Predicate = predicate;

        public void Accept(IVisitor visitor) => visitor.VisitFilter(this);
    }
}
