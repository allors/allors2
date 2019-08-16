//------------------------------------------------------------------------------------------------- 
// <copyright file="Exists.cs" company="Allors bvba">
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

using System;

namespace Allors.Data
{
    using System.Collections.Generic;

    using Allors.Meta;
    using Allors.Protocol.Data;

    public class Exists : IPropertyPredicate
    {
        public string Parameter { get; set; }

        public Exists(IPropertyType propertyType = null) => this.PropertyType = propertyType;

        public IPropertyType PropertyType { get; set; }

        public Predicate Save() =>
            new Predicate
            {
                Kind = PredicateKind.Exists,
                PropertyType = this.PropertyType?.Id,
                Parameter = this.Parameter
            };

        bool IPredicate.ShouldTreeShake(IReadOnlyDictionary<string, object> arguments) => ((IPredicate)this).HasMissingArguments(arguments);

        bool IPredicate.HasMissingArguments(IReadOnlyDictionary<string, object> arguments) => this.Parameter != null && (arguments == null || !arguments.ContainsKey(this.Parameter));

        void IPredicate.Build(ISession session, IReadOnlyDictionary<string, object> arguments, Allors.ICompositePredicate compositePredicate)
        {
            var argument = this.Parameter != null ? arguments[this.Parameter] : null;

            var exists = !(argument is bool b) || b;
            var propertyType = argument is string s && Guid.TryParse(s, out var metaObjectId) ? (IPropertyType)session.GetMetaObject(metaObjectId) : this.PropertyType;

            if (propertyType != null)
            {
                var predicate = !exists ? compositePredicate.AddNot() : compositePredicate;

                if (propertyType is IRoleType roleType)
                {
                    predicate.AddExists(roleType);
                }
                else
                {
                    var associationType = (IAssociationType)propertyType;
                    predicate.AddExists(associationType);
                }
            }
        }
    }
}
