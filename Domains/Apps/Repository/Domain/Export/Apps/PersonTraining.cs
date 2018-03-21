namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("6674e32d-c139-4c99-97c5-92354d3ccc4c")]
    #endregion
    public partial class PersonTraining : Period, AccessControlledObject 
    {
        #region inherited properties
        public DateTime FromDate { get; set; }

        public DateTime ThroughDate { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("023864ad-41e1-41cb-8ded-ad2bfa98afe3")]
        [AssociationId("04f1d7c4-1012-4b0e-b38e-02d6abc328be")]
        [RoleId("91bba22d-82b7-4425-ba55-2862f803088d")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public Training Training { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}


        #endregion

    }
}