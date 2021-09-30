// <copyright file="LT32UnitGT32Composite.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Allors;

    public partial class LT32UnitGT32Composite
    {
        public static LT32UnitGT32Composite Create(ISession session) => (LT32UnitGT32Composite)session.Create(Meta.ObjectType);

        public static LT32UnitGT32Composite[] Create(ISession session, int count) =>
            (LT32UnitGT32Composite[])
            session.Create(Meta.ObjectType, count);

        public static LT32UnitGT32Composite[] Instantiate(ISession session, string[] ids) =>
            (LT32UnitGT32Composite[])
            session.Instantiate(ids);

        public static LT32UnitGT32Composite[] Extent(ISession session) =>
            (LT32UnitGT32Composite[])
            session.Extent(Meta.ObjectType).ToArray();
    }
}
