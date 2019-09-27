// <copyright file="PushRequestRole.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Protocol.Remote.Push
{
    public class PushRequestRole
    {
        /// <summary>
        /// Gets or sets the role type.
        /// </summary>
        public string T { get; set; }

        /// <summary>
        /// Gets or sets the set role.
        /// </summary>
        public string S { get; set; }

        /// <summary>
        /// Gets or sets the add roles.
        /// </summary>
        public string[] A { get; set; }

        /// <summary>
        /// Gets or sets the remove roles.
        /// </summary>
        public string[] R { get; set; }
    }
}
