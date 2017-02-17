namespace Allors.Repository.Domain
{
    using System;

    #region Allors
    [Id("e3e87d40-b4f0-4953-9716-db13b35d716b")]
    #endregion
    public partial class Good : Product, Deletable 
    {
        #region inherited properties

        public ProductCharacteristicValue ProductCharacteristicValue { get; set; }

        public ProductCategory PrimaryProductCategory { get; set; }

        public DateTime SupportDiscontinuationDate { get; set; }

        public DateTime SalesDiscontinuationDate { get; set; }

        public LocalisedText[] LocalisedNames { get; set; }

        public LocalisedText[] LocalisedDescriptions { get; set; }

        public LocalisedText[] LocalisedComments { get; set; }

        public string InternalComment { get; set; }

        public string Description { get; set; }

        public PriceComponent[] VirtualProductPriceComponents { get; set; }

        public string IntrastatCode { get; set; }

        public ProductCategory[] ProductCategoriesExpanded { get; set; }

        public Product ProductComplement { get; set; }

        public ProductFeature[] OptionalFeatures { get; set; }

        public Party ManufacturedBy { get; set; }

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

        public InternalOrganisation SoldBy { get; set; }

        public Guid UniqueId { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("B316EB62-A654-4429-9699-403B23DB5284")]
        [AssociationId("F3A6EA79-9E12-405A-8195-90FC3973BD65")]
        [RoleId("BA8E7FFA-8557-4452-B97B-1A5E2BFA83D0")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public ProductType ProductType { get; set; }

        #region Allors
        [Id("04cd1e20-a031-4a4f-9f40-6debb52b002c")]
        [AssociationId("4441b31a-7807-41c6-803b-aeacd18e2867")]
        [RoleId("8dc2ddca-4ae2-48b9-92db-ac68f2f5542e")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        public decimal AvailableToPromise { get; set; }

        #region Allors
        [Id("1e977b9c-8582-48be-ac1d-20a055598290")]
        [AssociationId("be920e49-abff-4ef0-80c2-02df6dfa55e3")]
        [RoleId("67e83a0e-db03-439d-832a-b5685887eeaa")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public Media Thumbnail { get; set; }
        #region Allors
        [Id("2ca90db1-8595-4de0-957e-dc4476be1654")]
        [AssociationId("637fa802-fc65-4b5e-aaf5-e49ac5218b9b")]
        [RoleId("64036e01-6767-46d0-aca7-def5876db81f")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public InventoryItemKind InventoryItemKind { get; set; }
        #region Allors
        [Id("4e8eceff-aec2-44f8-9820-4e417ed904c1")]
        [AssociationId("30f4ec83-5854-4a53-a594-ba1247d02b2f")]
        [RoleId("80361383-e1fc-4256-9b69-7cd43469d0de")]
        #endregion
        [Size(256)]

        public string BarCode { get; set; }
        #region Allors
        [Id("82295ab2-8488-4d7e-8703-9f7fbec55925")]
        [AssociationId("c1801b8f-013b-42ff-b02a-a6c9b0e361b8")]
        [RoleId("cdc45553-9c60-4c40-8c82-56c488ee6aae")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public FinishedGood FinishedGood { get; set; }
        #region Allors
        [Id("859487f7-9759-4c30-8528-8cd5d014b0a2")]
        [AssociationId("e293bfae-afd7-4f15-8e01-58c9078364b6")]
        [RoleId("3a499b07-0d4f-4f5a-b679-7d76118f8441")]
        #endregion
        [Size(256)]

        public string Sku { get; set; }
        #region Allors
        [Id("989d9c6c-56d6-407a-a890-3769cb7a675e")]
        [AssociationId("4da4bb2d-f830-4827-bdaf-1c584cdeb437")]
        [RoleId("c31005b1-787d-4a0f-b281-f74551df7be7")]
        #endregion
        [Size(256)]

        public string ArticleNumber { get; set; }
        #region Allors
        [Id("98d99ee6-6dc1-4ef5-ad5c-e24bcd1dfa27")]
        [AssociationId("60d2c039-b034-4e7f-a677-d65a302d9f5f")]
        [RoleId("eeba67a7-b5c4-4783-b391-b9dd35093efb")]
        #endregion
        [Size(256)]

        public string ManufacturerId { get; set; }
        #region Allors
        [Id("acbe2dc6-63ad-4910-9752-4cab83e24afb")]
        [AssociationId("70d193cf-8985-4c25-84a5-31f4e2fd2a34")]
        [RoleId("73361510-c5a2-4c4f-afe5-94d2b9eaeea3")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]

        public Product[] ProductSubstitutions { get; set; }
        #region Allors
        [Id("e1ee15a9-f173-4d81-a11d-82abff076fb4")]
        [AssociationId("20928aed-02cc-4ea1-9640-cd31cb54ba13")]
        [RoleId("e1c65763-9c2d-4111-bca1-638a69490e99")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]

        public Product[] ProductIncompatibilities { get; set; }
        #region Allors
        [Id("f52c0b7e-dbc4-4082-a2b9-9b1a05ce7179")]
        [AssociationId("50478ca9-3eb4-487b-8c8a-6ff48d9155b5")]
        [RoleId("802b6cdb-873a-4455-9fa7-7f2267407f0f")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public Media Photo { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}





        public void Delete(){}
        #endregion

    }
}