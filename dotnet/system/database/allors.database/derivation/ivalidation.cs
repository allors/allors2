// <copyright file="IDomainDerivation.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IDomainDerivation type.</summary>

namespace Allors.Database.Derivations
{
    using Meta;

    public interface IValidation
    {
        bool HasErrors { get; }

        IDerivationError[] Errors { get; }

        void AddError(IDerivationError derivationError);

        void AddError(IObject association, IRoleType roleType, string errorMessage, params object[] messageParam);

        void AddError(IObject role, IAssociationType associationType, string errorMessage, params object[] messageParam);

        void AddError(string error);

        void AssertExists(IObject association, IRoleType roleType);

        void AssertNotExists(IObject association, IRoleType roleType);

        void AssertNonEmptyString(IObject association, IRoleType roleType);

        void AssertExistsNonEmptyString(IObject association, IRoleType roleType);

        void AssertIsUnique(IChangeSet changeSet, IObject association, IRoleType roleType);

        void AssertAtLeastOne(IObject association, params IRoleType[] roleTypes);

        void AssertExistsAtMostOne(IObject association, params IRoleType[] roleTypes);

        void AssertAreEqual(IObject association, IRoleType roleType, IRoleType otherRoleType);

        void AssertExists(IObject role, IAssociationType associationType);
    }
}
