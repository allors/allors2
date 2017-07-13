namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("f13976d4-b1f4-4b78-a720-beab1e0a7e4c")]
    #endregion
    public partial class CustomerShipmentStatus : AccessControlledObject 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("591d3237-220b-4765-8001-4bc18ecd2d8c")]
        [AssociationId("2a2704d6-44f6-4e86-a8c9-407842b7eb83")]
        [RoleId("fd56e773-b27b-4336-b1be-e262d1d26b41")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public CustomerShipmentObjectState CustomerShipmentObjectState { get; set; }
        #region Allors
        [Id("74e826e5-75d0-4e7d-b2fe-73a7c58e30ef")]
        [AssociationId("261ca695-c146-493d-b059-3836913268c4")]
        [RoleId("eb029966-6353-401b-b24b-190460f0c035")]
        #endregion
        [Required]

        public DateTime StartDateTime { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        #endregion

    }
}