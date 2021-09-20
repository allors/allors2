// <copyright file="Equals.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Data
{
    using System.Collections.Generic;

    using Allors.Meta;
    using Allors.Protocol.Data;

    public class Equals : IPropertyPredicate
    {
        public string[] Dependencies { get; set; }

        public Equals(IPropertyType propertyType = null) => this.PropertyType = propertyType;

        /// <inheritdoc/>
        public IPropertyType PropertyType { get; set; }

        public IObject Object { get; set; }

        public object Value { get; set; }

        public string Parameter { get; set; }

        public Predicate Save() =>
            new Predicate
            {
                kind = PredicateKind.Equals,
                propertyType = this.PropertyType.Id,
                @object = this.Object?.Id.ToString(),
                value = UnitConvert.ToString(this.Value),
                parameter = this.Parameter,
            };

        bool IPredicate.ShouldTreeShake(IDictionary<string, string> parameters) => this.HasMissingDependencies(parameters) || ((IPredicate)this).HasMissingArguments(parameters);

        bool IPredicate.HasMissingArguments(IDictionary<string, string> parameters) => this.Parameter != null && (parameters == null || !parameters.ContainsKey(this.Parameter));

        /// <inheritdoc/>
        void IPredicate.Build(ISession session, IDictionary<string, string> parameters, Allors.ICompositePredicate compositePredicate)
        {
            if (this.PropertyType == null)
            {
                var equals = this.Parameter != null ? session.Instantiate(parameters[this.Parameter]) : this.Object;
                if (equals != null)
                {
                    compositePredicate.AddEquals(this.Object);
                }
            }
            else
            {
                if (this.PropertyType is IRoleType roleType)
                {
                    if (roleType.ObjectType.IsUnit)
                    {
                        var equals = this.Parameter != null ? UnitConvert.Parse(roleType.ObjectType.Id, parameters[this.Parameter]) : this.Value;
                        if (equals != null)
                        {
                            compositePredicate.AddEquals(roleType, equals);
                        }
                    }
                    else
                    {
                        var equals = this.Parameter != null ? session.GetObject(parameters[this.Parameter]) : this.Object;
                        if (equals != null)
                        {
                            compositePredicate.AddEquals(roleType, equals);
                        }
                    }
                }
                else
                {
                    var associationType = (IAssociationType)this.PropertyType;
                    var equals = (IObject)(this.Parameter != null ? session.GetObject(parameters[this.Parameter]) : this.Object);
                    if (equals != null)
                    {
                        compositePredicate.AddEquals(associationType, equals);
                    }
                }
            }
        }
    }
}
