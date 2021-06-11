// <copyright file="Fetch.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Protocol.Data
{
    public class Fetch
    {
        /// <summary>
        /// Step
        /// </summary>
        public Step step { get; set; }

        /// <summary>
        /// Include
        /// </summary>
        public Node[] include { get; set; }
    }
}
