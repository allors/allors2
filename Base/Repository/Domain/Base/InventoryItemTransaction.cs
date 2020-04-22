// <copyright file="InventoryItemTransaction.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("b00e2650-283f-4326-bdd3-46a2890e2037")]
    #endregion
    public partial class InventoryItemTransaction : Commentable, Object
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public string Comment { get; set; }

        public LocalisedText[] LocalisedComments { get; set; }

        #endregion

        /// <summary>
        /// Gets or Sets the Part to which this InventoryItemTransaction applies.
        /// </summary>
        #region Allors
        [Id("F851D977-7D58-4105-AB4A-74CFD5298D2D")]
        [AssociationId("48AEE6D7-C974-4F7C-98B7-4D9B6B4961D8")]
        [RoleId("C87DFF96-9531-4408-A155-41C61708A323")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public Part Part { get; set; }

        #region Allors
        [Id("e422efc4-4d17-46d8-bba4-6e78e7761f93")]
        [AssociationId("468307f7-5033-4e77-9482-5df34ca9a4f1")]
        [RoleId("7e0b8650-0d19-4ecc-b6e6-3c78dfe8c2aa")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public InventoryTransactionReason Reason { get; set; }

        /// <summary>
        /// Gets or sets the Serial Number for this InventoryItemTransaction (required if Part.InventoryItemKind.IsSerialised).
        /// </summary>
        #region Allors
        [Id("AFC2C5F2-4E00-4FB8-836F-C2B6A5A292A0")]
        [AssociationId("416923C5-26C1-49BB-88E3-D6A67EFE9828")]
        [RoleId("8C43D427-697E-48AE-A60A-02699C93413B")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public SerialisedItem SerialisedItem { get; set; }

        #region Allors
        [Id("57bdf1d7-84b8-4c7c-a470-396f6facd3bd")]
        [AssociationId("6f8706cd-f005-4ab1-8deb-db5d00b72403")]
        [RoleId("1a45c449-2b0d-4f64-be40-0858018b9cf6")]
        #endregion
        [Required]
        [Workspace]
        public decimal Quantity { get; set; }

        #region Allors
        [Id("364853b7-13eb-4952-ac71-36541518b48a")]
        [AssociationId("310a9842-aab8-4b87-9db0-f0ea0fbfd4eb")]
        [RoleId("a496e4c4-fe45-45ad-b57f-cb554502dbef")]
        #endregion
        [Required]
        [Workspace]
        public decimal Cost { get; set; }

        /// <summary>
        /// The TransactionDate and Time when this InventoryItemTransaction occurred.
        /// </summary>
        #region Allors
        [Id("af9fa5bc-a392-473d-b077-7f06ee24390b")]
        [AssociationId("9a0f9ecd-9954-4c2f-bb0e-e94f9cc3c19a")]
        [RoleId("5665d533-cd9c-4328-b422-66a94d77b19b")]
        #endregion
        [Required]
        [Indexed]
        [Workspace]
        public DateTime TransactionDate { get; set; }

        #region Allors
        [Id("9ADBF0A8-5676-430A-8242-97660692A1F6")]
        [AssociationId("F8A66D91-2CED-4252-9B83-55519491BF79")]
        [RoleId("3A57D8C7-7D7E-44D9-9482-12C74393B0DC")]
        #endregion
        [Indexed]
        [Derived]
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public InventoryItem InventoryItem { get; set; }

        #region Allors
        [Id("58ead8d2-c9c3-4092-b5d1-79af4811f43c")]
        [AssociationId("82f2636f-738d-45b8-bdc0-5136ad8d8382")]
        [RoleId("eee9f92b-5649-467f-8a99-4318c24cc002")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        public ItemVarianceAccountingTransaction ItemVarianceAccountingTransaction { get; set; }

        /// <summary>
        /// Gets or Sets the Facility where this InventoryItemTransaction applies.
        /// If not provided, the DefaultFacility from the Part will be used for this InventoryItemTransaction.
        /// </summary>
        #region Allors
        [Id("D22BB11E-8E99-4B81-9B72-20858AF33A11")]
        [AssociationId("CE4CE90A-A050-48DD-95F9-408780D1C48F")]
        [RoleId("9521C434-E4C5-472D-AEFC-65CA0DD1690D")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Facility Facility { get; set; }

        /// <summary>
        /// Gets or Sets the Lot where this InventoryItemTransaction applies (if any).
        /// </summary>
        #region Allors
        [Id("7EC5EF43-3031-4519-9C0C-14828E123C7D")]
        [AssociationId("B43F6567-2074-4A3B-ADEB-948E33AD098D")]
        [RoleId("BCEBF5C1-1D76-4825-8D5A-DE735AB51DAC")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Lot Lot { get; set; }

        /// <summary>
        /// Gets or Sets the UnitOfMeasure for this InventoryItemTransaction.
        /// If not provided, the UnitOfMeasure from the Part will be used for this InventoryItemTransaction.
        /// </summary>
        #region Allors
        [Id("639C6EF1-1D76-42B4-A59B-184DAD622D6F")]
        [AssociationId("1C3580E9-2955-4059-9432-D9605EC23EC0")]
        [RoleId("6B6846F4-3DBD-4BAE-87D0-2F90D16EAC6E")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public UnitOfMeasure UnitOfMeasure { get; set; }

        /// <summary>
        /// Gets or Sets the NonSerialisedInventoryItemState for this InventoryItemTransaction.
        /// If not provided, the DefaultNonSerialisedInventoryItemState from the InventoryStrategy will be used for this InventoryItemTransaction.
        /// </summary>
        #region Allors
        [Id("CA486EA2-D3CA-47CD-B09B-87CDA73DFC42")]
        [AssociationId("D10C433C-7BC9-4534-9844-25ABF6438ED3")]
        [RoleId("DF1E5585-F9C0-4A44-92C2-5E75F1CABE2C")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public NonSerialisedInventoryItemState NonSerialisedInventoryItemState { get; set; }

        /// <summary>
        /// Gets or Sets the SerialisedInventoryItemState for this InventoryItemTransaction.
        /// /// If not provided, the DefaultSerialisedInventoryItemState from the InventoryStrategy will be used for this InventoryItemTransaction.
        /// </summary>
        #region Allors
        [Id("07166685-B405-41F7-9581-10CFB937ACF9")]
        [AssociationId("E85E7A73-A08B-4784-83AC-CFC848F881D9")]
        [RoleId("A22119E7-F630-4FC7-8E92-C3FA149E39BD")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public SerialisedInventoryItemState SerialisedInventoryItemState { get; set; }

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
