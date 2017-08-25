namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("BDCCB8B3-F8A1-4905-BFF9-92144A9C36EE")]
    #endregion
    public partial class NonSerialisedInventoryItemVersion : NonSerialisedInventoryItemVersioned
    {
        #region inherited properties

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
        [Id("905B1A1A-920E-42D8-BCBB-F379331C19E2")]
        [AssociationId("80D6D3E7-86DB-4EF4-9EC4-E67F9D616A9F")]
        [RoleId("49761B86-4C69-4015-BB35-558F3719FD74")]
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