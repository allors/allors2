// <copyright file="IObject.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IObject type.</summary>

namespace Allors.Database
{
    /// <summary>
    /// <para>
    /// A strategy based object delegates its framework related
    /// behavior to its own strategy object.
    /// </para>
    /// <para>
    /// Examples of framework related behavior are: persistence, relation management,
    /// life cycle management, transaction management, etc.
    /// </para>
    /// </summary>
    public partial interface IObject
    {
        /// <summary>
        /// Gets the Strategy.
        /// </summary>
        /// <value>The strategy.</value>
        IStrategy Strategy { get; }

        /// <summary>
        /// Gets the Object Id.
        /// </summary>
        /// <value>The object id.</value>
        long Id { get; }
    }
}
