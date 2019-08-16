
// <copyright file="DerivationErrorResponse.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Protocol.Remote
{
    public class DerivationErrorResponse
    {
        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        public string M { get; set; }

        /// <summary>
        /// Gets or sets the roles.
        /// </summary>
        public string[][] R { get; set; }
    }
}
