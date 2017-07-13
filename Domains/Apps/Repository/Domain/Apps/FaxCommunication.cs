namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("1e67320b-9680-4477-bf1b-70ccd24ab758")]
    #endregion
    public partial class FaxCommunication : CommunicationEvent 
    {
        #region inherited properties

        public SecurityToken OwnerSecurityToken { get; set; }

        public AccessControl OwnerAccessControl { get; set; }

        public DateTime ScheduledStart { get; set; }

        public Party[] ToParties { get; set; }

        public ContactMechanism[] ToContactMechanisms { get; set; }

        public CommunicationEventStatus[] CommunicationEventStatuses { get; set; }

        public Party[] InvolvedParties { get; set; }

        public DateTime InitialScheduledStart { get; set; }

        public CommunicationEventObjectState CurrentObjectState { get; set; }

        public CommunicationEventPurpose[] EventPurposes { get; set; }

        public DateTime ScheduledEnd { get; set; }

        public DateTime ActualEnd { get; set; }

        public WorkEffort[] WorkEfforts { get; set; }

        public string Description { get; set; }

        public DateTime InitialScheduledEnd { get; set; }

        public Party[] FromParties { get; set; }

        public string Subject { get; set; }

        public Media[] Documents { get; set; }

        public Case Case { get; set; }

        public Priority Priority { get; set; }

        public ContactMechanism[] FromContactMechanisms { get; set; }

        public Person Owner { get; set; }

        public CommunicationEventStatus CurrentCommunicationEventStatus { get; set; }

        public string Note { get; set; }

        public DateTime ActualStart { get; set; }

        public ObjectState PreviousObjectState { get; set; }

        public ObjectState LastObjectState { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public string Comment { get; set; }

        public Guid UniqueId { get; set; }

        public User CreatedBy { get; set; }

        public User LastModifiedBy { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastModifiedDate { get; set; }

        #endregion

        #region Allors
        [Id("3c4bea84-e00e-4ab3-8d40-5de7f394e835")]
        [AssociationId("30a33d23-6c06-45cc-8cef-25a2d02cfc5f")]
        [RoleId("c3ad4d30-c9ef-41da-b7de-f71c625b8549")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public Party Originator { get; set; }

        #region Allors
        [Id("79ec572e-b4a2-4a33-90c3-65c9f9e4012c")]
        [AssociationId("2a477a7f-bc36-437c-97df-dfca39236eb5")]
        [RoleId("2e213178-fe72-4258-a8f5-ff926f8e5591")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public Party Receiver { get; set; }

        #region Allors
        [Id("8797fd5b-0d89-420f-b656-aff35b50e75c")]
        [AssociationId("42e2cb18-3596-443c-876c-3e557189ef2a")]
        [RoleId("7c820d65-87d3-4be3-be2e-8fa6a8b13a97")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public TelecommunicationsNumber OutgoingFaxNumber { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}


        public void Cancel(){}

        public void Close(){}

        public void Reopen(){}




        public void Delete(){}


        #endregion

    }
}