// <copyright file="Sort.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Protocol.Data
{
    using System;

    public class Sort
    {
        /// <summary>
        /// Role Type
        /// </summary>
        public Guid? roleType { get; set; }

        /// <summary>
        /// Descending
        /// </summary>
        public bool @descending { get; set; }
    }
}
