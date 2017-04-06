//------------------------------------------------------------------------------------------------- 
// <copyright file="ConcreteRoleType.cs" company="Allors bvba">
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
// <summary>Defines the IObjectType type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Workspace.Meta
{
    using System;

    public sealed partial class ConcreteRoleType
    {
        private readonly RoleType roleType;

        public ConcreteRoleType(Class @class, RoleType roleType)
        {
            this.Class = @class;
            this.roleType = roleType;
        }

        public RoleType RoleType => this.roleType;

        public RelationType RelationType => this.roleType.RelationType;

        public Class Class { get; }
        
        public static implicit operator RoleType(ConcreteRoleType concreteRoleType)
        {
            return concreteRoleType.RoleType;
        }
    }
}