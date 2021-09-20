// <copyright file="PushResponse.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Protocol.Json.Api.Push
{
    public class PushResponse : Response
    {
        /// <summary>
        /// New Objects
        /// </summary>
        public PushResponseNewObject[] n { get; set; }
    }
}
