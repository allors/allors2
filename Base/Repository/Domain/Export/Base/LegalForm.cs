namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("528cf616-6c67-42e1-af69-b5e6cb1192ea")]
    #endregion
    public partial class LegalForm : Object
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("2867d3b0-5def-4fc6-880a-be4bfe1d2597")]
        [AssociationId("ee4e44e3-2f9b-45fc-8b79-f2ac8e2da434")]
        [RoleId("7aa44ba6-a0b4-403b-aabb-7622ddd2db30")]
        #endregion
        [Required]
        [Workspace]
        public string Description { get; set; }

        #region inherited methods


        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnInit()
        {

        }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        #endregion
    }
}
