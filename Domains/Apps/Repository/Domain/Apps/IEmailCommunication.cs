namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("2DFEE5CC-093C-4735-B53A-7937AFC4A5AB")]
    #endregion
    public partial interface IEmailCommunication : ICommunicationEvent
    {
        #region Allors
        [Id("25b8aa5e-e7c5-4689-b1ed-d9a0ba47b8eb")]
        [AssociationId("11649936-a5fa-488e-8d17-e80619c4d634")]
        [RoleId("6219fd3b-4f38-4f8f-8a5a-783f908ef55a")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Required]
        [Workspace]
        EmailAddress Originator { get; set; }

        #region Allors
        [Id("4026fcf7-3fc2-494b-9c4a-3e19eed74134")]
        [AssociationId("f2febf7f-7917-4499-8546-cae1e53d6791")]
        [RoleId("50439b5a-2251-469c-8512-f9dc65b0d9f6")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Required]
        [Workspace]
        EmailAddress[] Addressees { get; set; }

        #region Allors
        [Id("4f696f91-e185-4d3d-bf40-40e6c2b02eb4")]
        [AssociationId("a19fe8f6-a3b9-4d59-b2e6-cfc19cc01a58")]
        [RoleId("661f4ae9-684b-4b56-9ec6-7bf9fbfea4ab")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        EmailAddress[] CarbonCopies { get; set; }

        #region Allors
        [Id("dd7506bb-4daa-4da7-8f20-3f607c944959")]
        [AssociationId("42fb79f1-c891-41bf-be4b-a2717bd94e69")]
        [RoleId("6d75e51a-7994-43bb-9e99-cd0a88d9d8f2")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        EmailAddress[] BlindCopies { get; set; }

        #region Allors
        [Id("e12818ad-4ffd-4d91-8142-4ac9bfcbc146")]
        [AssociationId("a44a8d84-2510-45fd-add1-646f84be072d")]
        [RoleId("ae354426-6273-4b09-aabf-3f6d25f86e56")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        EmailTemplate EmailTemplate { get; set; }

        #region Allors
        [Id("3F61CB07-4E36-4AA3-AE0D-ABAC9D95DB49")]
        [AssociationId("8E130A0F-A905-4420-A661-D40BD14C8100")]
        [RoleId("B6702349-D126-4244-A0EF-214F8043A52E")]
        #endregion
        [Workspace]
        [Required]
        bool IncomingMail { get; set; }
    }
}