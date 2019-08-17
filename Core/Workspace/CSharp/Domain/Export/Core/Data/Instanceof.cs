//-------------------------------------------------------------------------------------------------
// <copyright file="Instanceof.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
//-------------------------------------------------------------------------------------------------

namespace Allors.Workspace.Data
{
    using System.Collections.Generic;
    using Allors.Protocol.Data;
    using Allors.Workspace.Meta;

    public class Instanceof : IPropertyPredicate
    {
        public string Parameter { get; set; }

        public Instanceof(IComposite objectType = null) => this.ObjectType = objectType;

        public IComposite ObjectType { get; set; }

        public IPropertyType PropertyType { get; set; }

        public Predicate ToJson() =>
            new Predicate
            {
                Kind = PredicateKind.Instanceof,
                ObjectType = this.ObjectType?.Id,
                PropertyType = this.PropertyType?.Id,
            };

        bool IPredicate.ShouldTreeShake(IReadOnlyDictionary<string, object> arguments) => ((IPredicate)this).HasMissingArguments(arguments);

        bool IPredicate.HasMissingArguments(IReadOnlyDictionary<string, object> arguments) => this.Parameter != null && (arguments == null || !arguments.ContainsKey(this.Parameter));
    }
}
