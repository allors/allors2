namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("9b9f2a59-ae10-49df-b0b5-98b48ec99157")]
    #endregion
    public partial class WorkEffortFixedAssetStandard : AccessControlledObject 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("5aca8d2b-0073-4890-b02a-f4c9a5fc8a2b")]
        [AssociationId("b20e8fbd-4493-4a13-ade6-a42ecc8e6793")]
        [RoleId("1ef2d6ea-7662-4a8b-83e3-712f8b7bda9a")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        public decimal EstimatedCost { get; set; }
        #region Allors
        [Id("73900f38-242a-4aac-ba8e-d8ffa57a125f")]
        [AssociationId("87bc4caf-7953-4805-816a-e6e6af4cfc19")]
        [RoleId("10eed02a-ae4b-483e-b912-170ec39bb92b")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        public decimal EstimatedDuration { get; set; }
        #region Allors
        [Id("98ca7e1a-8f15-4533-9de7-819b6c868788")]
        [AssociationId("da3497fc-7c30-4760-bfec-2bbc8d5ebf5b")]
        [RoleId("792772c6-0c04-417c-a22a-479f4c5cf35f")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public FixedAsset FixedAsset { get; set; }
        #region Allors
        [Id("b9d782af-1f4c-4639-bd11-fda3651982df")]
        [AssociationId("c17dbf4f-504f-4b9e-ba5c-25545e1386d0")]
        [RoleId("7a4ecdb9-2b88-4f41-9fe4-fe1016b12ad8")]
        #endregion

        public int EstimatedQuantity { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        #endregion

    }
}