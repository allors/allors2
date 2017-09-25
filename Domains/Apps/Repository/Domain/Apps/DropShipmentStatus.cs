namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("6fadcefe-2972-480d-9d38-d4207e199d48")]
    #endregion
    public partial class DropShipmentStatus : AccessControlledObject 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("2c667e0e-f489-470d-9f48-09a443588286")]
        [AssociationId("4f08a754-40ac-4806-bf34-fcc4b0d473af")]
        [RoleId("bd4fb4a3-c2ba-4ed6-8f09-4d7e8e8e3a9a")]
        #endregion
        [Required]

        public DateTime StartDateTime { get; set; }
        #region Allors
        [Id("3fe0bb83-1d87-4a7c-bda8-796cdaea9ac3")]
        [AssociationId("5098fab9-4d58-43e1-93b5-b95e090952e1")]
        [RoleId("09b78c50-2eef-4f01-9277-8a14a657f6b9")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public DropShipmentObjectState DropShipmentObjectState { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        #endregion

    }
}