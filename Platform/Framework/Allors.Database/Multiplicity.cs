//-------------------------------------------------------------------------------------------------
// <copyright file="Multiplicity.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
//-------------------------------------------------------------------------------------------------

namespace Allors
{
    public enum Multiplicity
    {
        /// <summary>
        /// One to one.
        /// </summary>
        OneToOne = 0,

        /// <summary>
        /// One to many.
        /// </summary>
        OneToMany = 1,

        /// <summary>
        /// Many to one.
        /// </summary>
        ManyToOne = 2,

        /// <summary>
        /// Many to Many.
        /// </summary>
        ManyToMany = 3
    }
}
