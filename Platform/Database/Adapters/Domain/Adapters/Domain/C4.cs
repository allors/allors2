// <copyright file="C4.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

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
