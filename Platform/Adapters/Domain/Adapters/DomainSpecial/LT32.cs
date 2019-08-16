
// <copyright file="LT32.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Allors;

    public partial class LT32
    {
        public static LT32 Create(ISession session) => (LT32)session.Create(Meta.ObjectType);

        public static LT32[] Create(ISession session, int count) => (LT32[])session.Create(Meta.ObjectType, count);

        public static LT32[] Instantiate(ISession session, string[] ids) => (LT32[])session.Instantiate(ids);

        public static LT32[] Extent(ISession session) => (LT32[])session.Extent(Meta.ObjectType).ToArray();
    }
}
