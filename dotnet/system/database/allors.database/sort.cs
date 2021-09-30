// <copyright file="Sort.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database
{
    using Meta;

    public class Sort
    {
        public IRoleType RoleType { get; set; }

        public SortDirection Direction { get; set; }
    }
}
