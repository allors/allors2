// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClassWithoutUnitRoles.cs" company="Allors bvba">
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

namespace Allors.Domain
{
    using Allors;

    public partial class ClassWithoutUnitRoles 
    {
        public static ClassWithoutUnitRoles Create(ISession session)
        {
            return (ClassWithoutUnitRoles)session.Create(Meta.ObjectType);
        }

        public static ClassWithoutUnitRoles[] Create(ISession session, int count)
        {
            return (ClassWithoutUnitRoles[])session.Create(Meta.ObjectType, count);
        }

        public static ClassWithoutUnitRoles[] Instantiate(ISession session, string[] ids)
        {
            return (ClassWithoutUnitRoles[])session.Instantiate(ids);
        }

        public static ClassWithoutUnitRoles[] Extent(ISession session)
        {
            return (ClassWithoutUnitRoles[])session.Extent(Meta.ObjectType).ToArray();
        }
    }
}