//-------------------------------------------------------------------------------------------------
// <copyright file="Exists.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
//-------------------------------------------------------------------------------------------------

using Allors.Workspace.Meta;

namespace Allors.Workspace.Data
{
    using System.Collections.Generic;
    using Allors.Protocol.Data;

    public class Exists : IPropertyPredicate
    {
        public string Parameter { get; set; }

        public Exists(IPropertyType propertyType = null) => this.PropertyType = propertyType;

        public IPropertyType PropertyType { get; set; }

        public Predicate ToJson() =>
            new Predicate
            {
                Kind = PredicateKind.Exists,
                PropertyType = this.PropertyType?.Id,
                Parameter = this.Parameter,
            };

        bool IPredicate.ShouldTreeShake(IReadOnlyDictionary<string, object> arguments) => ((IPredicate)this).HasMissingArguments(arguments);

        bool IPredicate.HasMissingArguments(IReadOnlyDictionary<string, object> arguments) => this.Parameter != null && (arguments == null || !arguments.ContainsKey(this.Parameter));
    }
}
