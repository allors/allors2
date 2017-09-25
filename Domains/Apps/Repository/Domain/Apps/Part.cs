namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("75916246-b1b5-48ef-9578-d65980fd2623")]
    #endregion
	public partial interface Part : AccessControlledObject, UniquelyIdentifiable 
    {

        #region Allors
        [Id("5239147e-0829-4250-bdbc-8115e9c19206")]
        [AssociationId("6f267a60-802b-454f-9ac7-762a92746255")]
        [RoleId("a9efc713-6574-4b82-b20e-0fc22747566a")]
        #endregion
        [Required]
        [Size(256)]

        string Name { get; set; }


        #region Allors
        [Id("527c0d02-7723-4715-b975-ec9474d0d22d")]
        [AssociationId("b8cce82f-8555-4d15-8012-3b122ad47b3d")]
        [RoleId("72e60215-a8fb-40a1-ac9b-0204421adde0")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]

        PartSpecification[] PartSpecifications { get; set; }


        #region Allors
        [Id("610f6c8c-0d1d-4c8e-9d3d-a98e17d181b5")]
        [AssociationId("00a2efd5-0a43-4b86-8ce3-2196c2ad7c3d")]
        [RoleId("f843b974-81bf-48a1-9397-8708da48e39c")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        UnitOfMeasure UnitOfMeasure { get; set; }


        #region Allors
        [Id("773e731d-47f7-4742-b8c6-81dec0a09f29")]
        [AssociationId("183113ef-8420-444d-8a80-61580a9f95dc")]
        [RoleId("05f1428a-26cd-4f08-9f1d-dec02edf6fe1")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]

        Document[] Documents { get; set; }


        #region Allors
        [Id("89a29d59-b56f-4846-a9d2-cf7d094826dc")]
        [AssociationId("8527c099-3ea0-486c-b288-ebf7e642952e")]
        [RoleId("84e90f5a-ce0f-4f88-b964-829154e682dd")]
        #endregion
        [Size(256)]

        string ManufacturerId { get; set; }


        #region Allors
        [Id("8dc701e0-1f66-44ee-acc6-9726aa7d5853")]
        [AssociationId("2b9103c7-7ff8-4733-aa02-53800bb6e9bc")]
        [RoleId("6d60fb2f-1893-48ac-9e7d-9aa2a9a89431")]
        #endregion

        int ReorderLevel { get; set; }


        #region Allors
        [Id("a093c852-cba8-43ff-9572-fd8c6cd53638")]
        [AssociationId("8c3d3a61-4d3a-477c-9701-a292435112e3")]
        [RoleId("f2ffce75-82d5-460f-83cc-621d63211d18")]
        #endregion

        int ReorderQuantity { get; set; }


        #region Allors
        [Id("a202a540-dc0d-4032-9963-d0aa1511c990")]
        [AssociationId("0dd915a3-d517-46c5-8664-e59c56623564")]
        [RoleId("ab316ee2-bf84-4501-a798-94832c55e73f")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]

        PriceComponent[] PriceComponents { get; set; }


        #region Allors
        [Id("f2c3407e-ab62-4f3e-94e5-7e9e65b89d6e")]
        [AssociationId("9bf78bcd-319c-4767-8053-4307577559ff")]
        [RoleId("319781e8-c83c-41ea-a8e7-b7224e8240e0")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        InventoryItemKind InventoryItemKind { get; set; }


        #region Allors
        [Id("43b4b5c7-f94d-4e9b-9ea9-32a4414b11a6")]
        [AssociationId("bb51ade7-37c0-4c7c-b32c-59cb06fa2c9c")]
        [RoleId("ffdc1a53-6fd9-4a21-a576-77c84fc893eb")]
        #endregion
        [Size(256)]

        string Sku { get; set; }

    }
}