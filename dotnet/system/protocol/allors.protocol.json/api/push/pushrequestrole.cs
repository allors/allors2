// <copyright file="PushRequestRole.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Protocol.Json.Api.Push
{
    public class PushRequestRole
    {
        /// <summary>
        /// Relation Type Tag
        /// </summary>
        public string t { get; set; }

        /// <summary>
        /// Set Unit Role
        /// </summary>
        public object u { get; set; }

        /// <summary>
        /// Set Composite Role
        /// </summary>
        public long? c { get; set; }

        /// <summary>
        /// Add Composites Role
        /// </summary>
        public long[] a { get; set; }

        /// <summary>
        /// Remove Composites Role
        /// </summary>
        public long[] r { get; set; }
    }
}
