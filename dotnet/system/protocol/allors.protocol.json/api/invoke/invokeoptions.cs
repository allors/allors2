// <copyright file="InvokeOptions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Protocol.Json.Api.Invoke
{
    public class InvokeOptions
    {
        /// <summary>
        ///  Isolated
        /// </summary>
        public bool i { get; set; }

        /// <summary>
        ///  Continue On Error
        /// </summary>
        public bool c { get; set; }
    }
}
