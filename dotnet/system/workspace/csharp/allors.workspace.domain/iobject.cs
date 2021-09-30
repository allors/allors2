// <copyright file="Object.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    public interface IObject
    {
        /// <summary>
        /// The id of the object.
        /// The id is negative
        /// <ul>
        /// <li>a database object is new and has never been pushed</li>
        /// <li>a workspace or session object</li>
        /// </ul>
        /// The id is positive for database objects that have been pulled
        /// </summary>
        long Id { get; }

        IStrategy Strategy { get; }
    }
}
