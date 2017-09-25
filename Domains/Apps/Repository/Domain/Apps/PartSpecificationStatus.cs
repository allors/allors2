namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("f1ae6225-15d0-4359-8188-afb73265a617")]
    #endregion
    public partial class PartSpecificationStatus : AccessControlledObject 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("14fdbce7-3494-48fb-a885-3b688b0c4e15")]
        [AssociationId("9b060799-5fca-4e1f-96c0-953444f4b6ac")]
        [RoleId("53c22224-4741-4b2a-ac1f-2174c1bda312")]
        #endregion
        [Required]

        public DateTime StartDateTime { get; set; }
        #region Allors
        [Id("3b3db2a8-bd50-422b-8605-01142cac2654")]
        [AssociationId("f3ae080e-2c22-46e2-9a78-2178f32eab55")]
        [RoleId("a62f66c9-5ed6-4059-8c2e-3a01e268f4eb")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public PartSpecificationObjectState PartSpecificationObjectState { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        #endregion

    }
}