// <copyright file="SyncResponse.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Protocol.Json.Api.Security
{
    public class PermissionResponse
    {
        /// <summary>
        /// Permissions
        /// </summary>
        public PermissionResponsePermission[] p { get; set; }
    }
}
