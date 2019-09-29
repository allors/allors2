// <copyright file="SyncResponse.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Protocol.Remote.Sync
{
    public class SyncResponse
    {
        public SyncResponseAccessControl[] AccessControls { get; set; }

        public SyncResponseObject[] Objects { get; set; }

        public SyncResponsePermission[] Permissions { get; set; }
    }
}
