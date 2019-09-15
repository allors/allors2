// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReferenceSubjectTest.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
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
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Database.Adapters
{
    public abstract class ReferenceSubjectTest : ReferenceTest
    {
        // Thorough
        //int associationCount = 4;
        //int roleCount = 15;
        //int roleGroupCount = 5;
        
        // Quick
        private const int AssociationCount = 3;
        private const int RoleCount = 8;
        private const int RoleGroupCount = 4;

        public override int GetAssociationCount()
        {
            return AssociationCount;
        }

        public override int GetRoleCount()
        {
            return RoleCount;
        }

        public override int GetRoleGroupCount()
        {
            return RoleGroupCount;
        }

        public override int GetRolesPerGroup()
        {
            return this.GetRoleCount() / this.GetRoleGroupCount();
        }
    }
}
