//------------------------------------------------------------------------------------------------- 
// <copyright file="MetaInterface.cs" company="Allors bvba">
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
// <summary>Defines the AssociationType type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Meta
{
    public abstract partial class MetaInterface
    {
        public abstract Interface Interface { get; }

        public Interface ObjectType => this.Interface;

        public static implicit operator Interface(MetaInterface metaInterface)
        {
            return metaInterface.Interface;
        }

        public static implicit operator Composite(MetaInterface metaInterface)
        {
            return metaInterface.Interface;
        }

        public static implicit operator ObjectType(MetaInterface metaInterface)
        {
            return metaInterface.Interface;
        }
    }
}