// <copyright file="PushRequestNewObject.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Protocol.Json.Api.Push
{
    /// <summary>
    ///  New objects require NI and T.
    ///  Existing objects require I and V.
    /// </summary>
    public class PushRequestNewObject
    {
        /// <summary>
        /// Workspace Id
        /// </summary>
        public long w { get; set; }

        /// <summary>
        /// Object Type Tag
        /// </summary>
        public string t { get; set; }

        /// <summary>
        /// Roles
        /// </summary>
        public PushRequestRole[] r { get; set; }
    }
}
