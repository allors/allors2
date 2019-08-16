
// <copyright file="C2.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

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
