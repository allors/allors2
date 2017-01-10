//------------------------------------------------------------------------------------------------- 
// <copyright file="CoreDomain.cs" company="Allors bvba">
// Copyright 2002-2013 Allors bvba.
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
// <summary>Defines the ObjectType type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Meta
{
    [Id("c4c09343-61d3-418c-ade2-fe6fd588f128")]
    public partial class AllorsDateTimeUnit : Unit
    {
        public static AllorsDateTimeUnit Instance { get; internal set; }

        internal AllorsDateTimeUnit()
            : base(MetaPopulation.Instance)
        {
            this.UnitTag = UnitTags.AllorsDateTime;
        }
    }
}