// <copyright file="Extent.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Protocol.Json.Data
{
    public class Extent : IVisitable
    {
        /// <summary>
        /// Kind
        /// </summary>
        public ExtentKind k { get; set; }

        /// <summary>
        /// Operands
        /// </summary>
        public Extent[] o { get; set; }

        /// <summary>
        /// Object Type Tag
        /// </summary>
        public string t { get; set; }

        /// <summary>
        /// Predicate
        /// </summary>
        public Predicate p { get; set; }

        /// <summary>
        /// Sorting
        /// </summary>
        public Sort[] s { get; set; }

        public void Accept(IVisitor visitor) => visitor.VisitExtent(this);
    }
}
