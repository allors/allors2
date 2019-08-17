// <copyright file="First.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    [Id("1937b42e-954b-4ef9-bc63-5b8ae7903e9d")]
    #endregion
    public partial class First : Object, DerivationCounted
    {
        #region inherited properties

        public int DerivationCount { get; set; }

        #endregion

        #region Allors
        [Id("24886999-11f0-408f-b094-14b36ac4129b")]
        [AssociationId("e48ab2ee-c7a5-4d9a-b3ab-263f6aa4cdd1")]
        [RoleId("cf5c725d-e567-44de-ab5b-b47bb0bf8647")]
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        #endregion
        public Second Second { get; set; }

        #region Allors
        [Id("b0274351-3403-4384-afb6-2cb49cd03893")]
        [AssociationId("ec145229-e33a-4807-a0dd-48778cc88ac7")]
        [RoleId("12c46bf1-eed0-4e2a-b704-5d40032b4911")]
        #endregion
        public bool CreateCycle { get; set; }

        #region Allors
        [Id("f2b61dd5-d30c-445a-ae7a-af1c0cc8e278")]
        [AssociationId("ae9f23b5-20a7-4ecc-b642-503d75c486f1")]
        [RoleId("eb6b0565-1440-4b9b-aa23-51cfae3f93dd")]
        #endregion
        public bool IsDerived { get; set; }

        #region inherited methods

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

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
