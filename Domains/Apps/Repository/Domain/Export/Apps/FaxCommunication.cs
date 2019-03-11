namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("1e67320b-9680-4477-bf1b-70ccd24ab758")]
    #endregion
    public partial class FaxCommunication : CommunicationEvent, Versioned
    {
        #region inherited properties

        public CommunicationEventState PreviousCommunicationEventState { get; set; }

        public CommunicationEventState LastCommunicationEventState { get; set; }

        public CommunicationEventState CommunicationEventState { get; set; }

        public ObjectState[] PreviousObjectStates { get; set; }

        public ObjectState[] LastObjectStates { get; set; }

        public ObjectState[] ObjectStates { get; set; }

        public SecurityToken OwnerSecurityToken { get; set; }

        public AccessControl OwnerAccessControl { get; set; }

        public DateTime ScheduledStart { get; set; }

        public Party FromParty { get; set; }

        public Party ToParty { get; set; }

        public ContactMechanism[] ContactMechanisms { get; set; }

        public Party[] InvolvedParties { get; set; }

        public DateTime InitialScheduledStart { get; set; }

        public CommunicationEventPurpose[] EventPurposes { get; set; }

        public DateTime ScheduledEnd { get; set; }

        public DateTime ActualEnd { get; set; }

        public WorkEffort[] WorkEfforts { get; set; }

        public string Description { get; set; }

        public DateTime InitialScheduledEnd { get; set; }

        public string Subject { get; set; }

        public Media[] Documents { get; set; }

        public Case Case { get; set; }

        public Priority Priority { get; set; }

        public Person Owner { get; set; }

        public DateTime ActualStart { get; set; }
        public bool SendNotification { get; set; }
        public bool SendReminder { get; set; }
        public DateTime RemindAt { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public string Comment { get; set; }

        public LocalisedText[] LocalisedComments { get; set; }

        public Guid UniqueId { get; set; }

        public User CreatedBy { get; set; }

        public User LastModifiedBy { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastModifiedDate { get; set; }
        #endregion

        #region Allors
        [Id("8797fd5b-0d89-420f-b656-aff35b50e75c")]
        [AssociationId("42e2cb18-3596-443c-876c-3e557189ef2a")]
        [RoleId("7c820d65-87d3-4be3-be2e-8fa6a8b13a97")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public TelecommunicationsNumber FaxNumber { get; set; }

        #region Versioning
        #region Allors
        [Id("D535262B-CD3F-4440-AC29-6211B3036A49")]
        [AssociationId("12003C1A-B152-4787-B58E-3C955EAC80EC")]
        [RoleId("5E3720C8-0720-49D0-BF49-0045C6C7376E")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public FaxCommunicationVersion CurrentVersion { get; set; }

        #region Allors
        [Id("E4B068A0-15ED-422A-AE2F-EAA6BC975618")]
        [AssociationId("91FC34B8-3628-4EA7-9D0D-D79267570A7F")]
        [RoleId("404A7C4B-BA4D-4DEE-ABDB-D98C45D1B74A")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public FaxCommunicationVersion[] AllVersions { get; set; }
        #endregion

        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnInit()
        {
            
        }

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