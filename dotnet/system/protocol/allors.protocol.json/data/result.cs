// <copyright file="Result.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Protocol.Json.Data
{
    using System;

    public class Result : IVisitable
    {
        /// <summary>
        /// Select Ref
        /// </summary>
        public Guid? r { get; set; }

        /// <summary>
        /// Select
        /// </summary>
        public Select s { get; set; }

        /// <summary>
        /// Include
        /// </summary>
        public Node[] i { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string n { get; set; }

        /// <summary>
        /// Skip
        /// </summary>
        public int? k { get; set; }

        /// <summary>
        /// Take
        /// </summary>
        public int? t { get; set; }

        public void Accept(IVisitor visitor) => visitor.VisitResult(this);
    }
}
