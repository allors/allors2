// <copyright file="Instanceof.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Data
{
    using System.Collections.Generic;
    using Allors.Protocol.Data;
    using Allors.Workspace.Meta;

    public class Instanceof : IPropertyPredicate
    {
        public string[] Dependencies { get; set; }

        public Instanceof(IComposite objectType = null) => this.ObjectType = objectType;

        public string Parameter { get; set; }

        public IComposite ObjectType { get; set; }

        public IPropertyType PropertyType { get; set; }

        public Predicate ToJson() =>
            new Predicate
            {
                Kind = PredicateKind.Instanceof,
                Dependencies = this.Dependencies,
                ObjectType = this.ObjectType?.Id,
                PropertyType = this.PropertyType?.Id,
            };
    }
}
