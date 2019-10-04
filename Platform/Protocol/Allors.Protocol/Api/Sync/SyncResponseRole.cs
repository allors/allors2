// <copyright file="SyncResponseObject.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Protocol.Remote.Sync
{
    using System.Diagnostics;

    [DebuggerDisplay("{V} [{T}]")]
    public class SyncResponseRole
    {
        /// <summary>
        /// Gets or sets the role type.
        /// </summary>
        public string T { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public string V { get; set; }
    }
}
