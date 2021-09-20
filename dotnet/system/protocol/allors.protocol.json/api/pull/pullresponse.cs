// <copyright file="PullResponse.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Protocol.Json.Api.Pull
{
    using System.Collections.Generic;

    public class PullResponse : Response
    {
        /// <summary>
        /// Collections
        /// </summary>
        public IDictionary<string, long[]> c { get; set; }

        /// <summary>
        /// Objects
        /// </summary>
        public IDictionary<string, long> o { get; set; }

        /// <summary>
        /// Values
        /// </summary>
        public IDictionary<string, object> v { get; set; }

        /// <summary>
        /// Pool
        /// </summary>
        public PullResponseObject[] p { get; set; }

        /// <summary>
        /// Grants [id, version]
        /// </summary>
        public long[][] g { get; set; }

        /// <summary>
        /// Revocations [id, version]
        /// </summary>
        public long[][] r { get; set; }
    }
}
