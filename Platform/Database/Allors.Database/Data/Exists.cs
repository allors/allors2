// <copyright file="Exists.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Data
{
    using System;
    using System.Collections.Generic;

    using Allors.Meta;
    using Allors.Protocol.Data;

    public class Exists : IPropertyPredicate
    {
        public string[] Dependencies { get; set; }

        public Exists(IPropertyType propertyType = null) => this.PropertyType = propertyType;

        public string Parameter { get; set; }

        public IPropertyType PropertyType { get; set; }

        public Predicate Save() =>
            new Predicate
            {
                kind = PredicateKind.Exists,
                propertyType = this.PropertyType?.Id,
                parameter = this.Parameter,
            };

        bool IPredicate.ShouldTreeShake(IDictionary<string, string> parameters) => this.HasMissingDependencies(parameters) || ((IPredicate)this).HasMissingArguments(parameters);

        bool IPredicate.HasMissingArguments(IDictionary<string, string> parameters) => this.Parameter != null && (parameters == null || !parameters.ContainsKey(this.Parameter));

        void IPredicate.Build(ISession session, IDictionary<string, string> parameters, Allors.ICompositePredicate compositePredicate)
        {
            var parameter = this.Parameter != null ? parameters[this.Parameter] : null;
            var propertyType = Guid.TryParse(parameter, out var metaObjectId) ? (IPropertyType)session.GetMetaObject(metaObjectId) : this.PropertyType;

            if (propertyType != null)
            {
                if (propertyType is IRoleType roleType)
                {
                    compositePredicate.AddExists(roleType);
                }
                else
                {
                    var associationType = (IAssociationType)propertyType;
                    compositePredicate.AddExists(associationType);
                }
            }
        }
    }
}
