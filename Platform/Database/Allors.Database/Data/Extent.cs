// <copyright file="Filter.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using Allors.Meta;
    using Allors.Protocol.Data;

    public class Extent : IExtent, IPredicateContainer
    {
        public Extent(IComposite objectType) => this.ObjectType = objectType;

        public IComposite ObjectType { get; set; }

        public IPredicate Predicate { get; set; }

        public Sort[] Sorting { get; set; }

        bool IExtent.HasMissingArguments(IDictionary<string, string> parameters) => this.Predicate != null && this.Predicate.HasMissingArguments(parameters);

        public Allors.Extent Build(ISession session, IDictionary<string, string> parameters = null)
        {
            var extent = session.Extent(this.ObjectType);

            if (this.Predicate != null && !this.Predicate.ShouldTreeShake(parameters))
            {
                this.Predicate?.Build(session, parameters, extent.Filter);
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

        public Protocol.Data.Extent Save() =>
            new Protocol.Data.Extent
            {
                kind = ExtentKind.Extent,
                objectType = this.ObjectType?.Id,
                predicate = this.Predicate?.Save(),
                sorting = this.Sorting?.Select(v => new Protocol.Data.Sort { @descending = v.Descending, roleType = v.RoleType?.Id }).ToArray(),
            };
    }
}
