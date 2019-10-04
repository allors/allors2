// <copyright file="SyncResponse.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Protocol.Remote.Security
{
    public class SecurityResponse
    {
        public SecurityResponseAccessControl[] AccessControls { get; set; }

        public string[][] Permissions { get; set; }
    }
}
