namespace Allors.Repository
{
    using Attributes;

    #region Allors
    [Id("")]
    #endregion
    public partial interface InventoryItemVersion : Version
    {
        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        ProductCharacteristicValue[] ProductCharacteristicValues { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Workspace]
        InventoryItemVariance[] InventoryItemVariances { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        Part Part { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        Container Container { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Derived]
        [Required]
        [Size(256)]
        [Workspace]
        string Name { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        Lot Lot { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Derived]
        [Required]
        [Size(256)]
        [Workspace]
        string Sku { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Workspace]
        UnitOfMeasure UnitOfMeasure { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Derived]
        ProductCategory[] DerivedProductCategories { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        Good Good { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        ProductType ProductType { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Required]
        [Workspace]
        Facility Facility { get; set; }
    }
}