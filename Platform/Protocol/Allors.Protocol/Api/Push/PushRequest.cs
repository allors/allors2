// <copyright file="PushRequest.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Protocol.Remote.Push
{
    public class PushRequest
    {
        /// <summary>
        /// New Objects
        /// </summary>
        public PushRequestNewObject[] newObjects { get; set; }

        /// <summary>
        /// Objects
        /// </summary>
        public PushRequestObject[] objects { get; set; }
    }
}
