//------------------------------------------------------------------------------------------------- 
// <copyright file="Equals.cs" company="Allors bvba">
// Copyright 2002-2017 Allors bvba.
// 
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// 
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
//-------------------------------------------------------------------------------------------------

namespace Allors.Data
{
    using System.Collections.Generic;

    using Allors.Data.Protocol;
    using Allors.Meta;

    public class Equals : IPropertyPredicate
    {
        public Equals(IPropertyType propertyType = null)
        {
            this.PropertyType = propertyType;
        }

        /// <inheritdoc/>
        public IPropertyType PropertyType { get; set; }

        public IObject Object { get; set; }

        public object Value { get; set; }

        public string Parameter { get; set; }

        public Predicate Save()
        {
            return new Predicate
            {
                Kind = PredicateKind.Equals,
                PropertyType = this.PropertyType.Id,
                Object = this.Object?.Id.ToString(),
                Value = Convert.ToString(this.Value),
                Parameter = this.Parameter
            };
        }

        bool IPredicate.ShouldTreeShake(IReadOnlyDictionary<string, object> arguments)
        {
            return ((IPredicate)this).HasMissingArguments(arguments);
        }

        bool IPredicate.HasMissingArguments(IReadOnlyDictionary<string, object> arguments)
        {
            return this.Parameter != null && (arguments == null || !arguments.ContainsKey(this.Parameter));
        }

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