namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("61af6d19-e8e4-4b5b-97e8-3610fbc82605")]
    #endregion
	public partial interface InventoryItem : Transitional, UniquelyIdentifiable, AccessControlledObject 
    {


        #region Allors
        [Id("0f4e5107-cf1e-4fc2-9be5-c3235ce9a7af")]
        [AssociationId("4552144a-0d6e-4a4a-94c7-cafcdd280350")]
        [RoleId("dbb34993-e385-4702-8a50-4cc26193b862")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]

        InventoryItemVariance[] InventoryItemVariances { get; set; }


        #region Allors
        [Id("374e9e57-e878-40ac-9021-d830dbf1efdc")]
        [AssociationId("fb58bcd8-d263-4563-b7d1-d62b6036e8bb")]
        [RoleId("99a9dadc-ac9e-4662-91d0-41804e70101f")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        Part Part { get; set; }


        #region Allors
        [Id("39ee9493-b628-4cc6-a31c-239f306e8497")]
        [AssociationId("04da42d7-6e24-409e-b384-2283dc95ac35")]
        [RoleId("b08500e2-feee-4115-90ec-c65d719d1d29")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        Container Container { get; set; }


        #region Allors
        [Id("57bd3950-8477-44c3-9c16-dd894d774c51")]
        [AssociationId("758170e9-123d-47aa-96f2-ecff7f90c3e0")]
        [RoleId("2618fdd7-d513-4c0d-be0a-12c73225777d")]
        #endregion
        [Derived]
        [Required]
        [Size(256)]

        string Name { get; set; }


        #region Allors
        [Id("5f8fa5ee-a638-4222-9865-518e220e7299")]
        [AssociationId("b6412daa-ac8d-4a73-8b87-cbc91981488c")]
        [RoleId("eb553284-25ab-4cc0-ba20-5b5ef5c74313")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        Lot Lot { get; set; }


        #region Allors
        [Id("64887d2f-3017-4804-afb4-5e46eec23491")]
        [AssociationId("cdb384fc-b2af-49df-a478-c3152c7386ea")]
        [RoleId("4a17bd4c-57a2-4602-b263-6bc9bd0e66aa")]
        #endregion
        [Derived]
        [Required]
        [Size(256)]

        string Sku { get; set; }


        #region Allors
        [Id("6efb2763-ce7e-4b43-afc1-e4e37af814f0")]
        [AssociationId("68c84889-9cb5-43d4-8406-88c3dcfea7aa")]
        [RoleId("e4d19331-72c6-4b4a-bb20-e3beffe3a46e")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]

        UnitOfMeasure UnitOfMeasure { get; set; }


        #region Allors
        [Id("ab7b1a91-4756-4806-a5d3-ed8b392c6fe7")]
        [AssociationId("32ee6a64-bc23-4cb8-a839-2a83d4a68c46")]
        [RoleId("8455bf8a-247f-41d9-b6fc-c30cda0f10f4")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Derived]
        [Indexed]

        ProductCategory[] DerivedProductCategories { get; set; }


        #region Allors
        [Id("b4d944f5-7376-4096-a34a-4571f537c5fc")]
        [AssociationId("78fe1b1f-b4e9-4b75-b16f-6647ef8080ee")]
        [RoleId("6cda2856-203d-409a-b9dd-3ae0c91d7443")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        Good Good { get; set; }


        #region Allors
        [Id("f99da732-7c31-4c67-a610-2147a2f29e44")]
        [AssociationId("300cc6b2-7f50-4399-a3f6-e492d3858524")]
        [RoleId("e1f8b0ea-6b2f-44f1-bfe0-504b8e06f96d")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        Facility Facility { get; set; }

    }
}