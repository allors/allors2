// <copyright file="SortExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
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
                Descending = @this.Descending,
                RoleType = @this.RoleType != null ? (IRoleType)session.Database.ObjectFactory.MetaPopulation.Find(@this.RoleType.Value) : null,
            };
    }
}
