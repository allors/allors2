namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("8C95A8D9-BD64-42E6-9EAA-2BC7E843AE22")]
    #endregion
    public partial interface IFaceToFaceCommunication : ICommunicationEvent 
    {
        #region Allors
        [Id("52b8614b-799e-4aea-a012-ea8dbc23f8dd")]
        [AssociationId("ac424847-d426-4614-99a2-37c70841c454")]
        [RoleId("bcf4a8df-8b57-4b3c-a6e5-f9b56c71a13b")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Required]
        [Workspace]
        Party[] Participants { get; set; }

        #region Allors
        [Id("95ae979f-d549-4ea1-87f0-46aa55e4b14a")]
        [AssociationId("d34e4203-0bd2-4fe4-a2ef-9f9f52b49cf9")]
        [RoleId("9f67b296-953d-4e04-b94d-6ffece87ceef")]
        #endregion
        [Size(256)]
        [Workspace]
        string Location { get; set; }
    }
}