// <copyright file="OrderItemBilling.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    [Id("1f14fdb3-9e0f-4cea-b7c7-3ca2ab898f56")]
    #endregion
    public partial class OrderItemBilling : Deletable, Object
    {
        #region inherited properties

        public Restriction[] Restrictions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }
        #endregion

        #region Allors
        [Id("214988fc-b5a2-4944-9c83-93a645a96853")]
        [AssociationId("2007bddd-e78c-40a8-9015-5d3f027586c0")]
        [RoleId("624c3c0b-faac-4542-aeb2-466952cbf832")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public OrderItem OrderItem { get; set; }

        #region Allors
        [Id("23a0d52d-3ec7-4ddf-a300-c0ee46edf41a")]
        [AssociationId("03ac8386-6706-4e3f-9ad2-a64e67edf08f")]
        [RoleId("61e3ad81-395e-46e9-837f-e48257141164")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public InvoiceItem InvoiceItem { get; set; }

        #region Allors
        [Id("2f75bdee-46f9-4dd0-b349-00a497462fdb")]
        [AssociationId("86dc1660-7719-4dae-93d8-ce4ca7a00f2a")]
        [RoleId("bc0b7bb6-c77b-451d-bf95-c32967766c49")]
        #endregion
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal Amount { get; set; }

        #region Allors
        [Id("cfff23f0-1f3c-48a1-b4a7-85bc2254dbff")]
        [AssociationId("ed09cee4-3c01-4a2a-ab3d-6f9e8de16466")]
        [RoleId("6193e84f-882a-4b7a-b51c-8ec93f09aff2")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal Quantity { get; set; }

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnInit() { }

        public void OnPreDerive() { }

        public void Delete() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        #endregion
    }
}
