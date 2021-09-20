// <copyright file="DerivationErrorResponse.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Protocol.Remote
{
    public class ResponseDerivationError
    {
        /// <summary>
        /// Error Message
        /// </summary>
        public string m { get; set; }

        /// <summary>
        /// Roles
        /// </summary>
        public string[][] r { get; set; }
    }
}
