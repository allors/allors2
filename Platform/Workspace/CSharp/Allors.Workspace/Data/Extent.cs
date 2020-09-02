// <copyright file="Filter.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using Allors.Protocol.Data;

    using Allors.Workspace.Meta;

    public class Extent : IExtent, IPredicateContainer
    {
        public Extent(IComposite objectType) => this.ObjectType = objectType;

        public IComposite ObjectType { get; set; }

        public IPredicate Predicate { get; set; }

        public Sort[] Sorting { get; set; }

        void IPredicateContainer.AddPredicate(IPredicate predicate) => this.Predicate = predicate;

        public Protocol.Data.Extent ToJson() =>
            new Protocol.Data.Extent
            {
                Kind = ExtentKind.Extent,
                ObjectType = this.ObjectType?.Id,
                Predicate = this.Predicate?.ToJson(),
                Sorting = this.Sorting?.Select(v => new Protocol.Data.Sort { Descending = v.Descending, RoleType = v.RoleType?.Id }).ToArray(),
            };
    }
}
