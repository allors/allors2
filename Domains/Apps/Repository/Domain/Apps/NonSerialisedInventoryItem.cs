namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("5b294591-e20a-4bad-940a-27ae7b2f8770")]
    #endregion
    public partial class NonSerialisedInventoryItem : InventoryItem, NonSerialisedInventoryItemVersioned
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
        public decimal QuantityCommittedOut { get; set; }
        public decimal QuantityOnHand { get; set; }
        public decimal PreviousQuantityOnHand { get; set; }
        public decimal AvailableToPromise { get; set; }
        public decimal QuantityExpectedIn { get; set; }
        public NonSerialisedInventoryItemObjectState CurrentObjectState { get; set; }

        #endregion

        #region Allors
        [Id("53B35269-EF6C-45EE-BE20-FCDC732CE06E")]
        [AssociationId("EBE26248-2154-4F81-B8D3-00628C504A95")]
        [RoleId("5DF2F337-C6FE-4250-B513-D5BB7E13579C")]
        [Indexed]
        #endregion
        [Workspace]
        [Multiplicity(Multiplicity.OneToMany)]
        public NonSerialisedInventoryItemVersion[] AllVersions { get; set; }

        #region Allors
        [Id("4E2486A2-3CF9-4EB6-B675-6565A64116A6")]
        [AssociationId("1E1295F2-DFE0-407C-A625-B6A6972251E0")]
        [RoleId("AE86A6A6-B5D9-459A-9DF9-ECFDC5C90700")]
        [Indexed]
        #endregion
        [Workspace]
        [Multiplicity(Multiplicity.OneToOne)]
        public NonSerialisedInventoryItemVersion CurrentVersion { get; set; }

        #region Allors
        [Id("DBBC2914-1391-4C0F-A9C9-6975637C2A6D")]
        [AssociationId("76713A64-0353-49F0-A1A1-42CC4FAE73C1")]
        [RoleId("426DD9B7-B6C5-4CCC-82BD-610A9CC68F9D")]
        [Indexed]
        #endregion
        [Workspace]
        [Multiplicity(Multiplicity.OneToOne)]
        public NonSerialisedInventoryItemVersion PreviousVersion { get; set; }

        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}




        #endregion
   }
}