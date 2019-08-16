namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("b6c168d6-3d5c-4f5f-b6c6-d348600f1483")]
    #endregion
    public partial class ManufacturingConfiguration : InventoryItemConfiguration
    {
        #region inherited properties
        public InventoryItem InventoryItem { get; set; }

        public int Quantity { get; set; }

        public InventoryItem ComponentInventoryItem { get; set; }

        public string Comment { get; set; }

        public LocalisedText[] LocalisedComments { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnInit()
        {

        }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        #endregion
    }
}
