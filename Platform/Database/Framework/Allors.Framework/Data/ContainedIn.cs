//------------------------------------------------------------------------------------------------- 
// <copyright file="ContainedIn.cs" company="Allors bvba">
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
    using System.Linq;

    using Allors.Data.Protocol;
    using Allors.Meta;

    public class ContainedIn : IPropertyPredicate
    {
        public ContainedIn(IPropertyType propertyType = null) => this.PropertyType = propertyType;

        public IPropertyType PropertyType { get; set; }

        public IExtent Extent { get; set; }

        public IEnumerable<IObject> Objects { get; set; }

        public string Parameter { get; set; }

        bool IPredicate.ShouldTreeShake(IReadOnlyDictionary<string, object> arguments)
        {
            if (this.Parameter != null)
            {
                if (arguments == null || !arguments.ContainsKey(this.Parameter))
                {
                    return false;
                }
            }

            if (this.Extent != null)
            {
                return this.Extent.HasMissingArguments(arguments);
            }

            return false;
        }

        bool IPredicate.HasMissingArguments(IReadOnlyDictionary<string, object> arguments)
        {
            if (this.Parameter != null)
            {
                if (arguments == null || !arguments.ContainsKey(this.Parameter))
                {
                    return true;
                }
            }

            if (this.Extent != null)
            {
                return this.Extent.HasMissingArguments(arguments);
            }

            return false;
        }

        public Predicate Save()
        {
            return new Predicate
                       {
                           Kind = PredicateKind.ContainedIn,
                           PropertyType = this.PropertyType?.Id,
                           Extent = this.Extent?.Save(),
                           Values = this.Objects.Select(v => v.Id.ToString()).ToArray(),
                           Parameter = this.Parameter
                       };
        }

        void IPredicate.Build(ISession session, IReadOnlyDictionary<string, object> arguments, Allors.ICompositePredicate compositePredicate)
        {
            var objects = this.Parameter != null ? session.GetObjects(arguments[this.Parameter]) : this.Objects;

            if (this.PropertyType is IRoleType roleType)
            {
                if (objects != null)
                {
                    compositePredicate.AddContainedIn(roleType, objects);
                }
                else
                {
                    compositePredicate.AddContainedIn(roleType, this.Extent.Build(session, arguments));
                }
            }
            else
            {
                var associationType = (IAssociationType)this.PropertyType;
                if (objects != null)
                {
                    compositePredicate.AddContainedIn(associationType, objects);
                }
                else
                {
                    compositePredicate.AddContainedIn(associationType, this.Extent.Build(session, arguments));
                }
            }
        }
    }
}