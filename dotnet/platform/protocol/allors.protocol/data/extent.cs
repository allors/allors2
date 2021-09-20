// <copyright file="Extent.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Protocol.Data
{
    using System;

    public class Extent
    {
        /// <summary>
        /// Kind
        /// </summary>
        public string kind { get; set; }

        /// <summary>
        /// Operands
        /// </summary>
        public Extent[] operands { get; set; }

        /// <summary>
        /// ObjectType
        /// </summary>
        public Guid? objectType { get; set; }

        /// <summary>
        /// Predicate
        /// </summary>
        public Predicate predicate { get; set; }

        /// <summary>
        /// Sorting
        /// </summary>
        public Sort[] sorting { get; set; }
    }
}
