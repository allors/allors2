// <copyright file="C1.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Allors;

    public partial class C1
    {
        public static C1 Create(ISession session) => (C1)session.Create(Meta.ObjectType);

        public static C1[] Create(ISession session, int count) => (C1[])session.Create(Meta.ObjectType, count);

        public static C1 Instantiate(ISession session, long id) => (C1)session.Instantiate(id);

        public static C1[] Instantiate(ISession session, string[] ids) => (C1[])session.Instantiate(ids);

        public static C1[] Extent(ISession session) => (C1[])session.Extent(Meta.ObjectType).ToArray();

        public void AnS1234Method()
        {
        }

        public override string ToString() => this.Name;
    }
}
