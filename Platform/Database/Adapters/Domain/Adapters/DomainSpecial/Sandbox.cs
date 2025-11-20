// <copyright file="Sandbox.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Allors;

    public partial class Sandbox
    {
        public static Sandbox Create(ISession session) => (Sandbox)session.Create(Meta.ObjectType);

        public static Sandbox[] Create(ISession session, int count) => (Sandbox[])session.Create(Meta.ObjectType, count);

        public static Sandbox[] Instantiate(ISession session, string[] ids) => (Sandbox[])session.Instantiate(ids);

        public static Sandbox[] Extent(ISession session) => (Sandbox[])session.Extent(Meta.ObjectType).ToArray();
    }
}
