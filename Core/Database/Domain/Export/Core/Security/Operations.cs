//-------------------------------------------------------------------------------------------------
// <copyright file="Operation.cs" company="Allors bvba">
// Copyright 2002-2017 Allors bvba.
//
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
//
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
//
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// For more information visit http://www.allors.com/legal
// </copyright>
// <summary>Defines the Operation type.</summary>
//-------------------------------------------------------------------------------------------------

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
