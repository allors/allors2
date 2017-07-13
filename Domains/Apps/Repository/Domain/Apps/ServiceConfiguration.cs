namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("5d4beea4-f480-460e-92ee-3e8d532ac7f9")]
    #endregion
    public partial class ServiceConfiguration : InventoryItemConfiguration 
    {
        #region inherited properties
        public InventoryItem InventoryItem { get; set; }

        public int Quantity { get; set; }

        public InventoryItem ComponentInventoryItem { get; set; }

        public string Comment { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion


        #region inherited methods


        public void OnBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}



        #endregion

    }
}