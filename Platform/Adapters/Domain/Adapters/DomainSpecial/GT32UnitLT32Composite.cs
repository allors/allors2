
// <copyright file="GT32UnitLT32Composite.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Allors;

    public partial class GT32UnitLT32Composite
    {
        public static GT32UnitLT32Composite Create(ISession session) => (GT32UnitLT32Composite)session.Create(Meta.ObjectType);

        public static GT32UnitLT32Composite[] Create(ISession session, int count) => (GT32UnitLT32Composite[])session.Create(Meta.ObjectType, count);

        public static GT32UnitLT32Composite[] Instantiate(ISession session, string[] ids) => (GT32UnitLT32Composite[])session.Instantiate(ids);

        public static GT32UnitLT32Composite[] Extent(ISession session) => (GT32UnitLT32Composite[])session.Extent(Meta.ObjectType).ToArray();
    }
}
