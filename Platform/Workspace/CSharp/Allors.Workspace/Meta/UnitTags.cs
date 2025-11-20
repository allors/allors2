// <copyright file="UnitTags.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Meta
{
    /// <summary>
    /// The tags for units.
    /// Do not use tags for long term persistence, UnitTypeIds should be used for that purpose.
    /// </summary>
    public enum UnitTags
    {
        /// <summary>
        /// The tag for the binary <see cref="IObjectType"/>.
        /// </summary>
        Binary,

        /// <summary>
        /// The tag for the boolean <see cref="IObjectType"/>.
        /// </summary>
        Boolean,

        /// <summary>
        /// The tag for the date time <see cref="IObjectType"/>.
        /// </summary>
        DateTime,

        /// <summary>
        /// The tag for the decimal <see cref="IObjectType"/>.
        /// </summary>
        Decimal,

        /// <summary>
        /// The tag for the float <see cref="IObjectType"/>.
        /// </summary>
        Float,

        /// <summary>
        /// The tag for the integer <see cref="IObjectType"/>.
        /// </summary>
        Integer,

        /// <summary>
        /// The tag for the string <see cref="IObjectType"/>.
        /// </summary>
        String,

        /// <summary>
        /// The tag for the unique <see cref="IObjectType"/>.
        /// </summary>
        Unique,
    }
}
