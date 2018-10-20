namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("75916246-b1b5-48ef-9578-d65980fd2623")]
    #endregion
    public partial class Part : AccessControlledObject, UniquelyIdentifiable
    {
        #region inheritedProperties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public Guid UniqueId { get; set; }
        #endregion inheritedProperties

        #region Allors
        [Id("17D9A211-83AC-4F77-B0D6-2673C50EE4C2")]
        [AssociationId("8B4C220A-A33B-468C-B345-2A40126118C1")]
        [RoleId("28DB278D-0A00-492C-8329-6FE23B541386")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Required]
        [Workspace]
        public InternalOrganisation InternalOrganisation { get; set; }

        /// <summary>
        /// Gets or sets the Default Facility where this Part is stored
        /// </summary>
        #region Allors
        [Id("23EC834E-849F-4CEF-9E22-BE73CCEC18FF")]
        [AssociationId("9EDEFEEE-AC9E-4858-9A0D-E1674316A3B5")]
        [RoleId("C4C36290-7AFF-4582-B075-4915340783B6")]
        #endregion
        [Indexed]
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        [Required]
        public Facility DefaultFacility { get; set; }

        #region Allors
        [Id("5239147e-0829-4250-bdbc-8115e9c19206")]
        [AssociationId("6f267a60-802b-454f-9ac7-762a92746255")]
        [RoleId("a9efc713-6574-4b82-b20e-0fc22747566a")]
        #endregion
        [Size(256)]
        public string Name { get; set; }

        #region Allors
        [Id("527c0d02-7723-4715-b975-ec9474d0d22d")]
        [AssociationId("b8cce82f-8555-4d15-8012-3b122ad47b3d")]
        [RoleId("72e60215-a8fb-40a1-ac9b-0204421adde0")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        public PartSpecification[] PartSpecifications { get; set; }

        /// <summary>
        /// Gets or sets the UnitOfMeasure in which this Part is tracked
        /// </summary>
        #region Allors
        [Id("610f6c8c-0d1d-4c8e-9d3d-a98e17d181b5")]
        [AssociationId("00a2efd5-0a43-4b86-8ce3-2196c2ad7c3d")]
        [RoleId("f843b974-81bf-48a1-9397-8708da48e39c")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        public UnitOfMeasure UnitOfMeasure { get; set; }

        #region Allors
        [Id("773e731d-47f7-4742-b8c6-81dec0a09f29")]
        [AssociationId("183113ef-8420-444d-8a80-61580a9f95dc")]
        [RoleId("05f1428a-26cd-4f08-9f1d-dec02edf6fe1")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        public Document[] Documents { get; set; }

        #region Allors
        [Id("5f727bd9-9c3e-421e-93eb-646c4fdf73d3")]
        [AssociationId("210976bb-e440-44ee-b2b5-39bcee04965b")]
        [RoleId("3165a365-a0db-4ce6-b194-7636cc9c015a")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Party ManufacturedBy { get; set; }

        #region Allors
        [Id("89a29d59-b56f-4846-a9d2-cf7d094826dc")]
        [AssociationId("8527c099-3ea0-486c-b288-ebf7e642952e")]
        [RoleId("84e90f5a-ce0f-4f88-b964-829154e682dd")]
        #endregion
        [Size(256)]
        public string ManufacturerId { get; set; }

        #region Allors
        [Id("8dc701e0-1f66-44ee-acc6-9726aa7d5853")]
        [AssociationId("2b9103c7-7ff8-4733-aa02-53800bb6e9bc")]
        [RoleId("6d60fb2f-1893-48ac-9e7d-9aa2a9a89431")]
        #endregion
        public int ReorderLevel { get; set; }

        #region Allors
        [Id("a093c852-cba8-43ff-9572-fd8c6cd53638")]
        [AssociationId("8c3d3a61-4d3a-477c-9701-a292435112e3")]
        [RoleId("f2ffce75-82d5-460f-83cc-621d63211d18")]
        #endregion
        public int ReorderQuantity { get; set; }

        #region Allors
        [Id("a202a540-dc0d-4032-9963-d0aa1511c990")]
        [AssociationId("0dd915a3-d517-46c5-8664-e59c56623564")]
        [RoleId("ab316ee2-bf84-4501-a798-94832c55e73f")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        public PriceComponent[] PriceComponents { get; set; }

        #region Allors
        [Id("f2c3407e-ab62-4f3e-94e5-7e9e65b89d6e")]
        [AssociationId("9bf78bcd-319c-4767-8053-4307577559ff")]
        [RoleId("319781e8-c83c-41ea-a8e7-b7224e8240e0")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public InventoryItemKind InventoryItemKind { get; set; }

        #region Allors
        [Id("B316EB62-A654-4429-9699-403B23DB5284")]
        [AssociationId("F3A6EA79-9E12-405A-8195-90FC3973BD65")]
        [RoleId("BA8E7FFA-8557-4452-B97B-1A5E2BFA83D0")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public ProductType ProductType { get; set; }

        #region Allors
        [Id("CA9F9403-B31F-4A44-9019-86272E21C1D8")]
        [AssociationId("66BD70AA-EF92-42E6-B0DF-30319CF88D6E")]
        [RoleId("BF290FC2-3677-4072-AEDF-6943958E1AA7")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        public SerialisedItem[] SerialisedItems { get; set; }

        #region Allors
        [Id("43b4b5c7-f94d-4e9b-9ea9-32a4414b11a6")]
        [AssociationId("bb51ade7-37c0-4c7c-b32c-59cb06fa2c9c")]
        [RoleId("ffdc1a53-6fd9-4a21-a576-77c84fc893eb")]
        #endregion
        [Unique]
        [Required]
        [Size(256)]
        public string PartId { get; set; }

        #region Allors
        [Id("30C81CF6-6295-44C4-ACDD-2A408DA3DC6D")]
        [AssociationId("9D3328E6-EE12-4A59-B664-967EB5DC6612")]
        [RoleId("E6010C20-764F-4FD6-BB0B-A5B57B59C840")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal QuantityOnHand { get; set; }

        #region Allors
        [Id("04cd1e20-a031-4a4f-9f40-6debb52b002c")]
        [AssociationId("4441b31a-7807-41c6-803b-aeacd18e2867")]
        [RoleId("8dc2ddca-4ae2-48b9-92db-ac68f2f5542e")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal AvailableToPromise { get; set; }

        #region Allors
        [Id("75CC0426-6695-4930-BB16-4B8B8618D7C8")]
        [AssociationId("14629D25-2A27-45BC-BF8C-A5D91997AF7C")]
        [RoleId("03733A79-F65B-4481-9B08-5C5DDEA7CB17")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal QuantityCommittedOut { get; set; }

        #region Allors
        [Id("2ED8E0B8-3ABA-4CDE-93C7-E45AFB381E66")]
        [AssociationId("D625F49C-3156-4BE7-97B0-197C9CD813E9")]
        [RoleId("F27E47C0-5D1E-4DF3-BCCF-DD5809324938")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal QuantityExpectedIn { get; set; }

        /// <summary>
        /// Gets or Sets the InventoryStrategy used by this Part
        /// </summary>
        #region Allors
        [Id("73BE7204-4AC4-4357-90B0-64E16D374B42")]
        [AssociationId("86B0C071-41B6-4A8F-914F-9E675BA98285")]
        [RoleId("DCABBDBE-B7A9-4F74-B244-4D8447FDF069")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public InventoryStrategy InventoryStrategy { get; set; }

        #region inheritedMethods
        public void OnBuild() { }

        public void OnDerive() { }

        public void OnPostBuild() { }

        public void OnPostDerive() { }

        public void OnPreDerive() { }
        #endregion inheritedMethods
    }
}