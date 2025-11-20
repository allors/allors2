// <copyright file="AccessControlListFactory.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System.Collections.Generic;

    public interface IAccessControlLists
    {
        User User { get; }

        IReadOnlyDictionary<AccessControl, HashSet<long>> EffectivePermissionIdsByAccessControl { get; set; }

        IAccessControlList this[IObject @object]
        {
            get;
        }
    }
}
