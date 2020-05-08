// <copyright file="Contains.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Data
{
    using System.Collections.Generic;
    using Allors.Protocol.Data;
    using Allors.Workspace.Meta;

    public class Contains : IPropertyPredicate
    {
        public string[] Dependencies { get; set; }

        public Contains(IPropertyType propertyType = null) => this.PropertyType = propertyType;

        public IPropertyType PropertyType { get; set; }

        public ISessionObject Object { get; set; }

        public string Parameter { get; set; }

        public Predicate ToJson() =>
            new Predicate
            {
                Kind = PredicateKind.Contains,
                Dependencies = this.Dependencies,
                PropertyType = this.PropertyType?.Id,
                Object = this.Object?.Id.ToString(),
                Parameter = this.Parameter,
            };
    }
}
