namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("e3e87d40-b4f0-4953-9716-db13b35d716b")]
    #endregion
    public partial class Good : Product
    {
        #region inherited properties

        public string InternalComment { get; set; }
        public ProductCategory PrimaryProductCategory { get; set; }

        public DateTime SupportDiscontinuationDate { get; set; }

        public DateTime SalesDiscontinuationDate { get; set; }

        public LocalisedText[] LocalisedNames { get; set; }

        public LocalisedText[] LocalisedDescriptions { get; set; }

        public string Description { get; set; }

        public PriceComponent[] VirtualProductPriceComponents { get; set; }

        public string IntrastatCode { get; set; }

        public ProductCategory[] ProductCategoriesExpanded { get; set; }

        public Product ProductComplement { get; set; }

        public ProductFeature[] OptionalFeatures { get; set; }

        public Product[] Variants { get; set; }

        public string Name { get; set; }

        public DateTime IntroductionDate { get; set; }

        public Document[] Documents { get; set; }

        public ProductFeature[] StandardFeatures { get; set; }

        public UnitOfMeasure UnitOfMeasure { get; set; }

        public EstimatedProductCost[] EstimatedProductCosts { get; set; }

        public Product[] ProductObsolescences { get; set; }

        public ProductFeature[] SelectableFeatures { get; set; }

        public VatRate VatRate { get; set; }

        public PriceComponent[] BasePrices { get; set; }

        public ProductCategory[] ProductCategories { get; set; }

        public Guid UniqueId { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

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
        [Id("2ca90db1-8595-4de0-957e-dc4476be1654")]
        [AssociationId("637fa802-fc65-4b5e-aaf5-e49ac5218b9b")]
        [RoleId("64036e01-6767-46d0-aca7-def5876db81f")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public InventoryItemKind InventoryItemKind { get; set; }

        #region Allors
        [Id("4e8eceff-aec2-44f8-9820-4e417ed904c1")]
        [AssociationId("30f4ec83-5854-4a53-a594-ba1247d02b2f")]
        [RoleId("80361383-e1fc-4256-9b69-7cd43469d0de")]
        #endregion
        [Size(256)]
        [Workspace]
        public string BarCode { get; set; }

        #region Allors
        [Id("82295ab2-8488-4d7e-8703-9f7fbec55925")]
        [AssociationId("c1801b8f-013b-42ff-b02a-a6c9b0e361b8")]
        [RoleId("cdc45553-9c60-4c40-8c82-56c488ee6aae")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public FinishedGood FinishedGood { get; set; }

        #region Allors
        [Id("859487f7-9759-4c30-8528-8cd5d014b0a2")]
        [AssociationId("e293bfae-afd7-4f15-8e01-58c9078364b6")]
        [RoleId("3a499b07-0d4f-4f5a-b679-7d76118f8441")]
        #endregion
        [Size(256)]
        [Workspace]
        public string Sku { get; set; }

        #region Allors
        [Id("989d9c6c-56d6-407a-a890-3769cb7a675e")]
        [AssociationId("4da4bb2d-f830-4827-bdaf-1c584cdeb437")]
        [RoleId("c31005b1-787d-4a0f-b281-f74551df7be7")]
        #endregion
        [Size(256)]
        [Workspace]
        public string ArticleNumber { get; set; }

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
        [Id("98d99ee6-6dc1-4ef5-ad5c-e24bcd1dfa27")]
        [AssociationId("60d2c039-b034-4e7f-a677-d65a302d9f5f")]
        [RoleId("eeba67a7-b5c4-4783-b391-b9dd35093efb")]
        #endregion
        [Size(256)]
        [Workspace]
        public string ManufacturerId { get; set; }

        #region Allors
        [Id("50C3BAB5-9BB9-48C0-B41A-9E9072D70C06")]
        [AssociationId("FB33E29C-7338-46C7-A612-A86ACC9051C8")]
        [RoleId("4A324844-A835-4CD7-ACC6-24A817D03BDC")]
        [Indexed]
        #endregion
        [Workspace]
        [Multiplicity(Multiplicity.ManyToOne)]
        public Party SuppliedBy { get; set; }

        #region Allors
        [Id("acbe2dc6-63ad-4910-9752-4cab83e24afb")]
        [AssociationId("70d193cf-8985-4c25-84a5-31f4e2fd2a34")]
        [RoleId("73361510-c5a2-4c4f-afe5-94d2b9eaeea3")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        public Product[] ProductSubstitutions { get; set; }

        #region Allors
        [Id("e1ee15a9-f173-4d81-a11d-82abff076fb4")]
        [AssociationId("20928aed-02cc-4ea1-9640-cd31cb54ba13")]
        [RoleId("e1c65763-9c2d-4111-bca1-638a69490e99")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        public Product[] ProductIncompatibilities { get; set; }

        #region Allors
        [Id("f52c0b7e-dbc4-4082-a2b9-9b1a05ce7179")]
        [AssociationId("50478ca9-3eb4-487b-8c8a-6ff48d9155b5")]
        [RoleId("802b6cdb-873a-4455-9fa7-7f2267407f0f")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        [Workspace]
        public Media PrimaryPhoto { get; set; }

        #region Allors
        [Id("C7FB85EB-CF47-4FE1-BD67-E2832E5893B9")]
        [AssociationId("1DE2FF68-A4CB-4244-8C34-6E9D08A6DFBF")]
        [RoleId("2A1DB194-1B06-498D-BA0D-C2FDA629A45D")]
        [Indexed]
        #endregion
        [Workspace]
        [Multiplicity(Multiplicity.ManyToMany)]
        public Media[] Photos { get; set; }

        #region Allors
        [Id("19449147-C4FB-4FB9-94AB-32B200DD519A")]
        [AssociationId("B6DA1EA1-8A1E-42D3-91AE-EA13C74BFC9A")]
        [RoleId("FB868E3C-D846-4FD8-B5B9-651EF7471C22")]
        #endregion
        [Workspace]
        [Size(-1)]
        public string Keywords { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}





        public void Delete(){}
        #endregion

        public string Comment { get; set; }
    }
}