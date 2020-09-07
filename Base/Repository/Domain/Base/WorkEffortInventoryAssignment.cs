// <copyright file="WorkEffortInventoryAssignment.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("f67e7755-5848-4601-ba70-4d1a39abfe4b")]
    #endregion
    public partial class WorkEffortInventoryAssignment : Versioned, Deletable, DelegatedAccessControlledObject
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public User CreatedBy { get; set; }

        public User LastModifiedBy { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastModifiedDate { get; set; }

        #endregion

        /// <summary>
        /// Gets or sets the WorkEffort under which this Assignment exists.
        /// </summary>
        #region Allors
        [Id("0bf425d4-7468-4e28-8fda-0b04278cb2cd")]
        [AssociationId("2c6841c6-c161-48e0-a257-d932d99ae7b4")]
        [RoleId("1afed0f6-15fa-4fd2-91f5-648773933e3b")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public WorkEffort Assignment { get; set; }

        /// <summary>
        /// Gets or sets the Part which describes this WorkEffortInventoryAssignment.
        /// </summary>
        #region Allors
        [Id("3704B202-A216-4943-A98A-EB0A78477EFD")]
        [AssociationId("77299BF7-A2AF-43CC-BC26-716D48F2E0B9")]
        [RoleId("D972B23E-FC81-4832-80E2-D94A34FA5D23")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public InventoryItem InventoryItem { get; set; }

        /// <summary>
        /// Gets or sets the Quantity of the Part for this WorkEffortInventoryAssignment.
        /// </summary>
        #region Allors
        [Id("70121570-c02d-4977-80e4-23e14cbc3fc9")]
        [AssociationId("b4224775-005c-4078-a5b6-2b8a60bc143a")]
        [RoleId("c82f1c25-9c42-4d38-8fae-f8790e2333ef")]
        #endregion
        [Required]
        [Workspace]
        public decimal Quantity { get; set; }

        #region Allors
        [Id("495645bf-0ef7-49be-9d4d-59125221ca06")]
        [AssociationId("efdb1b11-640c-4d4a-9ac2-6897f102baba")]
        [RoleId("9300e163-872d-4369-ac1e-ff3b1bebd2a7")]
        #endregion
        [Required]
        [Workspace]
        public decimal CostOfGoodsSold { get; set; }

        #region Allors
        [Id("E13BAD88-7B44-4B92-89D0-86D182404880")]
        [AssociationId("2A428B1E-E913-4E7D-ABBE-8D68A0BF8396")]
        [RoleId("729A391C-60D6-47A2-A66D-9E21B90285A8")]
        #endregion
        [Workspace]
        public decimal BillableQuantity { get; set; }

        /// <summary>
        /// Gets or sets the InventoryItemTransactions create by this WorkEffortInventoryAssignment (derived).
        /// </summary>
        #region Allors
        [Id("5fcdb553-4b8f-419b-9f12-b9cefa68d39f")]
        [AssociationId("dba27480-4d2f-4e69-af01-4e9afba2cc98")]
        [RoleId("3f7a72a4-2727-4dd6-a602-60ef9b6896af")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Derived]
        [Workspace]
        public InventoryItemTransaction[] InventoryItemTransactions { get; set; }

        #region Allors
        [Id("ED3CE72C-C980-43DC-95FC-E8111E87F018")]
        [AssociationId("C6C727BE-8E08-4067-AFEE-5239AE98E019")]
        [RoleId("BD121953-AC5D-4883-B8C1-BF3275B73BA8")]
        #endregion
        [Precision(19)]
        [Scale(4)]
        [Workspace]
        public decimal AssignedUnitSellingPrice { get; set; }

        #region Allors
        [Id("48C27B12-EAE9-48F6-B803-DC6C568D3816")]
        [AssociationId("A76C9D29-68E7-49DB-9839-97C7EA20392C")]
        [RoleId("24F92D49-99F0-493B-A1DE-34FFEA4E8819")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal UnitSellingPrice { get; set; }

        #region Versioning
        #region Allors
        [Id("07AAB5A6-19C0-4812-B957-B051C3998BCD")]
        [AssociationId("7F474028-0924-4C75-8C85-0A4CEC8D1D04")]
        [RoleId("F2DF6EC7-403A-4F3D-B69A-A01FE7444FC2")]
        #endregion
        [Indexed]
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public WorkEffortInventoryAssignmentVersion CurrentVersion { get; set; }

        #region Allors
        [Id("A5E9EAB4-D939-4F03-A771-712EAC2674A5")]
        [AssociationId("40ED6595-FE2A-439F-AA0F-F84085F09FC7")]
        [RoleId("D30F5386-04AF-4677-BE3C-8839CCBC8CED")]
        #endregion
        [Indexed]
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public WorkEffortInventoryAssignmentVersion[] AllVersions { get; set; }
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

        public void Delete() { }

        public void DelegateAccess() { }

        #endregion

        #region Allors

        [Id("0f837793-fa7c-47de-893a-b24bfdb869a9")]
        #endregion
        public void CalculatePurchasePrice()
        {
        }

        #region Allors

        [Id("8f817fcb-1655-4d54-9a68-844d8a7f5017")]
        #endregion
        public void CalculateSellingPrice()
        {
        }
    }
}
