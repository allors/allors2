// <copyright file="ItemIssuance.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("441f6007-022d-4d77-bc2d-04c7a876e1bd")]
    #endregion
    public partial class ItemIssuance : Deletable, Object
    {
        #region inherited properties
        public Restriction[] Restrictions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("60089b34-e9aa-4b09-9a5c-4523ce60152f")]
        [AssociationId("ddf8eba9-8821-490f-9d9d-adc6ebd32ddb")]
        [RoleId("ee8e4f06-63e8-4281-a010-9f9212244cf1")]
        #endregion
        public DateTime IssuanceDateTime { get; set; }

        #region Allors
        [Id("83de0bfa-98ca-4299-a529-f8ba8a02cb90")]
        [AssociationId("467ce53a-969b-4537-b51c-998ac64afbe9")]
        [RoleId("1766b9c8-436d-427c-8c54-4f10a6accf02")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        public ShipmentItem ShipmentItem { get; set; }

        #region Allors
        [Id("6d0e1669-1583-4004-a0dd-6481faaa4803")]
        [AssociationId("2deb9c3e-6e3e-462c-88bf-df682a4af6e0")]
        [RoleId("d8e7874c-a162-440a-8e99-4dd7b07216cd")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        public InventoryItem InventoryItem { get; set; }

        #region Allors
        [Id("af4fbe17-bbdc-4f05-bf2e-398ee18598a5")]
        [AssociationId("6744410c-6f9c-49db-b73c-ed723592fee6")]
        [RoleId("938bb734-f18c-4756-9c68-54cad2377639")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        public PickListItem PickListItem { get; set; }

        #region Allors
        [Id("72872b29-69e3-4408-ad61-80201c46421b")]
        [AssociationId("f191b03b-fb03-4c5b-9455-57d241160e3b")]
        [RoleId("69dca6e4-7d13-481c-8a77-ff4c365df923")]
        #endregion
        [Required]
        [Precision(19)]
        [Scale(2)]
        public decimal Quantity { get; set; }

        #region Allors
        [Id("8DCB7F3A-C47C-4276-9107-2DB60609EE4A")]
        [AssociationId("B093350B-1D10-4917-8B3E-DE48DF8BE0EF")]
        [RoleId("7B24A23C-6A5C-43F1-9AA6-5A405BABB80F")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Derived]
        [Workspace]
        public InventoryItemTransaction[] InventoryItemTransactions { get; set; }

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnInit()
        {
        }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        public void Delete() { }

        #endregion
    }
}
