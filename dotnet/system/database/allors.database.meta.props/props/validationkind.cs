// <copyright file="ValidationKind.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the ValidationReport type.</summary>

namespace Allors.Database.Meta
{
    /// <summary>
    /// The kind of validation that was performed.
    /// </summary>
    public enum ValidationKind
    {
        /// <summary>
        /// Should be present.
        /// </summary>
        Required,

        /// <summary>
        /// Should be in a legal format.
        /// </summary>
        Format,

        /// <summary>
        /// Should be unique.
        /// </summary>
        Unique,

        /// <summary>
        /// Should be mutual exclusiveness.
        /// </summary>
        Exclusive,

        /// <summary>
        /// Should be a legal hierarchy.
        /// </summary>
        Hierarchy,

        /// <summary>
        /// Should be a legal multiplicity.
        /// </summary>
        Multiplicity,

        /// <summary>
        /// Should have a minimum length.
        /// </summary>
        MinimumLength,

        /// <summary>
        /// Should not have a cycle.
        /// </summary>
        Cyclic,
    }
}
