// <copyright file="InvokeRequest.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Protocol.Remote.Invoke
{
    public class InvokeRequest
    {
        /// <summary>
        /// Invocations
        /// </summary>
        public Invocation[] i { get; set; }

        /// <summary>
        /// Invoke Options
        /// </summary>
        public InvokeOptions o { get; set; }
    }
}
