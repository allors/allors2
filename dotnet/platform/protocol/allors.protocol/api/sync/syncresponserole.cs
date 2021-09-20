// <copyright file="SyncResponseObject.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Protocol.Remote.Sync
{
    using System.Diagnostics;

    [DebuggerDisplay("{v} [{t}]")]
    public class SyncResponseRole
    {
        /// <summary>
        /// Role Type
        /// </summary>
        public string t { get; set; }

        /// <summary>
        /// Value
        /// </summary>
        public string v { get; set; }
    }
}
