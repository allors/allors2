// <copyright file="Instanceof.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Data
{
    using System.Collections.Generic;

    using Allors.Meta;
    using Allors.Protocol.Data;

    public class Instanceof : IPropertyPredicate
    {
        public string[] Dependencies { get; set; }

        public Instanceof(IComposite objectType = null) => this.ObjectType = objectType;

        public string Argument { get; set; }

        public IComposite ObjectType { get; set; }

        public IPropertyType PropertyType { get; set; }

        public Predicate Save() =>
            new Predicate
            {
                Kind = PredicateKind.Instanceof,
                ObjectType = this.ObjectType?.Id,
                PropertyType = this.PropertyType?.Id,
            };

        bool IPredicate.ShouldTreeShake(IDictionary<string, string> parameters) => this.HasMissingDependencies(parameters) || ((IPredicate)this).HasMissingArguments(parameters);

        bool IPredicate.HasMissingArguments(IDictionary<string, string> parameters) => this.Argument != null && (parameters == null || !parameters.ContainsKey(this.Argument));

        void IPredicate.Build(ISession session, IDictionary<string, string> parameters, Allors.ICompositePredicate compositePredicate)
        {
            var composite = this.Argument != null ? (IComposite)session.GetMetaObject(parameters[this.Argument]) : this.ObjectType;

            if (this.PropertyType != null)
            {
                if (this.PropertyType is IRoleType roleType)
                {
                    compositePredicate.AddInstanceof(roleType, composite);
                }
                else
                {
                    var associationType = (IAssociationType)this.PropertyType;
                    compositePredicate.AddInstanceof(associationType, composite);
                }
            }
            else
            {
                compositePredicate.AddInstanceof(composite);
            }
        }
    }
}
