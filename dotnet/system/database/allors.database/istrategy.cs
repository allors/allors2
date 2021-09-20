// <copyright file="IStrategy.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IStrategy type.</summary>

namespace Allors.Database
{
    using System.Collections.Generic;
    using Meta;

    /// <summary>
    /// A strategy based object delegates all framework related work
    /// to its strategy object.
    /// </summary>
    public interface IStrategy
    {
        /// <summary>
        /// Gets the database transaction.
        /// </summary>
        /// <value>The database transaction.</value>
        ITransaction Transaction { get; }

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
        /// Gets a value indicating whether this state is deleted.
        /// </summary>
        /// <value>
        /// <c>true</c> if this state is deleted; otherwise, <c>false</c>.
        /// </value>
        bool IsDeleted { get; }

        /// <summary>
        /// Gets a value indicating whether this object is new in the current Transaction.
        /// </summary>
        /// <value>
        ///  <c>true</c> if this object is new in the current transaction otherwise, <c>false</c>.
        /// </value>
        bool IsNewInTransaction { get; }

        /// <summary>
        /// Gets the <see cref="IObject"/>.
        /// </summary>
        /// <returns>The allors object.</returns>
        IObject GetObject();

        /// <summary>
        /// Deletes this state.
        /// </summary>
        void Delete();

        /// <summary>
        /// Gets a value indicating whether the composite role exists.
        /// </summary>
        /// <param name="roleType">The relation type.</param>
        /// <returns><c>true</c>if the composite role exists; otherwise,<c>false</c>. </returns>
        bool ExistRole(IRoleType roleType);

        /// <summary>
        /// Gets the role.
        /// </summary>
        /// <param name="roleType">Type of the relation.</param>
        /// <returns>The role object.</returns>
        object GetRole(IRoleType roleType);

        /// <summary>
        /// Sets the role.
        /// </summary>
        /// <param name="roleType">Type of the relation.</param>
        /// <param name="value">The value.</param>
        void SetRole(IRoleType roleType, object value);

        /// <summary>
        /// Removes the role.
        /// </summary>
        /// <param name="roleType">Type of the relation.</param>
        void RemoveRole(IRoleType roleType);

        /// <summary>
        /// Gets a value indicating whether the unit role exists.
        /// </summary>
        /// <param name="roleType">The relation type.</param>
        /// <returns><c>true</c>if the unit role exists; otherwise,<c>false</c>. </returns>
        bool ExistUnitRole(IRoleType roleType);

        /// <summary>
        /// Gets the unit role.
        /// </summary>
        /// <param name="roleType">Type of the relation.</param>
        /// <returns>The role object.</returns>
        object GetUnitRole(IRoleType roleType);

        /// <summary>
        /// Sets the unit role.
        /// </summary>
        /// <param name="roleType">Type of the relation.</param>
        /// <param name="unit">The unit .</param>
        void SetUnitRole(IRoleType roleType, object unit);

        /// <summary>
        /// Removes the unit role.
        /// </summary>
        /// <param name="roleType">Type of the relation.</param>
        void RemoveUnitRole(IRoleType roleType);

        /// <summary>
        /// Gets a value indicating whether the composite role exists.
        /// </summary>
        /// <param name="roleType">The relation type.</param>
        /// <returns><c>true</c>if the composite role exists; otherwise,<c>false</c>. </returns>
        bool ExistCompositeRole(IRoleType roleType);

        /// <summary>
        /// Gets the composite role.
        /// </summary>
        /// <param name="roleType">Type of the relation.</param>
        /// <returns>The role object.</returns>
        IObject GetCompositeRole(IRoleType roleType);

        /// <summary>
        /// Sets the composite role.
        /// </summary>
        /// <param name="roleType">Type of the relation.</param>
        /// <param name="newRole">The role.</param>
        void SetCompositeRole(IRoleType roleType, IObject newRole);

        /// <summary>
        /// Removes the composite role.
        /// </summary>
        /// <param name="roleType">Type of the relation.</param>
        void RemoveCompositeRole(IRoleType roleType);

        /// <summary>
        /// Gets a value indicating whether the composite roles exists.
        /// </summary>
        /// <param name="roleType">The relation type.</param>
        /// <returns><c>true</c>if the composite role exists; otherwise,<c>false</c>. </returns>
        bool ExistCompositesRole(IRoleType roleType);

        /// <summary>
        /// Gets the composite roles.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="roleType">Type of the relation.</param>
        /// <returns>The role objects.</returns>
        IEnumerable<T> GetCompositesRole<T>(IRoleType roleType) where T : IObject;

        /// <summary>
        /// Adds the composite role.
        /// </summary>
        /// <param name="roleType">The relation type..</param>
        /// <param name="role">The role.</param>
        void AddCompositesRole(IRoleType roleType, IObject role);

        /// <summary>
        /// Removes the composite role.
        /// </summary>
        /// <param name="roleType">Type of the relation.</param>
        /// <param name="role">The role.</param>
        void RemoveCompositesRole(IRoleType roleType, IObject role);

        /// <summary>
        /// Sets the composite roles.
        /// </summary>
        /// <param name="roleType">Type of the relation.</param>
        /// <param name="roles">The roles.</param>
        void SetCompositesRole(IRoleType roleType, IEnumerable<IObject> roles);

        /// <summary>
        /// Removes the composite roles.
        /// </summary>
        /// <param name="roleType">Type of the relation.</param>
        void RemoveCompositesRole(IRoleType roleType);

        /// <summary>
        /// Gets a value indicating whether the association exists.
        /// </summary>
        /// <param name="associationType">The relation type.</param>
        /// <returns><c>true</c>if the association exists; otherwise,<c>false</c>. </returns>
        bool ExistAssociation(IAssociationType associationType);

        /// <summary>
        /// Gets the association.
        /// </summary>
        /// <param name="associationType">Type of the relation.</param>
        /// <returns>The association object.</returns>
        object GetAssociation(IAssociationType associationType);

        /// <summary>
        /// Gets a value indicating whether the composite association exists.
        /// </summary>
        /// <param name="associationType">The relation type.</param>
        /// <returns><c>true</c>if the composite association exists; otherwise,<c>false</c>. </returns>
        bool ExistCompositeAssociation(IAssociationType associationType);

        /// <summary>
        /// Gets the composite association.
        /// </summary>
        /// <param name="associationType">Type of the relation.</param>
        /// <returns>The association object.</returns>
        IObject GetCompositeAssociation(IAssociationType associationType);

        /// <summary>
        /// Gets a value indicating whether the composite associations exists.
        /// </summary>
        /// <param name="associationType">The relation type.</param>
        /// <returns><c>true</c>if the composite associations exists; otherwise,<c>false</c>. </returns>
        bool ExistCompositesAssociation(IAssociationType associationType);

        /// <summary>
        /// Gets the composite associations.
        /// </summary>
        /// <param name="associationType">Type of the relation.</param>
        /// <returns>The association objects.</returns>
        IEnumerable<T> GetCompositesAssociation<T>(IAssociationType associationType) where T : IObject;
    }
}
