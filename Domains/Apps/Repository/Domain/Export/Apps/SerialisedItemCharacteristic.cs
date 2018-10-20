namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("A34F6226-7837-4905-9125-61CD00A07BF4")]
    #endregion
    public partial class SerialisedItemCharacteristic : AccessControlledObject, Deletable
    {
        #region inherited properties

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("73A04D99-CD9F-41F7-AA1C-B4CD80AF60AD")]
        [AssociationId("61DC8595-D02C-485C-9EC8-09470B33EF9E")]
        [RoleId("216DA9A4-BF58-44E9-B315-A9ED222122C8")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Required]
        [Workspace]
        public SerialisedItemCharacteristicType SerialisedItemCharacteristicType { get; set; }

        #region Allors
        [Id("E68E6931-F10C-4F04-A23E-B2BC82AC6D5C")]
        [AssociationId("3757E6C2-789B-4711-A366-F018212A2109")]
        [RoleId("0BC3A4E1-C3EB-4312-A699-D28EB778EA05")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Size(-1)]
        [Workspace]
        public string Value { get; set; }

        #region Allors
        [Id("EE9688B5-B93C-4911-914D-E76E4E4825B0")]
        [AssociationId("BF0F44AD-9E01-41E3-B880-579A4E3D46CB")]
        [RoleId("BDF36413-9690-445B-98C1-B7522C6C875A")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public LocalisedText[] LocalisedValues { get; set; }

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        public void Delete() { }
        
        #endregion
    }
}