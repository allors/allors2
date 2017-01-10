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
    [Id("6dc0a1a8-88a4-4614-adb4-92dd3d017c0e")]
    public partial class AllorsUniqueUnit : Unit
    {
        public static AllorsUniqueUnit Instance { get; internal set; }

        internal AllorsUniqueUnit()
            : base(MetaPopulation.Instance)
        {
            this.UnitTag = UnitTags.AllorsUnique;
        }
    }
}