// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RelationNotLoadedEventArgs.cs" company="Allors bvba">
//   Copyright 2002-2017 Allors bvba.
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
// --------------------------------------------------------------------------------------------------------------------

namespace Allors
{
    using System;

    /// <summary>
    /// The loader raises an RoleNotLoaded event when either the object's 
    /// saved relation is not compatible with the current relation or the
    /// relation's saved type doesn't exist in the current domain.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="args">The <see cref="RelationNotLoadedEventHandler"/>.</param>
    public delegate void RelationNotLoadedEventHandler(object sender, RelationNotLoadedEventArgs args);

    /// <summary>
    /// The relation not loaded event arguments.
    /// </summary>
    public class RelationNotLoadedEventArgs
    {
        /// <summary>
        /// The relation type id.
        /// </summary>
        private readonly Guid relationTypeId;

        /// <summary>
        /// The association id.
        /// </summary>
        private readonly long associationId;

        /// <summary>
        /// The contents of the role.
        /// </summary>
        private readonly string roleContents;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelationNotLoadedEventArgs"/> class.
        /// </summary>
        /// <param name="relationTypeId">The relation type id.</param>
        /// <param name="associationId">The association id.</param>
        /// <param name="roleContents">The role contents.</param>
        public RelationNotLoadedEventArgs(Guid relationTypeId, long associationId, string roleContents)
        {
            this.relationTypeId = relationTypeId;
            this.associationId = associationId;
            this.roleContents = roleContents;
        }

        /// <summary>
        /// Gets the relation type id.
        /// </summary>
        /// <value>The relation type id.</value>
        public Guid RelationTypeId => this.relationTypeId;

        /// <summary>
        /// Gets the association id.
        /// </summary>
        /// <value>The association id.</value>
        public long AssociationId => this.associationId;

        /// <summary>
        /// Gets the role contents.
        /// </summary>
        /// <value>The role contents.</value>
        public string RoleContents => this.roleContents;

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString() => "RelationType: " + this.relationTypeId + ", Association: " + this.AssociationId + ", Role: " + this.roleContents;
    }
}
