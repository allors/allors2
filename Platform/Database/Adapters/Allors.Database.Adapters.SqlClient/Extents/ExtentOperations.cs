// <copyright file="ExtentOperations.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.SqlClient
{
    internal enum ExtentOperations
    {
        /// <summary>
        /// The Union of two Extents.
        /// </summary>
        Union,

        /// <summary>
        /// The Intersection of two Extents.
        /// </summary>
        Intersect,

        /// <summary>
        /// The Difference of two Extents.
        /// </summary>
        Except,
    }
}
