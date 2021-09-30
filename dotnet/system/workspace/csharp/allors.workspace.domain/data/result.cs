// <copyright file="IPullResult.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Data
{
    using System;
    using System.Collections.Generic;

    public class Result : IVisitable
    {
        public Guid? SelectRef { get; set; }

        public Select Select { get; set; }

        public IEnumerable<Node> Include { get; set; }

        public string Name { get; set; }

        public int? Skip { get; set; }

        public int? Take { get; set; }

        public void Accept(IVisitor visitor) => visitor.VisitResult(this);
    }
}
