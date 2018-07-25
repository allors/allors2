namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("0E82B155-208C-41FD-B7D0-731EADBB5338")]
    #endregion
    public partial class Data : AccessControlledObject 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }
        #endregion

        #region Allors
        [Id("46C310DE-8E36-412E-8068-A9D734734E74")]
        [AssociationId("7F157FB6-8F06-4C71-B0B9-FD3B2E6237AD")]
        [RoleId("3E57DB60-D0C5-4748-8095-31FD10A9DD50")]
        #endregion
        [Size(-1)]
        [Workspace]
        public string String { get; set; }

        #region Allors
        [Id("31D0A290-2637-452D-8462-4BBB744E3065")]
        [AssociationId("0551F665-4510-4CAC-AB4E-C4B67B0C6099")]
        [RoleId("A77403DD-E597-4372-8BC4-61F9F0BA4615")]
        #endregion
        [Workspace]
        public DateTime Date { get; set; }

        #region Allors
        [Id("487A0EF5-C987-4064-BF6B-0B7354EC4315")]
        [AssociationId("49FCDC52-8093-4972-A6E7-0CA9302853F0")]
        [RoleId("4285B1D9-5697-4345-B18C-8EF746F82FB5")]
        #endregion
        [Workspace]
        public DateTime DateTime { get; set; }

        #region inherited methods
        
        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        #endregion
    }
}