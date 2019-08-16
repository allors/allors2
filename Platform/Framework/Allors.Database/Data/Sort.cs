//-------------------------------------------------------------------------------------------------
// <copyright file="Sort.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
//-------------------------------------------------------------------------------------------------

namespace Allors.Data
{
    using Allors.Meta;

    public class Sort
    {
        public Sort(IRoleType roleType = null) => this.RoleType = roleType;

        public IRoleType RoleType { get; set; }

        public bool Descending { get; set; }

        public void Build(Extent extent) => extent.AddSort(RoleType, this.Descending ? SortDirection.Descending : SortDirection.Ascending);
    }
}
