// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GT32.cs" company="Allors bvba">
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

    public partial class GT32
    {
        public static GT32 Create(ISession session)
        {
            return (GT32)session.Create(Meta.ObjectType);
        }

        public static GT32[] Create(ISession session, int count)
        {
            return (GT32[])session.Create(Meta.ObjectType, count);
        }

        public static GT32[] Instantiate(ISession session, string[] ids)
        {
            return (GT32[])session.Instantiate(ids);
        }

        public static GT32[] Extent(ISession session)
        {
            return (GT32[])session.Extent(Meta.ObjectType).ToArray();
        }
    }
}