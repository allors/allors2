// <copyright file="Exists.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
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
        public Exists(IPropertyType propertyType = null) => this.PropertyType = propertyType;

        public string Argument { get; set; }

        public IPropertyType PropertyType { get; set; }

        public Predicate Save() =>
            new Predicate
            {
                Kind = PredicateKind.Exists,
                PropertyType = this.PropertyType?.Id,
                Argument = this.Argument,
            };

        bool IPredicate.ShouldTreeShake(IDictionary<string, string> parameters) => ((IPredicate)this).HasMissingArguments(parameters);

        bool IPredicate.HasMissingArguments(IDictionary<string, string> parameters) => this.Argument != null && (parameters == null || !parameters.ContainsKey(this.Argument));

        void IPredicate.Build(ISession session, IDictionary<string, string> parameters, Allors.ICompositePredicate compositePredicate)
        {
            var parameter = this.Argument != null ? parameters[this.Argument] : null;
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
