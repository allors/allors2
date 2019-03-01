namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("273e69b7-6cda-44d4-b1d6-605b32a6a70d")]
    #endregion
    public partial class Model : AccessControlledObject, Deletable
    {
        #region inherited properties

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("ACB93740-E54D-40DD-BC61-579383AFBDC9")]
        [AssociationId("8A1FE510-5F10-453C-88C7-119DF24DA29C")]
        [RoleId("72811E1A-62A9-498B-8513-7079A459D23E")]
        [Required]
        [Workspace]
        #endregion
        public string Name { get; set; }

        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnInit()
        {
            
        }

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        public void OnPreFinalize(){} public void OnFinalize()
        {
            
        }

        public void OnPostFinalize()
        {
            
        }

        public void Delete()
        {
        }

        #endregion
    }
}