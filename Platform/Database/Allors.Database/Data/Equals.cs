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

        public string Parameter { get; set; }

        public Predicate Save() =>
            new Predicate
            {
                Kind = PredicateKind.Equals,
                PropertyType = this.PropertyType.Id,
                Object = this.Object?.Id.ToString(),
                Value = UnitConvert.ToString(this.Value),
                Parameter = this.Parameter,
            };

        bool IPredicate.ShouldTreeShake(IReadOnlyDictionary<string, object> arguments) => ((IPredicate)this).HasMissingArguments(arguments);

        bool IPredicate.HasMissingArguments(IReadOnlyDictionary<string, object> arguments) => this.Parameter != null && (arguments == null || !arguments.ContainsKey(this.Parameter));

        /// <inheritdoc/>
        void IPredicate.Build(ISession session, IReadOnlyDictionary<string, object> arguments, Allors.ICompositePredicate compositePredicate)
        {
            if (this.PropertyType == null)
            {
                var equals = this.Parameter != null ? arguments[this.Parameter] : this.Object;
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
                        var equals = this.Parameter != null ? arguments[this.Parameter] : this.Value;
                        if (equals != null)
                        {
                            compositePredicate.AddEquals(roleType, equals);
                        }
                    }
                    else
                    {
                        var equals = this.Parameter != null ? session.GetObject(arguments[this.Parameter]) : this.Object;
                        if (equals != null)
                        {
                            compositePredicate.AddEquals(roleType, equals);
                        }
                    }
                }
                else
                {
                    var associationType = (IAssociationType)this.PropertyType;
                    var equals = (IObject)(this.Parameter != null ? session.GetObject(arguments[this.Parameter]) : this.Object);
                    if (equals != null)
                    {
                        compositePredicate.AddEquals(associationType, equals);
                    }
                }
            }
        }
    }
}
