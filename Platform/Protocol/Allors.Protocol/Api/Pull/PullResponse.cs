// <copyright file="PullResponse.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Protocol.Remote.Pull
{
    using System.Collections.Generic;

    public class PullResponse
    {
        /// <summary>
        /// Access Controls
        /// </summary>
        public string[][] accessControls { get; set; }

        /// <summary>
        /// Named Collections
        /// </summary>
        public Dictionary<string, string[]> namedCollections { get; set; }

        /// <summary>
        /// Named Objects
        /// </summary>
        public Dictionary<string, string> namedObjects { get; set; }

        /// <summary>
        /// Named Values
        /// </summary>
        public Dictionary<string, object> namedValues { get; set; }

        /// <summary>
        /// Objects
        /// </summary>
        public string[][] Objects { get; set; }
    }
}
