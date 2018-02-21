//------------------------------------------------------------------------------------------------- 
// <copyright file="PrefetchRule.cs" company="Allors bvba">
// Copyright 2002-2016 Allors bvba.
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
// <summary>Defines the ObjectIdInteger type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors
{
    using System;

    using Allors.Meta;

    public sealed class PrefetchRule
    {
        public PrefetchRule(IPropertyType propertyType, PrefetchPolicy prefetchPolicy)
        {
            if (propertyType == null)
            {
                throw new ArgumentNullException("propertyType");
            }

            if (prefetchPolicy != null)
            {
                var roleType = propertyType as IRoleType;
                if (roleType != null && roleType.ObjectType.IsUnit)
                {
                    throw new ArgumentException("prefetchPolicy");
                }
            }

            this.PropertyType = propertyType;
            this.PrefetchPolicy = prefetchPolicy;
        }

        public IPropertyType PropertyType { get; private set; }

        public PrefetchPolicy PrefetchPolicy { get; private set; }

        public override string ToString()
        {
            return this.PropertyType.ToString();
        }
    }
}