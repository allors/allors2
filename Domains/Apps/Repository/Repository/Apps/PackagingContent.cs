namespace Allors.Repository.Domain
{
    using System;

    #region Allors
    [Id("1c05a785-2de1-48fa-813f-6e740f6f7cec")]
    #endregion
    public partial class PackagingContent : AccessControlledObject 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("316a8ff4-1073-486e-ad62-5bee3d3504d2")]
        [AssociationId("c2970739-17c4-488a-8f12-9e35ad72d311")]
        [RoleId("536e372d-5062-418a-b17e-752ebf32d430")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public ShipmentItem ShipmentItem { get; set; }
        #region Allors
        [Id("ca8bcf75-c40e-4d73-8d0c-f35d1005f73b")]
        [AssociationId("a97a1fd4-6d74-424c-aab4-909bdd198856")]
        [RoleId("db47bbd5-e9d8-4dea-801f-bae1c49fe67c")]
        #endregion
        [Required]
        [Precision(19)]
        [Scale(2)]
        public decimal Quantity { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        #endregion

    }
}