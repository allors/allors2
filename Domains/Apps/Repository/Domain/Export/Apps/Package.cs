namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("9371d5fc-748a-4ce4-95eb-6b21aa0ca841")]
    #endregion
    public partial class Package : UniquelyIdentifiable, AccessControlledObject 
    {
        #region inherited properties
        public Guid UniqueId { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("88b49c23-0c4c-4a2d-94aa-c6c8a12ac267")]
        [AssociationId("d1a984e7-2f57-43a0-8cca-e8682407498b")]
        [RoleId("cffa7e90-1c5b-459c-adbe-8fa008b36151")]
        #endregion
        [Required]
        [Size(256)]

        public string Name { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}


        #endregion

    }
}