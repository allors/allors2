// <copyright file="Invocation.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Protocol.Remote.Invoke
{
    public class Invocation
    {
        /// <summary>
        /// Id
        /// </summary>
        public string i { get; set; }

        /// <summary>
        /// Version
        /// </summary>
        public string v { get; set; }

        /// <summary>
        /// Method
        /// </summary>
        public string m { get; set; }
    }
}
