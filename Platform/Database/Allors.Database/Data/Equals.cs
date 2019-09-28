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
        public Equals(IPropertyType propertyType = null) => this.PropertyType = propertyType;

        /// <inheritdoc/>
        public IPropertyType PropertyType { get; set; }

        public IObject Object { get; set; }

        public object Value { get; set; }

        public string Argument { get; set; }

        public Predicate Save() =>
            new Predicate
            {
                Kind = PredicateKind.Equals,
                PropertyType = this.PropertyType.Id,
                Object = this.Object?.Id.ToString(),
                Value = UnitConvert.ToString(this.Value),
                Argument = this.Argument,
            };

        bool IPredicate.ShouldTreeShake(IDictionary<string, string> parameters) => ((IPredicate)this).HasMissingArguments(parameters);

        bool IPredicate.HasMissingArguments(IDictionary<string, string> parameters) => this.Argument != null && (parameters == null || !parameters.ContainsKey(this.Argument));

        /// <inheritdoc/>
        void IPredicate.Build(ISession session, IDictionary<string, string> parameters, Allors.ICompositePredicate compositePredicate)
        {
            if (this.PropertyType == null)
            {
                var equals = this.Argument != null ? session.Instantiate(parameters[this.Argument]) : this.Object;
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
                        var equals = this.Argument != null ? parameters[this.Argument] : this.Value;
                        if (equals != null)
                        {
                            compositePredicate.AddEquals(roleType, equals);
                        }
                    }
                    else
                    {
                        var equals = this.Argument != null ? session.GetObject(parameters[this.Argument]) : this.Object;
                        if (equals != null)
                        {
                            compositePredicate.AddEquals(roleType, equals);
                        }
                    }
                }
                else
                {
                    var associationType = (IAssociationType)this.PropertyType;
                    var equals = (IObject)(this.Argument != null ? session.GetObject(parameters[this.Argument]) : this.Object);
                    if (equals != null)
                    {
                        compositePredicate.AddEquals(associationType, equals);
                    }
                }
            }
        }
    }
}
