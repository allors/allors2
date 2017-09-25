namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("cd60cf6d-65ba-4e31-b85d-16c19fc0978b")]
    #endregion
    public partial class QuoteTerm : AccessControlledObject 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("ce70acf3-9bc4-4572-9487-ef1ab900b488")]
        [AssociationId("df24f334-df05-48b2-95c8-dc69bafbdf06")]
        [RoleId("c64eb6c1-0bf8-4504-8c35-e4753f050911")]
        #endregion
        [Size(256)]

        public string TermValue { get; set; }
        #region Allors
        [Id("e53203f0-1d8f-45ea-bcc2-627c9440e66f")]
        [AssociationId("8319e551-dc5c-461e-bbf2-6c37b50becce")]
        [RoleId("88fc03e5-6ab5-4b95-9027-282a595ca3f7")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public TermType TermType { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        #endregion

    }
}