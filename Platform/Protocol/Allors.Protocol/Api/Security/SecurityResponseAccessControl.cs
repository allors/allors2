// <copyright file="SyncResponseObject.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Protocol.Remote.Security
{
    public class SecurityResponseAccessControl
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public string I { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        public string V { get; set; }

        /// <summary>
        /// Gets or sets the permissions ids.
        /// </summary>
        public string P { get; set; }
    }
}
