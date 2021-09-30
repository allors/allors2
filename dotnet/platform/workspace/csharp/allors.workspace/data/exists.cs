// <copyright file="Exists.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Data
{
    using Allors.Protocol.Data;
    using Allors.Workspace.Meta;

    public class Exists : IPropertyPredicate
    {
        public string[] Dependencies { get; set; }

        public Exists(IPropertyType propertyType = null) => this.PropertyType = propertyType;

        public string Parameter { get; set; }

        public IPropertyType PropertyType { get; set; }

        public Predicate ToJson() =>
            new Predicate
            {
                kind = PredicateKind.Exists,
                dependencies = this.Dependencies,
                propertyType = this.PropertyType?.Id,
                parameter = this.Parameter,
            };
    }
}
