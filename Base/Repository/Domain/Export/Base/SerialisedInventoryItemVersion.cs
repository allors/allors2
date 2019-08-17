// <copyright file="SerialisedInventoryItemVersion.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("F9111BDF-A0B6-40CB-B33A-0A856B357327")]
    #endregion
    public partial class SerialisedInventoryItemVersion : InventoryItemVersion
    {
        #region inherited properties

        public Permission[] DeniedPermissions { get; set; }

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
        [Id("7F30A827-CBFA-4716-B0BD-08641CB66B1B")]
        [AssociationId("854A82D0-4CB3-42A2-8C0D-7DFE999BEE7F")]
        [RoleId("3860102B-4D92-40B8-9CEB-634D5212AFCF")]
        [Indexed]
        #endregion
        [Workspace]
        [Multiplicity(Multiplicity.ManyToOne)]
        public SerialisedInventoryItemState SerialisedInventoryItemState { get; set; }

        #region Allors
        [Id("6DD0FA27-1140-4F51-A642-35D8C1126684")]
        [AssociationId("DD8AD770-CCFF-4102-9830-CD6297299936")]
        [RoleId("53709128-15A0-4242-B887-5CA76C409F4D")]
        #endregion
        [Required]
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public SerialisedItem SerialisedItem { get; set; }

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
