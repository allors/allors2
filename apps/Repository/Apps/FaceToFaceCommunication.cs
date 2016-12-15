namespace Allors.Repository.Domain
{
    using System;

    #region Allors
    [Id("d0f9fc0d-a3c5-46cc-ab00-4c724995fc14")]
    #endregion
    public partial class FaceToFaceCommunication : CommunicationEvent 
    {
        #region inherited properties
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

        #endregion

        #region Allors
        [Id("52b8614b-799e-4aea-a012-ea8dbc23f8dd")]
        [AssociationId("ac424847-d426-4614-99a2-37c70841c454")]
        [RoleId("bcf4a8df-8b57-4b3c-a6e5-f9b56c71a13b")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Required]

        public Party[] Participants { get; set; }
        #region Allors
        [Id("95ae979f-d549-4ea1-87f0-46aa55e4b14a")]
        [AssociationId("d34e4203-0bd2-4fe4-a2ef-9f9f52b49cf9")]
        [RoleId("9f67b296-953d-4e04-b94d-6ffece87ceef")]
        #endregion
        [Size(256)]

        public string Location { get; set; }


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