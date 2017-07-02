namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("0C9E5DF7-5D30-4BA7-9874-0AF7CF87B785")]
    #endregion
    public partial class RequestItemStatus : AccessControlledObject 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("4FC5D9F5-8E25-4346-8BF6-26E8EDB37E5B")]
        [AssociationId("A5E1B608-DA6D-486A-A63E-7AF7133D9FF6")]
        [RoleId("0337E134-069A-456C-BC9B-F5A619605741")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public RequestItemObjectState RequestItemObjectState { get; set; }

        #region Allors
        [Id("681DFC28-59C5-4726-9B72-136A4FAF435C")]
        [AssociationId("1C513F2F-77A4-44A7-982A-F14BB3B7835C")]
        [RoleId("4AE8A192-B193-4EC3-8860-22EDD18F8316")]
        #endregion
        [Required]
        [Workspace]
        public DateTime StartDateTime { get; set; }

        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        #endregion
    }
}