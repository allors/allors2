// <copyright file="SyncResponseObject.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Protocol.Remote.Sync
{
    public class SyncResponseObject
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
        /// Gets or sets the object type.
        /// </summary>
        public string T { get; set; }

        public object[][] Roles { get; set; }

        public string[][] Methods { get; set; }

        public override string ToString() => $"{this.T} [{this.I}:{this.V}]";
    }
}
