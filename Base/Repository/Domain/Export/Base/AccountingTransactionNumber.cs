// <copyright file="AccountingTransactionNumber.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using Attributes;

    #region Allors
    [Id("0b9034b1-288a-48a7-9d46-3ca6dcb7ca3f")]
    #endregion
    public partial class AccountingTransactionNumber : Object
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("1a7eda6e-7b1c-4faf-8635-05bc233c5dd8")]
        [AssociationId("ad6df638-67d2-4d41-9695-01c6adf3f251")]
        [RoleId("e2178198-6bbd-4caa-9402-40137b2bd529")]
        #endregion
        public int Number { get; set; }

        #region Allors
        [Id("e01605b0-ba04-4775-ad15-cac1281cec9e")]
        [AssociationId("75523554-b713-433b-8916-c70278649b52")]
        [RoleId("dcaeed3f-1bbe-40a0-aacc-9c26db3f984f")]
        #endregion
        public int Year { get; set; }

        #region Allors
        [Id("f3bcec3b-b08e-4eab-812e-bb5b31fe6a4d")]
        [AssociationId("e447318b-ab6a-4259-8fb0-fb0ae81b15f7")]
        [RoleId("018e866e-3593-45d2-a10e-45bd79a0faeb")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        public AccountingTransactionType AccountingTransactionType { get; set; }

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
