// <copyright file="FetchExtensions.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Allors.Data;

    public static class FetchExtensions
    {
        public static object Get(this Fetch fetch, IObject allorsObject, IAccessControlLists acls) => fetch.Step == null ? allorsObject : fetch.Step.Get(allorsObject, acls);

        public static bool Set(this Fetch fetch, IObject allorsObject, IAccessControlLists acls, object value) => fetch.Step != null && fetch.Step.Set(allorsObject, acls, value);

        public static void Ensure(this Fetch fetch, IObject allorsObject, IAccessControlLists acls) => fetch.Step.Ensure(allorsObject, acls);
    }
}
