// <copyright file="Pull.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Protocol.Data
{
    using System;
    using System.Collections.Generic;

    public class Pull
    {
        /// <summary>
        /// Extent Ref
        /// </summary>
        public Guid? extentRef { get; set; }

        /// <summary>
        /// Extent
        /// </summary>
        public Extent extent { get; set; }

        /// <summary>
        /// Object Type
        /// </summary>
        public Guid? objectType { get; set; }

        /// <summary>
        /// Object
        /// </summary>
        public string @object { get; set; }

        /// <summary>
        /// Results
        /// </summary>
        public Result[] results { get; set; }

        /// <summary>
        /// Parameters
        /// </summary>
        public IDictionary<string, string> parameters { get; set; }
    }
}
