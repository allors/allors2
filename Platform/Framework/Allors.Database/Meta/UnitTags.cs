//------------------------------------------------------------------------------------------------- 
// <copyright file="UnitTags.cs" company="Allors bvba">
// Copyright 2002-2014 Allors bvba.
// 
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// 
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
//-------------------------------------------------------------------------------------------------
namespace Allors.Meta
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