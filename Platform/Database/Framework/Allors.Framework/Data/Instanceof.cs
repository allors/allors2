//------------------------------------------------------------------------------------------------- 
// <copyright file="Instanceof.cs" company="Allors bvba">
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

    public class Instanceof : IPropertyPredicate
    {
        public Instanceof(IComposite objectType = null)
        {
            this.ObjectType = objectType;
        }

        public IComposite ObjectType { get; set; }

        public IPropertyType PropertyType { get; set; }

        public Predicate Save()
        {
            return new Predicate
                       {
                           Kind = PredicateKind.Instanceof,
                           ObjectType = this.ObjectType?.Id,
                           PropertyType = this.PropertyType?.Id
                       };
        }

        void IPredicate.Build(ISession session, IReadOnlyDictionary<string, object> arguments, Allors.ICompositePredicate compositePredicate)
        {
            if (this.PropertyType != null)
            {
                if (this.PropertyType is IRoleType roleType)
                {
                    compositePredicate.AddInstanceof(roleType, this.ObjectType);
                }
                else
                {
                    var associationType = (IAssociationType)PropertyType;
                    compositePredicate.AddInstanceof(associationType, this.ObjectType);
                }
            }
            else
            {
                compositePredicate.AddInstanceof(this.ObjectType);
            }
        }
    }
}