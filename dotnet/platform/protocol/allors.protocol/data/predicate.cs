// <copyright file="Predicate.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Protocol.Data
{
    using System;

    public class Predicate
    {
        /// <summary>
        /// Kind
        /// </summary>
        public string kind { get; set; }

        /// <summary>
        /// Property Type
        /// </summary>
        public Guid? propertyType { get; set; }

        /// <summary>
        /// Role Type
        /// </summary>
        public Guid? roleType { get; set; }

        /// <summary>
        /// Object Type
        /// </summary>
        public Guid? objectType { get; set; }

        /// <summary>
        /// Parameter
        /// </summary>
        public string parameter { get; set; }

        /// <summary>
        /// Dependencies
        /// </summary>
        public string[] dependencies { get; set; }

        /// <summary>
        /// Operand
        /// </summary>
        public Predicate operand { get; set; }

        /// <summary>
        /// Operands
        /// </summary>
        public Predicate[] operands { get; set; }

        /// <summary>
        /// Object
        /// </summary>
        public string @object { get; set; }

        /// <summary>
        /// Objects
        /// </summary>
        public string[] objects { get; set; }

        /// <summary>
        /// Value
        /// </summary>
        public string value { get; set; }

        /// <summary>
        /// Values
        /// </summary>
        public string[] values { get; set; }

        /// <summary>
        /// Extent
        /// </summary>
        public Extent extent { get; set; }
    }
}
