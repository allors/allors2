// <copyright file="ClassWithoutRoles.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Allors;

    public partial class ClassWithoutRoles
    {
        public static ClassWithoutRoles Create(ISession session) => (ClassWithoutRoles)session.Create(Meta.ObjectType);

        public static ClassWithoutRoles[] Create(ISession session, int count) => (ClassWithoutRoles[])session.Create(Meta.ObjectType, count);

        public static ClassWithoutRoles[] Instantiate(ISession session, string[] ids) => (ClassWithoutRoles[])session.Instantiate(ids);

        public static ClassWithoutRoles[] Extent(ISession session) => (ClassWithoutRoles[])session.Extent(Meta.ObjectType).ToArray();
    }
}
