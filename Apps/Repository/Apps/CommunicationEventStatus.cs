namespace Allors.Repository.Domain
{
    using System;

    #region Allors
    [Id("e2c3f3fa-7b94-4315-b8dd-2f538d8e2132")]
    #endregion
    public partial class CommunicationEventStatus : Deletable, AccessControlledObject 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("414fc983-3086-4362-806a-d77b09f04b24")]
        [AssociationId("0da368d4-31c3-45b3-b556-561b080a03a5")]
        [RoleId("d0e76e79-e797-44dd-8256-f88c10b1d440")]
        #endregion
        [Required]

        public DateTime StartDateTime { get; set; }
        #region Allors
        [Id("5ad71d39-d9e2-4b08-a6ac-322c18f14be5")]
        [AssociationId("51e4f7d6-a511-493e-8207-b60343fccae6")]
        [RoleId("4d04158e-c9a4-490c-9a35-c2205a01938a")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public CommunicationEventObjectState CommunicationEventObjectState { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}


        public void Delete(){}

        #endregion

    }
}