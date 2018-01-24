namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("b1a10fe4-2d84-452b-b0cb-e96e55014856")]
    #endregion
    public partial class SubAssembly : Part 
    {
        #region inherited properties

        public InternalOrganisation InternalOrganisation { get; set; }

        public string Name { get; set; }

        public PartSpecification[] PartSpecifications { get; set; }

        public UnitOfMeasure UnitOfMeasure { get; set; }

        public Document[] Documents { get; set; }

        public string ManufacturerId { get; set; }

        public int ReorderLevel { get; set; }

        public int ReorderQuantity { get; set; }

        public PriceComponent[] PriceComponents { get; set; }

        public InventoryItemKind InventoryItemKind { get; set; }

        public string Sku { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public Guid UniqueId { get; set; }

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