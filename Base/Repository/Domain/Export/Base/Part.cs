namespace Allors.Repository
{
    using Attributes;

    #region Allors
    [Id("894BE589-D536-4FEB-8B94-8E127A170F80")]
    #endregion
    public partial interface Part : UnifiedProduct
    {
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
        Facility DefaultFacility { get; set; }

        #region Allors
        [Id("527c0d02-7723-4715-b975-ec9474d0d22d")]
        [AssociationId("b8cce82f-8555-4d15-8012-3b122ad47b3d")]
        [RoleId("72e60215-a8fb-40a1-ac9b-0204421adde0")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        PartSpecification[] PartSpecifications { get; set; }

        #region Allors
        [Id("5f727bd9-9c3e-421e-93eb-646c4fdf73d3")]
        [AssociationId("210976bb-e440-44ee-b2b5-39bcee04965b")]
        [RoleId("3165a365-a0db-4ce6-b194-7636cc9c015a")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        Party ManufacturedBy { get; set; }

        #region Allors
        [Id("50C3BAB5-9BB9-48C0-B41A-9E9072D70C06")]
        [AssociationId("FB33E29C-7338-46C7-A612-A86ACC9051C8")]
        [RoleId("4A324844-A835-4CD7-ACC6-24A817D03BDC")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Workspace]
        [Derived]
        Party[] SuppliedBy { get; set; }

        #region Allors
        [Id("B615880B-DA81-4437-A59B-F6350A812249")]
        [AssociationId("C8C3FB82-05C1-4680-8F48-0542046D4CD3")]
        [RoleId("C4095B6C-D5ED-49F2-B7E1-54A011A6C2B0")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        Brand Brand { get; set; }

        #region Allors
        [Id("DCDC68FD-B69B-4320-8224-0B304EBDD62C")]
        [AssociationId("B99B9347-1EAD-4761-9E9E-093FE9D6D485")]
        [RoleId("2BC21680-B7A8-47B7-97BD-66A883FC3AB7")]
        #endregion
        [Workspace]
        [Size(10)]
        string HsCode { get; set; }

        #region Allors
        [Id("B6EB8A17-3092-44F0-86D1-59162208D5B9")]
        [AssociationId("89E99C39-BD79-4261-AD72-2FDFA574E663")]
        [RoleId("B8D42797-6D96-4A18-B953-DB33840044FB")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        Model Model { get; set; }

        #region Allors
        [Id("8dc701e0-1f66-44ee-acc6-9726aa7d5853")]
        [AssociationId("2b9103c7-7ff8-4733-aa02-53800bb6e9bc")]
        [RoleId("6d60fb2f-1893-48ac-9e7d-9aa2a9a89431")]
        #endregion
        [Workspace]
        int ReorderLevel { get; set; }

        #region Allors
        [Id("a093c852-cba8-43ff-9572-fd8c6cd53638")]
        [AssociationId("8c3d3a61-4d3a-477c-9701-a292435112e3")]
        [RoleId("f2ffce75-82d5-460f-83cc-621d63211d18")]
        #endregion
        [Workspace]
        int ReorderQuantity { get; set; }

        #region Allors
        [Id("f2c3407e-ab62-4f3e-94e5-7e9e65b89d6e")]
        [AssociationId("9bf78bcd-319c-4767-8053-4307577559ff")]
        [RoleId("319781e8-c83c-41ea-a8e7-b7224e8240e0")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        InventoryItemKind InventoryItemKind { get; set; }

        #region Allors
        [Id("B316EB62-A654-4429-9699-403B23DB5284")]
        [AssociationId("F3A6EA79-9E12-405A-8195-90FC3973BD65")]
        [RoleId("BA8E7FFA-8557-4452-B97B-1A5E2BFA83D0")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        ProductType ProductType { get; set; }

        #region Allors
        [Id("CA9F9403-B31F-4A44-9019-86272E21C1D8")]
        [AssociationId("66BD70AA-EF92-42E6-B0DF-30319CF88D6E")]
        [RoleId("BF290FC2-3677-4072-AEDF-6943958E1AA7")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        SerialisedItem[] SerialisedItems { get; set; }

        #region Allors
        [Id("ACD0DFBF-030B-410B-9A7B-E04CC748EA2D")]
        [AssociationId("1EF32761-196E-4127-AD61-249436EBB857")]
        [RoleId("40B5DFAD-4273-42EF-A0DD-C1F1143F9A82")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        SerialisedItemCharacteristic[] SerialisedItemCharacteristics { get; set; }

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
        decimal QuantityOnHand { get; set; }

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
        decimal AvailableToPromise { get; set; }

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
        decimal QuantityCommittedOut { get; set; }

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
        decimal QuantityExpectedIn { get; set; }
    }
}
