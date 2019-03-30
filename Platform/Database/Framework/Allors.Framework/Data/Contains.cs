//------------------------------------------------------------------------------------------------- 
// <copyright file="Contains.cs" company="Allors bvba">
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

    using Allors.Meta;
    using Allors.Protocol.Data;

    public class Contains : IPropertyPredicate
    {
        public Contains(IPropertyType propertyType = null) => this.PropertyType = propertyType;

        public IPropertyType PropertyType { get; set; }

        public IObject Object { get; set; }

        public string Parameter { get; set; }

        public Predicate Save()
        {
            return new Predicate
                       {
                           Kind = PredicateKind.Contains,
                           PropertyType = this.PropertyType?.Id,
                           Object = this.Object?.Id.ToString(),
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