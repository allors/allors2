// --------------------------------------------------------------------------------------------------------------------
// <copyright file="C2.cs" company="Allors bvba">
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

    public partial class C2
    {
        public static C2 Create(ISession session) => (C2)session.Create(Meta.ObjectType);

        public static C2[] Create(ISession session, int count) => (C2[])session.Create(Meta.ObjectType, count);

        public static C2 Instantiate(ISession session, long id) => (C2)session.Instantiate(id);

        public static C2[] Instantiate(ISession session, string[] ids) => (C2[])session.Instantiate(ids);

        public static C2[] Extent(ISession session) => (C2[])session.Extent(Meta.ObjectType).ToArray();

        public void AnS1234Method()
        {
        }

        public override string ToString() => this.Name;
    }
}
