// <copyright file="InvestmentAccount.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("8a06c50b-5951-465e-86b8-43e733f20b90")]
    #endregion
    public partial class InvestmentAccount : FinancialAccount
    {
        #region inherited properties
        public FinancialAccountTransaction[] FinancialAccountTransactions { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("9eefdec1-48db-4f91-9eac-928b6a42d4e4")]
        [AssociationId("2759ed05-afa4-49ea-91d1-20b8d2ff527c")]
        [RoleId("1d337bb7-2b33-4c8a-aeb3-d37c3ea72690")]
        #endregion
        [Required]
        [Size(256)]

        public string Name { get; set; }

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
