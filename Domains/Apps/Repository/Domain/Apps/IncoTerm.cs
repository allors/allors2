namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("BAF23654-7A2D-49A6-81F2-309A61363447")]
    #endregion
    public partial class IncoTerm : AccessControlledObject, AgreementTerm 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public string TermValue { get; set; }

        public TermType TermType { get; set; }

        public string Description { get; set; }

        #endregion

        #region Allors
        [Id("6166D528-ED0D-4343-B3D5-F76E909CDC72")]
        [AssociationId("4DCAEFB8-6A09-438B-ABF5-4B162F104442")]
        [RoleId("1BA0EB35-11E8-4DDD-A395-C7A0FD2579A3")]
        #endregion
        [Workspace]
        public string incoTermCity { get; set; }

        #region Allors
        [Id("FA983A32-4BCA-41A5-956E-2EB5272ACCCF")]
        [AssociationId("94B9AF64-F17A-4302-A7B2-5986FF984C28")]
        [RoleId("4C689854-24B4-4C01-84FC-352580A8E508")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public Country IncoTermCountry { get; set; }

        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}


        #endregion
    }
}