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
    [Id("c28e515b-cae8-4d6b-95bf-062aec8042fc")]
    public partial class AllorsBinaryUnit : Unit
    {
        public static AllorsBinaryUnit Instance { get; internal set; }

        internal AllorsBinaryUnit()
            : base(MetaPopulation.Instance)
        {
            this.UnitTag = UnitTags.AllorsBinary;
        }
    }
}