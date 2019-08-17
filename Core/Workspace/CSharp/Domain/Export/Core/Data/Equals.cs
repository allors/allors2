//-------------------------------------------------------------------------------------------------
// <copyright file="Equals.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
//-------------------------------------------------------------------------------------------------

using Allors.Workspace.Meta;

namespace Allors.Workspace.Data
{
    using System.Collections.Generic;
    using Allors.Protocol.Data;

    public class Equals : IPropertyPredicate
    {
        public Equals(IPropertyType propertyType = null) => this.PropertyType = propertyType;

        /// <inheritdoc/>
        public IPropertyType PropertyType { get; set; }

        public SessionObject Object { get; set; }

        public object Value { get; set; }

        public string Parameter { get; set; }

        public Predicate ToJson() =>
            new Predicate
            {
                Kind = PredicateKind.Equals,
                PropertyType = this.PropertyType.Id,
                Object = this.Object?.Id.ToString(),
                Value = DataConvert.ToString(this.Value),
                Parameter = this.Parameter,
            };

        bool IPredicate.ShouldTreeShake(IReadOnlyDictionary<string, object> arguments) => ((IPredicate)this).HasMissingArguments(arguments);

        bool IPredicate.HasMissingArguments(IReadOnlyDictionary<string, object> arguments) => this.Parameter != null && (arguments == null || !arguments.ContainsKey(this.Parameter));
    }
}
