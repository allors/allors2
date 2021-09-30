// <copyright file="Pull.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Protocol.Json.Data
{
    using System;
    using System.Collections.Generic;

    public class Pull : IVisitable
    {
        /// <summary>
        /// ExtentRef
        /// </summary>
        public Guid? er { get; set; }

        /// <summary>
        /// Extent
        /// </summary>
        public Extent e { get; set; }

        /// <summary>
        /// Object Type Tag
        /// </summary>
        public string t { get; set; }

        /// <summary>
        /// Object
        /// </summary>
        public long? o { get; set; }

        /// <summary>
        /// Results
        /// </summary>
        public Result[] r { get; set; }

        /// <summary>
        /// Arguments
        /// </summary>
        public IDictionary<string, object> a { get; set; }

        public void Accept(IVisitor visitor) => visitor.VisitPull(this);
    }
}
