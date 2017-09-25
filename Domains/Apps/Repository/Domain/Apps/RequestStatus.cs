namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("93D6BC00-A10A-482F-B773-1593EFC3F859")]
    #endregion
    public partial class RequestStatus : AccessControlledObject 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("640B2B34-6797-4320-A889-CC6972C3A150")]
        [AssociationId("9F60DA1C-B687-4FD4-AB68-122C8F606562")]
        [RoleId("D0B06525-3161-4F62-BB20-856A93080CBE")]
        #endregion
        [Required]
        [Workspace]
        public DateTime StartDateTime { get; set; }

        #region Allors
        [Id("3DB758E9-A8DD-4F58-BDA5-C6B13DE6F246")]
        [AssociationId("6975F29A-D34B-486E-A5FF-DE5A74590170")]
        [RoleId("3F61F9CC-0CA4-4915-955C-9773B5C17929")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public RequestObjectState RequestObjectState { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        #endregion

    }
}