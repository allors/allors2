// <copyright file="ContainedIn.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
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
                kind = PredicateKind.ContainedIn,
                dependencies = this.Dependencies,
                propertyType = this.PropertyType?.Id,
                extent = this.Extent?.ToJson(),
                values = this.Objects?.Select(v => v.Id.ToString()).ToArray(),
                parameter = this.Parameter,
            };
    }
}
