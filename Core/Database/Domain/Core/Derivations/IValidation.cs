// <copyright file="IValidation.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain.Derivations
{
    using Allors.Meta;

    public interface IValidation
    {
        IDerivation Derivation { get; }

        bool HasErrors { get; }

        IDerivationError[] Errors { get; }

        void AddError(IDerivationError derivationError);

        void AddError(IObject association, RoleType roleType, string errorMessage, params object[] messageParam);

        void AddError(IObject role, AssociationType associationType, string errorMessage, params object[] messageParam);

        void AssertExists(IObject association, RoleType roleType);

        void AssertNotExists(IObject association, RoleType roleType);

        void AssertNonEmptyString(IObject association, RoleType roleType);

        void AssertExistsNonEmptyString(IObject association, RoleType roleType);

        void AssertIsUnique(IObject association, RoleType roleType);

        void AssertAtLeastOne(IObject association, params RoleType[] roleTypes);

        void AssertExistsAtMostOne(IObject association, params RoleType[] roleTypes);

        void AssertAreEqual(IObject association, RoleType roleType, RoleType otherRoleType);

        void AssertExists(IObject role, AssociationType associationType);
    }
}
