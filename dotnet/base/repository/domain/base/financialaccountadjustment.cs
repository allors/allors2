// <copyright file="FinancialAccountAdjustment.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("12ba6843-bae1-41d1-9ef2-c19d74b0a365")]
    #endregion
    public partial class FinancialAccountAdjustment : FinancialAccountTransaction
    {
        #region inherited properties
        public string Description { get; set; }

        public DateTime EntryDate { get; set; }

        public DateTime TransactionDate { get; set; }

        public Restriction[] Restrictions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

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
