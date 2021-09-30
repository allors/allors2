// <copyright file="PushRequestObject.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Protocol.Remote.Push
{
    public class PushRequestObject
    {
        /// <summary>
        /// Id
        /// </summary>
        public string i { get; set; }

        /// <summary>
        /// Version
        /// </summary>
        public string v { get; set; }

        /// <summary>
        /// Roles
        /// </summary>
        public PushRequestRole[] roles { get; set; }
    }
}
