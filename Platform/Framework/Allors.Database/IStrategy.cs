// <copyright file="IStrategy.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IStrategy type.</summary>

namespace Allors
{
    using Allors.Meta;

    /// <summary>
    /// A strategy based object delegates all framework related work
    /// to its strategy object.
    /// </summary>
    public interface IStrategy
    {
        /// <summary>
        /// Gets the database session.
        /// </summary>
        /// <value>The database session.</value>
        ISession Session { get; }

        /// <summary>
        /// Gets the <see cref="Class"/>.
        /// </summary>
        /// <value>The object type.</value>
        IClass Class { get; }

        /// <summary>
        /// Gets the <see cref="Allors.ObjectId"/>.
        /// </summary>
        /// <value>The object id.</value>
        long ObjectId { get; }

        /// <summary>
        /// Gets the <see cref="Allors.ObjectId"/>.
        /// </summary>
        /// <value>The object id.</value>
        long ObjectVersion { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is deleted.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is deleted; otherwise, <c>false</c>.
        /// </value>
        bool IsDeleted { get; }

        /// <summary>
        /// Gets a value indicating whether this object is new in the current Session.
        /// </summary>
        /// <value>
        ///  <c>true</c> if this object is new in the current session otherwise, <c>false</c>.
        /// </value>
        bool IsNewInSession { get; }

        /// <summary>
        /// Gets the <see cref="IObject"/>.
        /// </summary>
        /// <returns>The allors object.</returns>
        IObject GetObject();

        /// <summary>
        /// Deletes this instance.
        /// </summary>
        void Delete();

        /// <summary>
        /// Gets a value indicating whether the composite role exists.
        /// </summary>
        /// <param name="relationType">The relation type.</param>
        /// <returns><c>true</c>if the composite role exists; otherwise,<c>false</c>. </returns>
        bool ExistRole(IRelationType relationType);

        /// <summary>
        /// Gets the role.
        /// </summary>
        /// <param name="relationType">Type of the relation.</param>
        /// <returns>The role object.</returns>
        object GetRole(IRelationType relationType);

        /// <summary>
        /// Sets the role.
        /// </summary>
        /// <param name="relationType">Type of the relation.</param>
        /// <param name="value">The value.</param>
        void SetRole(IRelationType relationType, object value);

        /// <summary>
        /// Removes the role.
        /// </summary>
        /// <param name="relationType">Type of the relation.</param>
        void RemoveRole(IRelationType relationType);

        /// <summary>
        /// Gets a value indicating whether the unit role exists.
        /// </summary>
        /// <param name="relationType">The relation type.</param>
        /// <returns><c>true</c>if the unit role exists; otherwise,<c>false</c>. </returns>
        bool ExistUnitRole(IRelationType relationType);

        /// <summary>
        /// Gets the unit role.
        /// </summary>
        /// <param name="relationType">Type of the relation.</param>
        /// <returns>The role object.</returns>
        object GetUnitRole(IRelationType relationType);

        /// <summary>
        /// Sets the unit role.
        /// </summary>
        /// <param name="relationType">Type of the relation.</param>
        /// <param name="unit">The unit .</param>
        void SetUnitRole(IRelationType relationType, object unit);

        /// <summary>
        /// Removes the unit role.
        /// </summary>
        /// <param name="relationType">Type of the relation.</param>
        void RemoveUnitRole(IRelationType relationType);

        /// <summary>
        /// Gets a value indicating whether the composite role exists.
        /// </summary>
        /// <param name="relationType">The relation type.</param>
        /// <returns><c>true</c>if the composite role exists; otherwise,<c>false</c>. </returns>
        bool ExistCompositeRole(IRelationType relationType);

        /// <summary>
        /// Gets the composite role.
        /// </summary>
        /// <param name="relationType">Type of the relation.</param>
        /// <returns>The role object.</returns>
        IObject GetCompositeRole(IRelationType relationType);

        /// <summary>
        /// Sets the composite role.
        /// </summary>
        /// <param name="relationType">Type of the relation.</param>
        /// <param name="role">The role.</param>
        void SetCompositeRole(IRelationType relationType, IObject role);

        /// <summary>
        /// Removes the composite role.
        /// </summary>
        /// <param name="relationType">Type of the relation.</param>
        void RemoveCompositeRole(IRelationType relationType);

        /// <summary>
        /// Gets a value indicating whether the composite roles exists.
        /// </summary>
        /// <param name="relationType">The relation type.</param>
        /// <returns><c>true</c>if the composite role exists; otherwise,<c>false</c>. </returns>
        bool ExistCompositeRoles(IRelationType relationType);

        /// <summary>
        /// Gets the composite roles.
        /// </summary>
        /// <param name="relationType">Type of the relation.</param>
        /// <returns>The role objects.</returns>
        Extent GetCompositeRoles(IRelationType relationType);

        /// <summary>
        /// Adds the composite role.
        /// </summary>
        /// <param name="relationType">The relation type..</param>
        /// <param name="role">The role.</param>
        void AddCompositeRole(IRelationType relationType, IObject role);

        /// <summary>
        /// Removes the composite role.
        /// </summary>
        /// <param name="relationType">Type of the relation.</param>
        /// <param name="role">The role.</param>
        void RemoveCompositeRole(IRelationType relationType, IObject role);

        /// <summary>
        /// Sets the composite roles.
        /// </summary>
        /// <param name="relationType">Type of the relation.</param>
        /// <param name="roles">The roles.</param>
        void SetCompositeRoles(IRelationType relationType, Extent roles);

        /// <summary>
        /// Removes the composite roles.
        /// </summary>
        /// <param name="relationType">Type of the relation.</param>
        void RemoveCompositeRoles(IRelationType relationType);

        /// <summary>
        /// Gets a value indicating whether the association exists.
        /// </summary>
        /// <param name="relationType">The relation type.</param>
        /// <returns><c>true</c>if the association exists; otherwise,<c>false</c>. </returns>
        bool ExistAssociation(IRelationType relationType);

        /// <summary>
        /// Gets the association.
        /// </summary>
        /// <param name="relationType">Type of the relation.</param>
        /// <returns>The association object.</returns>
        object GetAssociation(IRelationType relationType);

        /// <summary>
        /// Gets a value indicating whether the composite association exists.
        /// </summary>
        /// <param name="relationType">The relation type.</param>
        /// <returns><c>true</c>if the composite association exists; otherwise,<c>false</c>. </returns>
        bool ExistCompositeAssociation(IRelationType relationType);

        /// <summary>
        /// Gets the composite association.
        /// </summary>
        /// <param name="relationType">Type of the relation.</param>
        /// <returns>The association object.</returns>
        IObject GetCompositeAssociation(IRelationType relationType);

        /// <summary>
        /// Gets a value indicating whether the composite associations exists.
        /// </summary>
        /// <param name="relationType">The relation type.</param>
        /// <returns><c>true</c>if the composite associations exists; otherwise,<c>false</c>. </returns>
        bool ExistCompositeAssociations(IRelationType relationType);

        /// <summary>
        /// Gets the composite associations.
        /// </summary>
        /// <param name="relationType">Type of the relation.</param>
        /// <returns>The association objects.</returns>
        Extent GetCompositeAssociations(IRelationType relationType);
    }
}
