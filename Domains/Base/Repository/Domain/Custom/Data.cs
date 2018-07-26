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
        [Id("36FA4EB8-5EA9-4F56-B5AA-9908EF2B417F")]
        [AssociationId("C0CA43A1-9C16-42BA-B83B-5E6C72DCB605")]
        [RoleId("5F26C1A3-BD24-465B-A4F9-D7A5D79A5C80")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public Person AutocompleteFilter { get; set; }

        #region Allors
        [Id("C1C4D5D9-EEC0-44B5-9317-713E9AB2277E")]
        [AssociationId("9ED53BA6-6B03-448D-B2E7-42AD045BEEC3")]
        [RoleId("4B25DD13-A74D-483C-A0C4-7A5491B9D955")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public Person AutocompleteOptions { get; set; }
        
        #region Allors
        [Id("46964F62-AF12-4450-83DA-C695C4A0ECE8")]
        [AssociationId("4E112908-E5B4-448C-B6A6-58094165522B")]
        [RoleId("BA0EA6C5-E62F-487B-B57C-D7412A6BF958")]
        #endregion
        [Workspace]
        public bool Checkbox { get; set; }

        #region Allors
        [Id("7E098B17-2ECB-4D1C-AA73-80684394BD9B")]
        [AssociationId("D13FDDE0-8817-4B13-BE41-D54ED349813F")]
        [RoleId("903F0C58-0867-49D8-B3F7-EA1A6F89EA35")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public Person[] Chips { get; set; }

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