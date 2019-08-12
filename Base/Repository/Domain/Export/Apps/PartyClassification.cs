namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("3bb83aa5-e58a-4421-bdbc-3c9fa0b2324f")]
    #endregion
	public partial interface PartyClassification : Object
    {
        #region Allors
        [Id("4f35ae7e-fe06-4a3b-abe1-adb78fcf2e6b")]
        [AssociationId("fd171d61-90ae-4169-8286-6054b82569a1")]
        [RoleId("654f2aca-2eb7-495c-a739-82c38a629130")]
        #endregion
        [Required]
        [Size(256)]
        [Workspace]
        string Name { get; set; }

        #region Allors

        [Id("0058B700-0F45-4EFD-8094-4BE6404BF502")]

        #endregion

        [Workspace]
        void Delete();
    }
}