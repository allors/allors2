// <copyright file="SyncResponseObject.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Protocol.Remote.Sync
{
    public class SyncResponseObject
    {
        /// <summary>
        /// Id
        /// </summary>
        public string i { get; set; }

        /// <summary>
        /// Object Type
        /// Format is a mapping ":{key}:{value}" or a key "{key}".
        /// The key will be generated on first occurence of the ObjectType
        /// and is local to this Sync.
        /// </summary>
        public string t { get; set; }

        /// <summary>
        /// Version
        /// </summary>
        public string v { get; set; }

        /// <summary>
        /// AccessControls
        /// </summary>
        public string a { get; set; }

        /// <summary>
        /// DeniedPermissions
        /// </summary>
        public string d { get; set; }

        /// <summary>
        /// Roles
        /// </summary>
        public SyncResponseRole[] r { get; set; }

        public override string ToString() => $"{this.t} [{this.i}:{this.v}]";
    }
}
