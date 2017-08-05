namespace Allors.Repository
{
    using System;

    using Attributes;
    
    #region Allors
    [Id("DE43963D-3505-4B29-8F1F-C24E517D9497")]
    #endregion
    public partial class CustomOrganisationClassification : OrganisationClassification 
    {
        #region inherited properties
        public string Name { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        public void Delete() { }
    
        #endregion

    }
}