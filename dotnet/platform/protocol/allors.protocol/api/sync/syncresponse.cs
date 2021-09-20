// <copyright file="SyncResponse.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Protocol.Remote.Sync
{
    public class SyncResponse
    {
        /// <summary>
        /// AccessControls
        /// </summary>
        public string[][] accessControls { get; set; }

        /// <summary>
        /// Objects
        /// </summary>
        public SyncResponseObject[] objects { get; set; }
    }
}
