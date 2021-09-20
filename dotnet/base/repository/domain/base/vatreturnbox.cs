// <copyright file="VatReturnBox.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    [Id("8dc67774-c15a-47dd-9b8a-ce4e7139e8a3")]
    #endregion
    public partial class VatReturnBox : Object
    {
        #region inherited properties
        public Restriction[] Restrictions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("3bcc4fc9-5646-4ceb-b48b-bb1d7fbcba64")]
        [AssociationId("69f949c3-f5c1-4cb4-a907-ce3673496628")]
        [RoleId("ec126f8a-4d48-4c1e-bdb6-ad66ab529580")]
        #endregion
        [Size(256)]

        public string HeaderNumber { get; set; }

        #region Allors
        [Id("78e114b4-ec1d-49ce-ab32-40a3184dea31")]
        [AssociationId("98920876-b4f8-4d41-90f1-115164441836")]
        [RoleId("9a8717dd-0713-458b-8d84-9758f4ddfb03")]
        #endregion
        [Size(-1)]

        public string Description { get; set; }

        #region Allors
        [Id("beb55438-918d-4876-8d0b-989b7d9fabfa")]
        [AssociationId("419c0cb0-f596-40bb-84e7-1ebb5c2361de")]
        [RoleId("1a61ebd7-e255-42c9-932a-05031df0f2ab")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public VatReturnBoxType VatReturnBoxType { get; set; }

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
