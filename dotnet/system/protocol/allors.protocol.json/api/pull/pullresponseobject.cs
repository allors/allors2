// <copyright file="PullResponseObject.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Protocol.Json.Api.Pull
{
    public class PullResponseObject
    {
        /// <summary>
        /// Id
        /// </summary>
        public long i { get; set; }

        /// <summary>
        /// Version
        /// </summary>
        public long v { get; set; }

        /// <summary>
        /// Sorted Grants
        /// </summary>
        public long[] g { get; set; }

        /// <summary>
        /// Sorted Revocations
        /// </summary>
        public long[] r { get; set; }
    }
}
