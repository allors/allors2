// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UniquelyIdentifiableExtension.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using System;

    using Allors.Meta;

    public static partial class UniquelyIdentifiableExtension
    {
        public static void CoreOnBuild(this UniquelyIdentifiable @this, ObjectOnBuild method)
        {
            if (!@this.ExistUniqueId)
            {
                @this.Strategy.SetUnitRole(MetaUniquelyIdentifiable.Instance.UniqueId.RelationType, Guid.NewGuid());
            }
        }
    }
}
