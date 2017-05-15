namespace Allors.Repository
{
    using Attributes;

    #region Allors
    [Id("020f5d4d-4a59-4d7b-865a-d72fc70e4d97")]
    #endregion
    public partial class LocalisedText : AccessControlledObject, Localised 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public Locale Locale { get; set; }

        #endregion

        #region Allors
        [Id("50dc85f0-3d22-4bc1-95d9-153674b89f7a")]
        [AssociationId("accd061b-20b9-4a24-bb2c-c2f7276f43ab")]
        [RoleId("8d3f68e1-fa6e-414f-aa4d-25fcc2c975d6")]
        #endregion
        [Required]
        [Size(-1)]
        public string Text { get; set; }

        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}


        #endregion
    }
}