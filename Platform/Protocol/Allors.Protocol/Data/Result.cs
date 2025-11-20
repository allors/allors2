// <copyright file="Result.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Protocol.Data
{
    using System;

    public class Result
    {
        /// <summary>
        /// FetchRef
        /// </summary>
        public Guid? fetchRef { get; set; }

        /// <summary>
        /// Fetch
        /// </summary>
        public Fetch fetch { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// Skip
        /// </summary>
        public int? skip { get; set; }

        /// <summary>
        /// Take
        /// </summary>
        public int? take { get; set; }
    }
}
