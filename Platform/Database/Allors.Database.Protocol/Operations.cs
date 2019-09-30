// <copyright file="Operations.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Operation type.</summary>

namespace Allors.Domain
{
    using System;

    [Flags]
    public enum Operations
    {
        /// <summary>
        /// No operation.
        /// </summary>
        None = 0,

        /// <summary>
        /// Read a relation (get).
        /// </summary>
        Read = 1,

        /// <summary>
        /// Write a relation (set, add and remove).
        /// </summary>
        Write = 2,

        /// <summary>
        /// Execute a method.
        /// </summary>
        Execute = 4,
    }
}
