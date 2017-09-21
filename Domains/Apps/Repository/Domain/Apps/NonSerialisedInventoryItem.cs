namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("5b294591-e20a-4bad-940a-27ae7b2f8770")]
    #endregion
    public partial class NonSerialisedInventoryItem : InventoryItem, Versioned
    {
        #region inherited properties

        public Permission[] DeniedPermissions { get; set; }
        public SecurityToken[] SecurityTokens { get; set; }
        public ObjectState PreviousObjectState { get; set; }
        public ObjectState LastObjectState { get; set; }
        public Guid UniqueId { get; set; }
        public ProductCharacteristicValue[] ProductCharacteristicValues { get; set; }
        public InventoryItemVariance[] InventoryItemVariances { get; set; }
        public Part Part { get; set; }
        public Container Container { get; set; }
        public string Name { get; set; }
        public Lot Lot { get; set; }
        public string Sku { get; set; }
        public UnitOfMeasure UnitOfMeasure { get; set; }
        public ProductCategory[] DerivedProductCategories { get; set; }
        public Good Good { get; set; }
        public ProductType ProductType { get; set; }
        public Facility Facility { get; set; }

        #endregion

        #region Allors
        [Id("2959a4d0-5945-4231-8a12-a2d1bdb9be04")]
        [AssociationId("d48f3a6f-915f-42fe-a508-8cddc3cf3fbc")]
        [RoleId("bd3e6dd7-c339-4ac4-bdce-31526ed7fa1a")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal QuantityCommittedOut { get; set; }

        #region Allors
        [Id("a6b78e16-6aef-4478-b426-9429c1a01059")]
        [AssociationId("9bcc50ce-a070-4cdd-802f-4296908b75f7")]
        [RoleId("a44947f1-b7e2-4f0c-97d6-2fd32ecae097")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal QuantityOnHand { get; set; }

        #region Allors
        [Id("ba5e2476-abdd-4d61-8a14-5d99a36c4544")]
        [AssociationId("f1e3216e-1af7-4354-b8ac-258bfa9222ac")]
        [RoleId("4d41e84c-ee79-4ce2-874e-a000e30c1120")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal PreviousQuantityOnHand { get; set; }

        #region Allors
        [Id("dfbd2b04-306c-415c-af67-895810b01044")]
        [AssociationId("c1ec09e8-2c1e-4e4a-9496-8c081dee23d9")]
        [RoleId("9a56d091-f6a8-4db1-bd65-10d84eaaaa05")]
        #endregion
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal AvailableToPromise { get; set; }

        #region Allors
        [Id("eb32d183-9c7b-47a7-ab38-e4966d745161")]
        [AssociationId("a7512a69-d27e-47dc-9da5-8713489cc2e5")]
        [RoleId("9aaf1a36-04b9-4cc5-9a22-691b3b3c4633")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal QuantityExpectedIn { get; set; }

        #region Allors
        [Id("886BDFE1-5195-4961-8A50-A4A3876A1379")]
        [AssociationId("E10056D8-DB69-4959-86EF-331D0E61F5C1")]
        [RoleId("7D6D6B50-8E94-41DF-A132-B29ED3EA3DA1")]
        [Indexed]
        #endregion
        [Workspace]
        [Multiplicity(Multiplicity.ManyToOne)]
        public NonSerialisedInventoryItemObjectState CurrentObjectState { get; set; }

        #region Versioning
        #region Allors
        [Id("4E2486A2-3CF9-4EB6-B675-6565A64116A6")]
        [AssociationId("1E1295F2-DFE0-407C-A625-B6A6972251E0")]
        [RoleId("AE86A6A6-B5D9-459A-9DF9-ECFDC5C90700")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public NonSerialisedInventoryItemVersion CurrentVersion { get; set; }

        #region Allors
        [Id("53B35269-EF6C-45EE-BE20-FCDC732CE06E")]
        [AssociationId("EBE26248-2154-4F81-B8D3-00628C504A95")]
        [RoleId("5DF2F337-C6FE-4250-B513-D5BB7E13579C")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public NonSerialisedInventoryItemVersion[] AllVersions { get; set; }
        #endregion

        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}




        #endregion
   }
}