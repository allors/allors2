// <copyright file="Deposit.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("52458d42-94ee-4757-bcfb-bc9c45ed6dc6")]
    #endregion
    public partial class Deposit : FinancialAccountTransaction
    {
        #region inherited properties
        public string Description { get; set; }

        public DateTime EntryDate { get; set; }

        public DateTime TransactionDate { get; set; }

        public Restriction[] Restrictions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("2a41dcff-72f9-4225-8a92-1955f10b8ae2")]
        [AssociationId("3a24349b-c31d-4ba1-bb95-616852f07c93")]
        [RoleId("04ff9dbf-60cd-4062-b61a-c26b78cf1c48")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]

        public Receipt[] Receipts { get; set; }

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
