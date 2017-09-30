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
        public ObjectState[] PreviousObjectStates { get; set; }

        public ObjectState[] LastObjectStates { get; set; }

        public ObjectState[] ObjectStates { get; set; }

        public Permission[] DeniedPermissions { get; set; }
        public SecurityToken[] SecurityTokens { get; set; }
        public Guid UniqueId { get; set; }
        public ProductCharacteristicValue[] ProductCharacteristicValues { get; set; }
        public InventoryItemVariance[] InventoryItemVariances { get; set; }
        public Part Part { get; set; }
        public string Name { get; set; }
        public Lot Lot { get; set; }
        public string Sku { get; set; }
        public UnitOfMeasure UnitOfMeasure { get; set; }
        public ProductCategory[] DerivedProductCategories { get; set; }
        public Good Good { get; set; }
        public ProductType ProductType { get; set; }
        public Facility Facility { get; set; }

        #endregion

        #region ObjectStates
        #region NonSerialisedInventoryItemState
        #region Allors
        [Id("35D3FF5B-AA47-41F9-A44F-7809EC2D7955")]
        [AssociationId("EBE15EA6-05F8-4EC0-8CD5-E5773A836EC4")]
        [RoleId("624BCFA2-C488-4404-ACFA-8D4EC7CC1B7D")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public NonSerialisedInventoryItemState PreviousNonSerialisedInventoryItemState { get; set; }

        #region Allors
        [Id("4524D9FF-A484-49BD-B8BC-74C4D488FDC3")]
        [AssociationId("43452B62-BDD8-41C9-85DA-EE9DF093A917")]
        [RoleId("1F6FAA52-BC38-400D-BC04-9D7E0499F9AD")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public NonSerialisedInventoryItemState LastNonSerialisedInventoryItemState { get; set; }

        #region Allors
        [Id("B31DEEC8-709E-4049-989A-D4BD3028A166")]
        [AssociationId("D3D5E468-4F4C-4EFE-822F-C9CA753C0CA6")]
        [RoleId("731CBA99-ABD0-4C7A-A38A-B606E4E42812")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public NonSerialisedInventoryItemState NonSerialisedInventoryItemState { get; set; }
        #endregion
        #endregion

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

        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}




        #endregion
    }
}