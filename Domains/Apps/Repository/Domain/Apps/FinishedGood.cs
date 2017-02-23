namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("364071a2-bcda-4bdc-b0f9-0e56d28604d6")]
    #endregion
    public partial class FinishedGood : Part 
    {
        #region inherited properties
        public InternalOrganisation OwnedByParty { get; set; }

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