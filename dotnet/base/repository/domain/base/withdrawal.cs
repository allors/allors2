// <copyright file="Withdrawal.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("edf1788a-0c75-4635-904d-db9f9a6a7399")]
    #endregion
    public partial class Withdrawal : FinancialAccountTransaction
    {
        #region inherited properties
        public string Description { get; set; }

        public DateTime EntryDate { get; set; }

        public DateTime TransactionDate { get; set; }

        public Restriction[] Restrictions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("b97344ac-a848-4ee0-bdb5-a9d79bb785fc")]
        [AssociationId("265511f9-0f02-47c8-b7c4-392f09a69fa2")]
        [RoleId("c7dcd911-b352-4f0f-98fd-ea1c3d8d77d6")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]

        public Disbursement Disbursement { get; set; }

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
