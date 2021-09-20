// <copyright file="SyncResponseObject.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Protocol.Json.Api.Sync
{
    public class SyncResponseObject
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
        /// Class Tag
        /// </summary>
        public string c { get; set; }

        /// <summary>
        /// Sorted Grants
        /// </summary>
        public long[] g { get; set; }

        /// <summary>
        /// Sorted Revocations
        /// </summary>
        public long[] r { get; set; }

        /// <summary>
        /// Roles
        /// </summary>
        public SyncResponseRole[] ro { get; set; }

        public override string ToString() => $"{this.c} [{this.i}:{this.v}]";
    }
}
