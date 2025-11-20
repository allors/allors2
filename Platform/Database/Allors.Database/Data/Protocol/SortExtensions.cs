// <copyright file="SortExtensions.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Protocol.Data
{
    using Allors.Meta;

    public static class SortExtensions
    {
        public static Allors.Data.Sort Load(this Sort @this, ISession session) =>
            new Allors.Data.Sort
            {
                Descending = @this.@descending,
                RoleType = @this.roleType != null ? (IRoleType)session.Database.ObjectFactory.MetaPopulation.Find(@this.roleType.Value) : null,
            };
    }
}
