// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IValidation.cs" company="Allors bvba">
//   Copyright 2002-2017 Allors bvba.
//
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
//
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
//
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain
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
