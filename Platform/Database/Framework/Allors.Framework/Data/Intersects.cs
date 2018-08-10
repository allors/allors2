//------------------------------------------------------------------------------------------------- 
// <copyright file="Extent.cs" company="Allors bvba">
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

using System.Collections.Generic;
using System.Linq;
using Allors.Meta;

namespace Allors.Data
{
    public class Intersects : IPropertyPredicate
    {
        public Intersects(IPropertyType propertyType = null) => this.PropertyType = propertyType;

        public IPropertyType PropertyType { get; set; }

        public IExtent Extent { get; set; }

        public IEnumerable<IObject> Objects { get; set; }

        public string Parameter { get; set; }

        void IPredicate.Build(ISession session, IReadOnlyDictionary<string, object> arguments, Allors.ICompositePredicate compositePredicate)
        {
            if (this.PropertyType is IRoleType roleType)
            {
                if (roleType.IsOne)
                {
                    if (this.Objects != null)
                    {
                        compositePredicate.AddContainedIn(roleType, this.Objects);
                    }
                    else
                    {
                        compositePredicate.AddContainedIn(roleType, this.Extent.Build(session, arguments));
                    }
                }
                else
                {
                    compositePredicate.AddContains(roleType, this.Objects.First());
                }
            }
            else
            {
                var associationType = (IAssociationType)PropertyType;
                if (associationType.IsOne)
                {
                    if (this.Objects != null)
                    {
                        compositePredicate.AddContainedIn(associationType, this.Objects);
                    }
                    else
                    {
                        compositePredicate.AddContainedIn(associationType, this.Extent.Build(session, arguments));
                    }
                }
                else
                {
                    compositePredicate.AddContains(associationType, this.Objects.First());
                }
            }
        }
    }
}