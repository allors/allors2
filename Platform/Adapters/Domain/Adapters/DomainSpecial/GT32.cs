// <copyright file="GT32.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Allors;

    public partial class GT32
    {
        public static GT32 Create(ISession session) => (GT32)session.Create(Meta.ObjectType);

        public static GT32[] Create(ISession session, int count) => (GT32[])session.Create(Meta.ObjectType, count);

        public static GT32[] Instantiate(ISession session, string[] ids) => (GT32[])session.Instantiate(ids);

        public static GT32[] Extent(ISession session) => (GT32[])session.Extent(Meta.ObjectType).ToArray();
    }
}
