namespace Allors.Repository
{
  using System;

  using Attributes;

  #region Allors
  [Id("cd66a79f-c4b8-4c33-b6ec-1928809b6b88")]
  #endregion
  public partial class Transfer : Shipment, Versioned
  {
    #region inherited properties

    public Permission[] DeniedPermissions { get; set; }

    public SecurityToken[] SecurityTokens { get; set; }

    public string HtmlContent { get; set; }

      public Media PdfContent { get; set; }

      public string Comment { get; set; }

    public User CreatedBy { get; set; }

    public User LastModifiedBy { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime LastModifiedDate { get; set; }

    public ObjectState[] PreviousObjectStates { get; set; }

    public ObjectState[] LastObjectStates { get; set; }

    public ObjectState[] ObjectStates { get; set; }

    public ShipmentMethod ShipmentMethod { get; set; }

    public ContactMechanism BillToContactMechanism { get; set; }

    public ShipmentPackage[] ShipmentPackages { get; set; }

    public string ShipmentNumber { get; set; }

    public Document[] Documents { get; set; }

    public Party BillToParty { get; set; }

    public Party ShipToParty { get; set; }

    public ShipmentItem[] ShipmentItems { get; set; }

    public ContactMechanism ReceiverContactMechanism { get; set; }

    public PostalAddress ShipToAddress { get; set; }

    public decimal EstimatedShipCost { get; set; }

    public DateTime EstimatedShipDate { get; set; }

    public DateTime LatestCancelDate { get; set; }

    public Carrier Carrier { get; set; }

    public ContactMechanism InquireAboutContactMechanism { get; set; }

    public DateTime EstimatedReadyDate { get; set; }

    public PostalAddress ShipFromAddress { get; set; }

    public ContactMechanism BillFromContactMechanism { get; set; }

    public string HandlingInstruction { get; set; }

    public Store Store { get; set; }

    public Party ShipFromParty { get; set; }

    public ShipmentRouteSegment[] ShipmentRouteSegments { get; set; }

    public DateTime EstimatedArrivalDate { get; set; }

    #endregion

    #region ObjectStates
    #region TransferState
    #region Allors
    [Id("FF31947B-692C-42D1-9568-A59AA43BFA9D")]
    [AssociationId("6F6175AE-6C8B-4D35-8C95-E1F612EA3BED")]
    [RoleId("9161D787-2F5A-4806-A16E-AD74D840A954")]
    [Indexed]
    #endregion
    [Multiplicity(Multiplicity.ManyToOne)]
    [Derived]
    public TransferState PreviousTransferState { get; set; }

    #region Allors
    [Id("D079CC6D-2801-4480-99CA-371AC8ED302A")]
    [AssociationId("6905FB0C-FB77-4C9F-B11E-C0FCD8C3D170")]
    [RoleId("CB48FED5-9EAB-46C8-83B5-FC4E3EE579D8")]
    [Indexed]
    #endregion
    [Multiplicity(Multiplicity.ManyToOne)]
    [Derived]
    public TransferState LastTransferState { get; set; }

    #region Allors
    [Id("C1BDF196-7228-4C89-848E-2AD123FB771D")]
    [AssociationId("ABCBB9E3-3D3D-4DBB-B7A8-ADD8D7E2769C")]
    [RoleId("33082E9D-01A2-4B54-AC0B-3790E1405BD0")]
    [Indexed]
    #endregion
    [Multiplicity(Multiplicity.ManyToOne)]
    [Workspace]
    public TransferState TransferState { get; set; }
    #endregion
    #endregion

    #region Versioning
    #region Allors
    [Id("2654DADF-DA68-48A7-86B2-F2202E813B22")]
    [AssociationId("551AEDC0-1DF1-4AFE-A8CA-24C45E204C36")]
    [RoleId("82C2CC30-09B2-4E02-95B1-2AD3F60F93A7")]
    [Indexed]
    #endregion
    [Multiplicity(Multiplicity.OneToOne)]
    [Workspace]
    public TransferVersion CurrentVersion { get; set; }

    #region Allors
    [Id("10E49FE1-25CA-4DF7-8B26-B664826F37B3")]
    [AssociationId("FB3D2BAE-D97A-4D2E-8605-C139E6B7CA4C")]
    [RoleId("882B3978-35BE-4376-AC24-A7C5EE4A7C61")]
    [Indexed]
    #endregion
    [Multiplicity(Multiplicity.OneToMany)]
    [Workspace]
    public TransferVersion[] AllVersions { get; set; }
    #endregion

    #region inherited methods


    public void OnBuild() { }

    public void OnPostBuild() { }

    public void OnPreDerive() { }

    public void OnDerive() { }

    public void OnPostDerive() { }





    #endregion
  }
}