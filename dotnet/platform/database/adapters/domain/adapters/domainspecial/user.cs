// <copyright file="User.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Allors;

    public partial class User
    {
        public static User Create(ISession session) => (User)session.Create(Meta.ObjectType);

        public static User[] Create(ISession session, int count) => (User[])session.Create(Meta.ObjectType, count);

        public static User[] Instantiate(ISession session, string[] ids) => (User[])session.Instantiate(ids);

        public static User[] Extent(ISession session) => (User[])session.Extent(Meta.ObjectType).ToArray();
    }
}
