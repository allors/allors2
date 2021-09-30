// <copyright file="NonSerialisedInventoryItemVersion.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("21C27A88-F99A-4871-B9D3-00C78F648574")]
    #endregion
    public partial class NonSerialisedInventoryItemVersion : InventoryItemVersion
    {
        #region inherited properties

        public Restriction[] Restrictions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public Part Part { get; set; }

        public string Name { get; set; }

        public Lot Lot { get; set; }

        public UnitOfMeasure UnitOfMeasure { get; set; }

        public Facility Facility { get; set; }

        public Guid DerivationId { get; set; }

        public DateTime DerivationTimeStamp { get; set; }

        public User LastModifiedBy { get; set; }

        #endregion

        #region Allors
        [Id("01860D37-7452-440C-85C6-15C5178732B7")]
        [AssociationId("4ED29A37-C30D-45CC-945B-DF88488FDFB6")]
        [RoleId("5EE0DC1F-AD4D-4022-92B7-396274A40256")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal PreviousQuantityOnHand { get; set; }

        #region Allors
        [Id("B3800237-4D03-4228-955D-7E573CEE47FA")]
        [AssociationId("5B906A1D-B127-4064-A944-401AFC9F86D7")]
        [RoleId("29FD8D29-6C87-4141-BE92-10723C0C49AE")]
        [Indexed]
        #endregion
        [Workspace]
        [Multiplicity(Multiplicity.ManyToOne)]
        public NonSerialisedInventoryItemState NonSerialisedInventoryItemState { get; set; }

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
