// <copyright file="Company.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Allors;

    public partial class Company
    {
        public static Company Create(ISession session) => (Company)session.Create(Meta.ObjectType);

        public static Company[] Create(ISession session, int count) => (Company[])session.Create(Meta.ObjectType, count);

        public static Company[] Instantiate(ISession session, string[] ids) => (Company[])session.Instantiate(ids);

        public static Company[] Extent(ISession session) => (Company[])session.Extent(Meta.ObjectType).ToArray();

        public static Company Create(ISession session, string name)
        {
            var company = Create(session);
            company.Name = name;
            return company;
        }

        public static Company Create(ISession session, string name, int index)
        {
            var company = Create(session);
            company.Name = name;
            company.Index = index;
            return company;
        }

        public override string ToString() => this.Name;
    }
}
