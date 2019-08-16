//------------------------------------------------------------------------------------------------- 
// <copyright file="ValidationKind.cs" company="Allors bvba">
// Copyright 2002-2017 Allors bvba.
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
// <summary>Defines the ValidationReport type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Workspace.Meta
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
        Cyclic
    }
}
