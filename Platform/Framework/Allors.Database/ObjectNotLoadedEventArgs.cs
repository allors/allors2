//------------------------------------------------------------------------------------------------- 
// <copyright file="ObjectNotLoadedEventArgs.cs" company="Allors bvba">
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
// <summary>Defines the ObjectNotLoadedEventArgs type.</summary>
//------------------------------------------------------------------------------------------------- 

namespace Allors
{
    using System;

    /// <summary>
    /// The loader raises an ObjectNotLoaded event when either the object's 
    /// saved type is not compatible with the current type or the
    /// object's saved type doesn't exist in the current domain.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="args">The <see cref="ObjectNotLoadedEventArgs"/>.</param>
    public delegate void ObjectNotLoadedEventHandler(object sender, ObjectNotLoadedEventArgs args);

    /// <summary>
    /// The object not loaded event arguments.
    /// </summary>
    public class ObjectNotLoadedEventArgs
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectNotLoadedEventArgs"/> class.
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
}
