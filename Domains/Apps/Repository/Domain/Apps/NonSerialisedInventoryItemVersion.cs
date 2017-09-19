namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("")]
    #endregion
    public partial class NonSerialisedInventoryItemVersion : InventoryItemVersion
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }
        public SecurityToken[] SecurityTokens { get; set; }
        public ObjectState PreviousObjectState { get; set; }
        public ObjectState LastObjectState { get; set; }
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

        public DateTime TimeStamp { get; set; }
        #endregion

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal QuantityCommittedOut { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal QuantityOnHand { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal PreviousQuantityOnHand { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal AvailableToPromise { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal QuantityExpectedIn { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        [Indexed]
        #endregion
        [Workspace]
        [Multiplicity(Multiplicity.ManyToOne)]
        public NonSerialisedInventoryItemObjectState CurrentObjectState { get; set; }

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }
        #endregion
    }
}