// <copyright file="ObjectNotLoadedEventArgs.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the ObjectNotLoadedEventArgs type.</summary>

namespace Allors.Database
{
    using System;

    /// <summary>
    /// The object not loaded event arguments.
    /// </summary>
    public class ObjectNotLoadedEventArgs
    {
        /// <summary>
        /// Initializes a new state of the <see cref="ObjectNotLoadedEventArgs"/> class.
        /// </summary>
        /// <param name="objectTypeId">The object type id.</param>
        /// <param name="objectId">The object id.</param>
        public ObjectNotLoadedEventArgs(Guid objectTypeId, long objectId)
        {
            this.ObjectTypeId = objectTypeId;
            this.ObjectId = objectId;
        }

        /// <summary>
        /// Gets the object id.
        /// </summary>
        /// <value>The object id.</value>
        public long ObjectId { get; }

        /// <summary>
        /// Gets the object type id.
        /// </summary>
        /// <value>The object type id.</value>
        public Guid ObjectTypeId { get; }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString() => this.ObjectTypeId + ":" + this.ObjectId;
    }

    /// <summary>
    /// The loader raises an ObjectNotLoaded event when either the object's
    /// saved type is not compatible with the current type or the
    /// object's saved type doesn't exist in the current domain.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="args">The <see cref="ObjectNotLoadedEventArgs"/>.</param>
    public delegate void ObjectNotLoadedEventHandler(object sender, ObjectNotLoadedEventArgs args);
}
