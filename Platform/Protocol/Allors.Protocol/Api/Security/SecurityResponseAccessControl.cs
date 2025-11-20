// <copyright file="SyncResponseObject.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Protocol.Remote.Security
{
    public class SecurityResponseAccessControl
    {
        /// <summary>
        /// Id
        /// </summary>
        public string i { get; set; }

        /// <summary>
        /// Version
        /// </summary>
        public string v { get; set; }

        /// <summary>
        /// Permissions
        /// </summary>
        public string p { get; set; }
    }
}
