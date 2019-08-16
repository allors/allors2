//------------------------------------------------------------------------------------------------- 
// <copyright file="MetaClass.cs" company="Allors bvba">
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

namespace Allors.Workspace.Meta
{
    public abstract partial class MetaClass
    {
        public abstract Class Class { get; }

        public Class ObjectType => this.Class;

        public static implicit operator Class(MetaClass metaClass) => metaClass.Class;

        public static implicit operator Composite(MetaClass metaClass) => metaClass.Class;

        public static implicit operator ObjectType(MetaClass metaClass) => metaClass.Class;
    }
}
