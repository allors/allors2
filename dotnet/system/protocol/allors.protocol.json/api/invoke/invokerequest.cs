// <copyright file="InvokeRequest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Protocol.Json.Api.Invoke
{
    public class InvokeRequest
    {
        /// <summary>
        ///  List of Invocations
        /// </summary>
        public Invocation[] l { get; set; }

        /// <summary>
        /// Options
        /// </summary>
        public InvokeOptions o { get; set; }
    }
}
