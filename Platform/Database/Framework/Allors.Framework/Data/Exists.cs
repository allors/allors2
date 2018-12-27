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

namespace Allors.Data
{
    using System.Collections.Generic;

    using Allors.Data.Protocol;
    using Allors.Meta;

    public class Exists : IPropertyPredicate
    {
        public string Parameter { get; set; }

        public Exists(IPropertyType propertyType = null)
        {
            this.PropertyType = propertyType;
        }

        public IPropertyType PropertyType { get; set; }
        
        public Predicate Save()
        {
            return new Predicate
                       {
                           Kind = PredicateKind.Exists,
                           PropertyType = this.PropertyType?.Id
                       };
        }

        bool IPredicate.ShouldTreeShake(IReadOnlyDictionary<string, object> arguments)
        {
            return ((IPredicate)this).HasMissingArguments(arguments);
        }

        bool IPredicate.HasMissingArguments(IReadOnlyDictionary<string, object> arguments)
        {
            return this.Parameter != null && arguments != null && !arguments.ContainsKey(this.Parameter);
        }

        void IPredicate.Build(ISession session, IReadOnlyDictionary<string, object> arguments, Allors.ICompositePredicate compositePredicate)
        {
            object argument = null;
            if (this.Parameter != null)
            {
                if (arguments == null || !arguments.TryGetValue(this.Parameter, out argument))
                {
                    return;
                }
            }

            var propertyType = argument != null ? (IPropertyType)session.GetMetaObject(argument) : this.PropertyType;

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