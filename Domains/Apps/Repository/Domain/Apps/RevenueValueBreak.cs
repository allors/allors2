namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("32f8ea23-5ef9-4d2c-86d9-b6f67529c05d")]
    #endregion
    public partial class RevenueValueBreak : AccessControlledObject 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("03baba9a-c9ef-49d0-8fc8-fbdc4bfec949")]
        [AssociationId("c2746ebd-cdd4-4e22-a9fb-d8c4fbcc86da")]
        [RoleId("cec9b76a-7ab9-4c47-a8a8-635ccd374fb0")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public ProductCategory ProductCategory { get; set; }
        #region Allors
        [Id("96391ee1-5ba2-48db-95c9-cec6e73758b7")]
        [AssociationId("846a94f9-72cd-48a7-be94-9e8f146e245a")]
        [RoleId("8ea60dde-6149-4389-bb94-b94e7bcc81b2")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        public decimal ThroughAmount { get; set; }
        #region Allors
        [Id("cf544df2-3ccb-42b5-b009-c355fcf88ed6")]
        [AssociationId("dbdb3b16-701c-4f45-9d38-6b3e21f66ab3")]
        [RoleId("44217cbb-1f44-4c04-bb66-e8bf597df3f6")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        public decimal FromAmount { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        #endregion

    }
}