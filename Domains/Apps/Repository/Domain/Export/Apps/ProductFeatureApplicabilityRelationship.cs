namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("003433eb-a0c6-454d-8517-0c03e9be3e96")]
    #endregion
    public partial class ProductFeatureApplicabilityRelationship : AccessControlledObject 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("3198ade4-8080-4584-9b67-b00af681c5cf")]
        [AssociationId("d0f5e3af-01ea-44fc-8921-a7eec052ed22")]
        [RoleId("73ff3323-7903-42c7-8278-b5f36f547463")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public Product AvailableFor { get; set; }
        #region Allors
        [Id("c17d3bde-ebbc-463c-b9cb-b0a5a700c6a1")]
        [AssociationId("323a85e8-ee5c-4967-9f3d-64e8e5b04d7c")]
        [RoleId("22a7598e-6862-4627-b380-06804e263871")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public ProductFeature UsedToDefine { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        #endregion

    }
}