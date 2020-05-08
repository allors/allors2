// <copyright file="ContainedIn.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using Allors.Protocol.Data;
    using Allors.Workspace;
    using Allors.Workspace.Meta;

    public class ContainedIn : IPropertyPredicate
    {
        public string[] Dependencies { get; set; }

        public ContainedIn(IPropertyType propertyType = null) => this.PropertyType = propertyType;

        public IPropertyType PropertyType { get; set; }

        public IExtent Extent { get; set; }

        public IEnumerable<ISessionObject> Objects { get; set; }

        public string Parameter { get; set; }

        public Predicate ToJson() =>
            new Predicate
            {
                Kind = PredicateKind.ContainedIn,
                Dependencies = this.Dependencies,
                PropertyType = this.PropertyType?.Id,
                Extent = this.Extent?.ToJson(),
                Values = this.Objects.Select(v => v.Id.ToString()).ToArray(),
                Parameter = this.Parameter,
            };
    }
}
