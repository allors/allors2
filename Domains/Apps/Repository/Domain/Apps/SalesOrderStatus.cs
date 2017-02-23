namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("347ee1c4-5275-4ea7-a349-6bab2de45aff")]
    #endregion
    public partial class SalesOrderStatus : AccessControlledObject 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("4c0986f4-c033-4646-b062-da9699bd8455")]
        [AssociationId("96c8b409-1ac9-4f31-a6a2-191bee4a1a82")]
        [RoleId("99a005bb-6961-4a19-bedc-5bdce1829cb9")]
        #endregion
        [Required]

        public DateTime StartDateTime { get; set; }
        #region Allors
        [Id("e61dabc2-729c-41cc-8d89-aea6e6557914")]
        [AssociationId("f3b9f2c8-18b5-4334-99e5-7f4f4eee7571")]
        [RoleId("2e1c48fe-536b-4b2a-8e49-7320c961d42c")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public SalesOrderObjectState SalesOrderObjectState { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        #endregion

    }
}