// <copyright file="PushRequestObject.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Protocol.Json.Api.Push
{
    /// <summary>
    ///  New objects require NI and T.
    ///  Existing objects require I and V.
    /// </summary>
    public class PushRequestObject
    {
        /// <summary>
        /// Database Id
        /// </summary>
        public long d { get; set; }

        /// <summary>
        /// Version
        /// </summary>
        public long v { get; set; }

        /// <summary>
        /// Roles
        /// </summary>
        public PushRequestRole[] r { get; set; }
    }
}
