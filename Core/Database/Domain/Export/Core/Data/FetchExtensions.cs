// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FetchExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using Allors.Data;

    public static class FetchExtensions
    {
        public static object Get(this Fetch fetch, IObject allorsObject, IAccessControlListFactory aclFactory) => fetch.Step == null ? allorsObject : fetch.Step.Get(allorsObject, aclFactory);

        public static bool Set(this Fetch fetch, IObject allorsObject, IAccessControlListFactory aclFactory, object value) => fetch.Step != null && fetch.Step.Set(allorsObject, aclFactory, value);

        public static void Ensure(this Fetch fetch, IObject allorsObject, IAccessControlListFactory aclFactory) => fetch.Step.Ensure(allorsObject, aclFactory);
    }
}
