// <copyright file="InvokeRequest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Protocol.Remote.Invoke
{
    public class InvokeRequest
    {
        /// <summary>
        /// The id.
        /// </summary>
        public Invocation[] I { get; set; }

        /// <summary>
        /// The version.
        /// </summary>
        public InvokeOptions O { get; set; }
    }
}
