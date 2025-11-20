// <copyright file="SyncRequest.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Protocol.Remote.Security
{
    public class SecurityRequest
    {
        /// <summary>
        /// AccessControls
        /// </summary>
        public string[] accessControls { get; set; }

        /// <summary>
        /// Permissions
        /// </summary>
        public string[] permissions { get; set; }
    }
}
