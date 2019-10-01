// <copyright file="SyncResponseObject.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Protocol.Remote.Sync
{
    public class SyncResponseObject
    {
        /// <summary>
        /// Gets or sets the object id.
        /// </summary>
        public string I { get; set; }

        /// <summary>
        /// Gets or sets the object type.
        /// Format is a mapping ":{key}:{value}" or a key "{key}".
        /// The key will be generated on first occurence of the ObjectType
        /// and is local to this Sync.
        /// </summary>
        public string T { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        public string V { get; set; }

        /// <summary>
        /// Gets or sets the access controls.
        /// </summary>
        public string A { get; set; }

        /// <summary>
        /// Gets or sets the denied permissions.
        /// </summary>
        public string D { get; set; }

        /// <summary>
        /// Gets or sets the roles.
        /// </summary>
        public SyncResponseRole[] R { get; set; }

        public override string ToString() => $"{this.T} [{this.I}:{this.V}]";
    }
}
