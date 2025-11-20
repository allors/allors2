// <copyright file="PushRequestRole.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Protocol.Remote.Push
{
    public class PushRequestRole
    {
        /// <summary>
        /// Role Type
        /// </summary>
        public string t { get; set; }

        /// <summary>
        /// Set Role
        /// </summary>
        public string s { get; set; }

        /// <summary>
        /// Add Roles
        /// </summary>
        public string[] a { get; set; }

        /// <summary>
        /// Remove Roles
        /// </summary>
        public string[] r { get; set; }
    }
}
