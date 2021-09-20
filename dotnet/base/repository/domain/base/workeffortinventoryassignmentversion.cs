// <copyright file="WorkEffortInventoryAssignmentVersion.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;
    using Allors.Repository.Attributes;

    #region Allors
    [Id("D4BF9E21-4EB5-4ED0-A9BF-3FE01E585AC7")]
    #endregion
    public partial class WorkEffortInventoryAssignmentVersion : Version
    {
        #region inherited properties
        public Restriction[] Restrictions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public Guid DerivationId { get; set; }

        public DateTime DerivationTimeStamp { get; set; }

        public User LastModifiedBy { get; set; }

        #endregion

        /// <summary>
        /// Gets or sets the WorkEffort under which this Assignment exists.
        /// </summary>
        #region Allors
        [Id("1B695715-456E-4BE6-8B42-0386928BBE07")]
        [AssociationId("D9424EA4-0486-47F8-8892-C85800125CD6")]
        [RoleId("A1F96E5B-C959-4D82-82D4-9F0F7954FA46")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public WorkEffort Assignment { get; set; }

        /// <summary>
        /// Gets or sets the Part which describes this WorkEffortInventoryAssignment.
        /// </summary>
        #region Allors
        [Id("F1811C2A-9A4E-4949-9008-E3519EA4AB51")]
        [AssociationId("DD39BE11-C72D-41C5-B0B2-D039C06B44FF")]
        [RoleId("11FADBA0-B32B-437B-8FDE-30F56B473882")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public InventoryItem InventoryItem { get; set; }

        /// <summary>
        /// Gets or sets the Quantity of the Part for this WorkEffortInventoryAssignment.
        /// </summary>
        #region Allors
        [Id("F5E0881D-E239-4B21-8E7A-C380E96E2A26")]
        [AssociationId("AD4120B4-5276-41F6-9F97-590234D31003")]
        [RoleId("AE7FFA88-4F5C-475B-B31B-898BCC0EF459")]
        #endregion
        [Required]
        [Workspace]
        public decimal Quantity { get; set; }

        #region Allors
        [Id("A1A024D4-B20F-44E1-84F0-1EBFEB962DE2")]
        [AssociationId("35A06638-9D8B-4F68-BCDE-6344AD23E61C")]
        [RoleId("7CE35E26-3393-45E7-B44C-F84EA8E80DA7")]
        #endregion
        [Workspace]
        public decimal AssignedBillableQuantity { get; set; }

        #region Allors
        [Id("5e6511de-2845-4306-8c68-57e1cabbe092")]
        [AssociationId("e9e4df72-7c9a-488e-973f-5d4223519070")]
        [RoleId("bc3483da-a41c-4e67-bf05-ff355bd4bf8a")]
        #endregion
        [Workspace]
        public decimal DerivedBillableQuantity { get; set; }

        /// <summary>
        /// Gets or sets the InventoryItemTransactions create by this WorkEffortInventoryAssignment (derived).
        /// </summary>
        #region Allors
        [Id("30EF280B-A7EA-400D-BA36-2DCD242C96F2")]
        [AssociationId("35DD3B2D-0B15-400C-AD4C-F0B8860D9754")]
        [RoleId("70CFE0A0-BF10-4EA6-8AE0-41CCE4ED447F")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
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

        #endregion
    }
}
