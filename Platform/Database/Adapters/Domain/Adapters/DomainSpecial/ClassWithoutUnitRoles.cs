// <copyright file="ClassWithoutUnitRoles.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Allors;

    public partial class ClassWithoutUnitRoles
    {
        public static ClassWithoutUnitRoles Create(ISession session) => (ClassWithoutUnitRoles)session.Create(Meta.ObjectType);

        public static ClassWithoutUnitRoles[] Create(ISession session, int count) => (ClassWithoutUnitRoles[])session.Create(Meta.ObjectType, count);

        public static ClassWithoutUnitRoles[] Instantiate(ISession session, string[] ids) => (ClassWithoutUnitRoles[])session.Instantiate(ids);

        public static ClassWithoutUnitRoles[] Extent(ISession session) => (ClassWithoutUnitRoles[])session.Extent(Meta.ObjectType).ToArray();
    }
}
