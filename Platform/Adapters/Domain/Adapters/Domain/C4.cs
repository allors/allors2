// --------------------------------------------------------------------------------------------------------------------
// <copyright file="C4.cs" company="Allors bvba">
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

    public partial class C4
    {
        public static C4 Create(ISession session) => (C4)session.Create(Meta.ObjectType);

        public static C4[] Create(ISession session, int count) => (C4[])session.Create(Meta.ObjectType, count);

        public static C4 Instantiate(ISession session, long id) => (C4)session.Instantiate(id);

        public static C4[] Instantiate(ISession session, string[] ids) => (C4[])session.Instantiate(ids);

        public static C4[] Extent(ISession session) => (C4[])session.Extent(Meta.ObjectType).ToArray();

        public void AnS1234Method()
        {
        }

        public override string ToString() => this.Name;
    }
}
