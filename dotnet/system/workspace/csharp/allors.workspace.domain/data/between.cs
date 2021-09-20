// <copyright file="Between.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Data
{
    using System.Collections.Generic;
    using Meta;

    public class Between : IRolePredicate
    {
        public string[] Dependencies { get; set; }

        public Between(IRoleType roleType = null) => this.RoleType = roleType;

        public IRoleType RoleType { get; set; }

        public IEnumerable<object> Values { get; set; }

        public IEnumerable<IRoleType> Paths { get; set; }

        public string Parameter { get; set; }

        public void Accept(IVisitor visitor) => visitor.VisitBetween(this);
    }
}
