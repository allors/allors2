// <copyright file="PullRequest.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Protocol.Remote.Pull
{
    using Allors.Protocol.Data;

    public class PullRequest
    {
        /// <summary>
        /// Pulls
        /// </summary>
        public Pull[] p { get; set; }
    }
}
