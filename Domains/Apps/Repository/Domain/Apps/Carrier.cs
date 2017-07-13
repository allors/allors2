namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("4f46f32a-04e6-4ccc-829b-68fb3336f870")]
    #endregion
    public partial class Carrier : UniquelyIdentifiable, AccessControlledObject 
    {
        #region inherited properties
        public Guid UniqueId { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("8defc9c0-6cc8-4e8a-b892-dad6ff908b85")]
        [AssociationId("9a0673e4-8c79-4677-a542-e17f4211d74d")]
        [RoleId("cde2981f-9ba6-4c85-a0cc-b98bd3b7a8a2")]
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