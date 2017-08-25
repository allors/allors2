namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("C9004999-C582-4AF1-8EDC-CBF3D13284D4")]
    #endregion
    public partial class SerialisedInventoryItemVersion : SerialisedInventoryItemVersioned
    {
        #region inherited properties

        public string SerialNumber { get; set; }
        public Ownership Ownership { get; set; }
        public string Owner { get; set; }
        public int AcquisitionYear { get; set; }
        public int ManufacturingYear { get; set; }
        public decimal ReplacementValue { get; set; }
        public int LifeTime { get; set; }
        public int DepreciationYears { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal ExpectedSalesPrice { get; set; }
        public decimal RefurbishCost { get; set; }
        public decimal TransportCost { get; set; }
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
        public SerialisedInventoryItemObjectState CurrentObjectState { get; set; }

        #endregion

        #region Allors
        [Id("605A66C9-628A-416C-84DA-87699D4564CA")]
        [AssociationId("386C0C30-24AC-40D4-AFFF-D1B769D298A0")]
        [RoleId("6BCD0F6E-ABC5-4837-A5B8-D967F3C01908")]
        #endregion
        public DateTime TimeStamp { get; set; }

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }
        #endregion
    }
}