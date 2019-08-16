
// <copyright file="PushRequestNewObject.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Protocol.Remote.Push
{
    public class PushRequestNewObject
    {
        /// <summary>
        /// Gets or sets the new id.
        /// </summary>
        public string NI { get; set; }

        /// <summary>
        /// Gets or sets the object type.
        /// </summary>
        public string T { get; set; }

        public PushRequestRole[] Roles { get; set; }
    }
}
