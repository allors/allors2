// <copyright file="SingleUnit.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

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
