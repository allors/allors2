// <copyright file="RelationNotLoadedEventArgs.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database
{
    using System;

    /// <summary>
    /// The relation not loaded event arguments.
    /// </summary>
    public class RelationNotLoadedEventArgs
    {
        /// <summary>
        /// Initializes a new state of the <see cref="RelationNotLoadedEventArgs"/> class.
        /// </summary>
        /// <param name="relationTypeId">The relation type id.</param>
        /// <param name="associationId">The association id.</param>
        /// <param name="roleContents">The role contents.</param>
        public RelationNotLoadedEventArgs(Guid relationTypeId, long associationId, string roleContents)
        {
            this.RelationTypeId = relationTypeId;
            this.AssociationId = associationId;
            this.RoleContents = roleContents;
        }

        /// <summary>
        /// Gets the relation type id.
        /// </summary>
        /// <value>The relation type id.</value>
        public Guid RelationTypeId { get; }

        /// <summary>
        /// Gets the association id.
        /// </summary>
        /// <value>The association id.</value>
        public long AssociationId { get; }

        /// <summary>
        /// Gets the role contents.
        /// </summary>
        /// <value>The role contents.</value>
        public string RoleContents { get; }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString() => "RelationType: " + this.RelationTypeId + ", Association: " + this.AssociationId + ", Role: " + this.RoleContents;
    }

    /// <summary>
    /// The loader raises an RoleNotLoaded event when either the object's
    /// saved relation is not compatible with the current relation or the
    /// relation's saved type doesn't exist in the current domain.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="args">The <see cref="RelationNotLoadedEventHandler"/>.</param>
    public delegate void RelationNotLoadedEventHandler(object sender, RelationNotLoadedEventArgs args);
}
