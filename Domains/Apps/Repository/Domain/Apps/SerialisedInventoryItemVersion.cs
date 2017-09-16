namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("C9004999-C582-4AF1-8EDC-CBF3D13284D4")]
    #endregion
    public partial class SerialisedInventoryItemVersion : ISerialisedInventoryItem
    {
        #region inherited properties

        public Permission[] DeniedPermissions { get; set; }
        public SecurityToken[] SecurityTokens { get; set; }
        public ObjectState PreviousObjectState { get; set; }
        public ObjectState LastObjectState { get; set; }
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
        [Id("BAA3A811-6A2E-4DAC-AD06-7D244A394A58")]
        [AssociationId("48DE7487-E0B3-47EC-B7EF-24C2444CC9DB")]
        [RoleId("C3F402BC-9BA9-4AA5-BBA8-6B60A947B6EB")]
        #endregion
        [Workspace]
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