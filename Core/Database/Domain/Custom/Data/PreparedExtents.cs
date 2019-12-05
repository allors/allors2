// <copyright file="Organisations.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    using Allors.Data;
    using Allors.Meta;

    public partial class PreparedExtents
    {
        public static readonly Guid ByName = new Guid("2A2246FD-91F8-438F-B6DB-6BA9C3481778");

        protected override void CustomSetup(Setup setup)
        {
            var merge = this.Cache.Merger().Action();

            merge(ByName, v =>
            {
                v.Description = "Organisation by name";
                v.Extent = new Filter(this.Meta.Class) { Predicate = new Equals(this.Meta.Name) { Parameter = "name" } };
            });
        }
    }
}
