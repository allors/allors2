namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("3b9f21f4-2f2c-47a9-9c76-15f5ef4f5e00")]
    #endregion
    public partial class CustomerRelationship : Period, Deletable, AccessControlledObject
    {
        #region inherited properties

        public DateTime FromDate { get; set; }

        public DateTime ThroughDate { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("5c7c79e1-6b61-4f64-b8d1-608984f91268")]
        [AssociationId("9ce91d5f-12af-44a5-97a9-16c1b9986f67")]
        [RoleId("74a36a15-f48a-4794-ac10-2c0860cc4ca1")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public Party Customer { get; set; }

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