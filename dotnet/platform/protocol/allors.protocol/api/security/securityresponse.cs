// <copyright file="SyncResponse.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Protocol.Remote.Security
{
    public class SecurityResponse
    {
        /// <summary>
        /// AccessControls
        /// </summary>
        public SecurityResponseAccessControl[] accessControls { get; set; }

        /// <summary>
        /// Permissions
        /// </summary>
        public string[][] permissions { get; set; }
    }
}
