namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("41D19E80-8ABB-4515-AA44-3E0AF1146AE7")]
    #endregion
    public partial class NonSerialisedInventoryItemState : ObjectState 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public string Name { get; set; }

        public Guid UniqueId { get; set; }

        #endregion

        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnInit()
        {
            
        }

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        #endregion

        #region Allors
        [Id("1AB4364F-2DAA-4EC0-BF2E-77CAF2E354CD")]
        [AssociationId("26B6D1A8-FBD9-4775-9B59-34883EAC415F")]
        [RoleId("77CEB46D-8FDD-43E2-9FAC-8E9017357C02")]
        #endregion
        [Required]
        [Workspace]
        public bool AvailableForSale { get; set; }
    }
}