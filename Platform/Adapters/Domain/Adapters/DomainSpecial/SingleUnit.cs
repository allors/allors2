// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SingleUnit.cs" company="Allors bvba">
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

    public partial class SingleUnit
    {
        public static SingleUnit Create(ISession session) => (SingleUnit)session.Create(Meta.ObjectType);

        public static SingleUnit[] Create(ISession session, int count) => (SingleUnit[])session.Create(Meta.ObjectType, count);

        public static SingleUnit[] Instantiate(ISession session, string[] ids) => (SingleUnit[])session.Instantiate(ids);

        public static SingleUnit[] Extent(ISession session) => (SingleUnit[])session.Extent(Meta.ObjectType).ToArray();
    }
}
