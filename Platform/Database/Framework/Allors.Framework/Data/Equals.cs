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

using System;
using System.Collections.Generic;
using Allors.Meta;

namespace Allors.Data
{
    public class Equals : IPropertyPredicate
    {
        public Equals(IPropertyType propertyType = null)
        {
            this.PropertyType = propertyType;
        }

        public IPropertyType PropertyType { get; set; }

        public IObject Object { get; set; }

        public object Value { get; set; }

        public string Parameter { get; set; }

        void IPredicate.Build(ISession session, IDictionary<string, object> arguments, Allors.ICompositePredicate compositePredicate)
        {
            if (this.PropertyType == null)
            {
                compositePredicate.AddEquals(this.Object);
            }

            if (this.PropertyType is IRoleType roleType)
            {
                compositePredicate.AddEquals(roleType, this.Object ?? this.Value);
            }
            else
            {
                var associationType = (IAssociationType)PropertyType;
                compositePredicate.AddEquals(associationType, this.Object);
            }
        }
    }
}