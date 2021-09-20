// <copyright file="Pull.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Data
{
    using System;
    using System.Collections.Generic;
    using Workspace;
    using Meta;

    public class Pull : IVisitable
    {
        public Guid? ExtentRef { get; set; }

        public Extent Extent { get; set; }

        public IObjectType ObjectType { get; set; }

        public IObject Object { get; set; }

        public long? ObjectId { get; set; }

        public Result[] Results { get; set; }

        public IDictionary<string, object> Arguments { get; set; }

        public void Accept(IVisitor visitor) => visitor.VisitPull(this);
    }
}
