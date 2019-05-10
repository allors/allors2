// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnBankAccount.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Allors.Domain
{
    using Allors.Meta;

    public partial class OwnBankAccount
    {
        public void AppsOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistIsActive)
            {
                this.IsActive = true;
            }
        }

        public void AppsOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            if (this.ExistBankAccount && derivation.ChangeSet.GetRoleTypes(this.Id).Contains(this.Meta.BankAccount))
            {
                derivation.Add(this.BankAccount);
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            if (this.ExistInternalOrganisationWhereActiveCollectionMethod && this.InternalOrganisationWhereActiveCollectionMethod.DoAccounting)
            {
                derivation.Validation.AssertAtLeastOne(this, M.Cash.GeneralLedgerAccount, M.Cash.Journal);
            }
            
            derivation.Validation.AssertExistsAtMostOne(this, M.Cash.GeneralLedgerAccount, M.Cash.Journal);
        }
    }
}