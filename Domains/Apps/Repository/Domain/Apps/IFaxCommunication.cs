namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("337DA6F8-E79F-4F29-973C-0175D8F20A8C")]
    #endregion
    public partial interface IFaxCommunication : ICommunicationEvent 
    {
        #region Allors
        [Id("3c4bea84-e00e-4ab3-8d40-5de7f394e835")]
        [AssociationId("30a33d23-6c06-45cc-8cef-25a2d02cfc5f")]
        [RoleId("c3ad4d30-c9ef-41da-b7de-f71c625b8549")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        Party Originator { get; set; }

        #region Allors
        [Id("79ec572e-b4a2-4a33-90c3-65c9f9e4012c")]
        [AssociationId("2a477a7f-bc36-437c-97df-dfca39236eb5")]
        [RoleId("2e213178-fe72-4258-a8f5-ff926f8e5591")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        Party Receiver { get; set; }

        #region Allors
        [Id("8797fd5b-0d89-420f-b656-aff35b50e75c")]
        [AssociationId("42e2cb18-3596-443c-876c-3e557189ef2a")]
        [RoleId("7c820d65-87d3-4be3-be2e-8fa6a8b13a97")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        TelecommunicationsNumber OutgoingFaxNumber { get; set; }
    }
}