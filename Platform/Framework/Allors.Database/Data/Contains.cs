//-------------------------------------------------------------------------------------------------
// <copyright file="Contains.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
//-------------------------------------------------------------------------------------------------

namespace Allors.Data
{
    using System.Collections.Generic;

    using Allors.Meta;
    using Allors.Protocol.Data;

    public class Contains : IPropertyPredicate
    {
        public Contains(IPropertyType propertyType = null) => this.PropertyType = propertyType;

        public IPropertyType PropertyType { get; set; }

        public IObject Object { get; set; }

        public string Parameter { get; set; }

        public Predicate Save() =>
            new Predicate
            {
                Kind = PredicateKind.Contains,
                PropertyType = this.PropertyType?.Id,
                Object = this.Object?.Id.ToString(),
                Parameter = this.Parameter,
            };

        bool IPredicate.ShouldTreeShake(IReadOnlyDictionary<string, object> arguments) => ((IPredicate)this).HasMissingArguments(arguments);

        bool IPredicate.HasMissingArguments(IReadOnlyDictionary<string, object> arguments) => this.Parameter != null && (arguments == null || !arguments.ContainsKey(this.Parameter));

        void IPredicate.Build(ISession session, IReadOnlyDictionary<string, object> arguments, Allors.ICompositePredicate compositePredicate)
        {
            var containedObject = this.Parameter != null ? session.GetObject(arguments[this.Parameter]) : this.Object;

            if (this.PropertyType is IRoleType roleType)
            {
                compositePredicate.AddContains(roleType, containedObject);
            }
            else
            {
                var associationType = (IAssociationType)this.PropertyType;
                compositePredicate.AddContains(associationType, containedObject);
            }
        }
    }
}
