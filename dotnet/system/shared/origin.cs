// <copyright file="Origin.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors
{
    using System;

    [Flags]
    public enum Origin
    {
        /// <summary>
        /// Database origin.
        /// </summary>
        Database = 1,

        /// <summary>
        /// Local origin.
        /// </summary>
        Workspace = 2,

        /// <summary>
        /// Session origin.
        /// </summary>
        Session = 4,
    }
}
