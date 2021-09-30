// <copyright file="ExtentOperationType.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Memory
{
    public enum ExtentOperationType
    {
        /// <summary>
        /// The union operation.
        /// </summary>
        Union,

        /// <summary>
        /// The intersect operation.
        /// </summary>
        Intersect,

        /// <summary>
        /// The except operation.
        /// </summary>
        Except,
    }
}
