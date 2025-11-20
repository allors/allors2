// <copyright file="Step.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Protocol.Data
{
    using System;

    public class Step
    {
        /// <summary>
        /// Property Type
        /// </summary>
        public Guid? propertyType { get; set; }

        /// <summary>
        /// Next
        /// </summary>
        public Step next { get; set; }

        /// <summary>
        /// Include
        /// </summary>
        public Node[] include { get; set; }
    }
}
