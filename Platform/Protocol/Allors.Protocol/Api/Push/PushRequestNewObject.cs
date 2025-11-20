// <copyright file="PushRequestNewObject.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Protocol.Remote.Push
{
    public class PushRequestNewObject
    {
        /// <summary>
        /// New id
        /// </summary>
        public string ni { get; set; }

        /// <summary>
        /// Object Type
        /// </summary>
        public string t { get; set; }

        /// <summary>
        /// Roles
        /// </summary>
        public PushRequestRole[] roles { get; set; }
    }
}
