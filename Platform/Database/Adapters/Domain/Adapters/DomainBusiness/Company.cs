// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Company.cs" company="Allors bvba">
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

    public partial class Company
    {
        public static Company Create(ISession session)
        {
            return (Company)session.Create(Meta.ObjectType);
        }

        public static Company[] Create(ISession session, int count)
        {
            return (Company[])session.Create(Meta.ObjectType, count);
        }

        public static Company[] Instantiate(ISession session, string[] ids)
        {
            return (Company[])session.Instantiate(ids);
        }

        public static Company[] Extent(ISession session)
        {
            return (Company[])session.Extent(Meta.ObjectType).ToArray();
        }

        public static Company Create(ISession session, string name)
        {
            Company company = Create(session);
            company.Name = name;
            return company;
        }

        public static Company Create(ISession session, string name, int index)
        {
            Company company = Create(session);
            company.Name = name;
            company.Index = index;
            return company;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}