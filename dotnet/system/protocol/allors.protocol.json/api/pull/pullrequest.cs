// <copyright file="PullRequest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Protocol.Json.Api.Pull
{
    using Data;

    public class PullRequest
    {
        /// <summary>
        /// List of Pulls
        /// </summary>
        public Pull[] l { get; set; }

        /// <summary>
        /// Procedure
        /// </summary>
        public Procedure p { get; set; }
    }
}
