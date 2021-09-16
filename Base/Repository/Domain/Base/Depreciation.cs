// <copyright file="Depreciation.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("7107db4e-8406-4fe3-8136-271077c287f8")]
    #endregion
    public partial class Depreciation : InternalAccountingTransaction
    {
        #region inherited properties
        public InternalOrganisation InternalOrganisation { get; set; }

        public AccountingTransactionDetail[] AccountingTransactionDetails { get; set; }

        public string Description { get; set; }

        public DateTime TransactionDate { get; set; }

        public decimal DerivedTotalAmount { get; set; }

        public AccountingTransactionNumber AccountingTransactionNumber { get; set; }

        public DateTime EntryDate { get; set; }

        public Restriction[] Restrictions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("83ae8e4e-c4cd-4f27-b5fd-b468e4603295")]
        [AssociationId("031bc098-9f75-4ced-bcca-0f35519887b2")]
        [RoleId("9e2be493-0100-474c-a49b-00b69c8d8ce1")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public FixedAsset FixedAsset { get; set; }

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnInit()
        {
        }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        #endregion

    }
}
