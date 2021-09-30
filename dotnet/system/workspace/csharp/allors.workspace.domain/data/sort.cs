// <copyright file="Sort.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Data
{
    using Meta;

    public class Sort : IVisitable
    {
        public Sort(IRoleType roleType = null) => this.RoleType = roleType;

        public IRoleType RoleType { get; set; }

        public SortDirection SortDirection { get; set; }

        public void Accept(IVisitor visitor) => visitor.VisitSort(this);
    }
}
