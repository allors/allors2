
// <copyright file="C3.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Allors;

    public partial class C3
    {
        public static C3 Create(ISession session) => (C3)session.Create(Meta.ObjectType);

        public static C3[] Create(ISession session, int count) => (C3[])session.Create(Meta.ObjectType, count);

        public static C3 Instantiate(ISession session, long id) => (C3)session.Instantiate(id);

        public static C3[] Instantiate(ISession session, string[] ids) => (C3[])session.Instantiate(ids);

        public static C3[] Extent(ISession session) => (C3[])session.Extent(Meta.ObjectType).ToArray();

        public void AnS1234Method()
        {
        }

        public override string ToString() => this.Name;
    }
}
