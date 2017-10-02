namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("ecf2996a-7f8b-45d5-afac-56c88c62136a")]
    #endregion
    public partial class WebSiteCommunication : CommunicationEvent, Versioned
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

        public Party[] ToParties { get; set; }

        public ContactMechanism[] ContactMechanisms { get; set; }

        public Party[] InvolvedParties { get; set; }

        public DateTime InitialScheduledStart { get; set; }


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

        public Person Owner { get; set; }

        public string Note { get; set; }

        public DateTime ActualStart { get; set; }
        public bool SendNotification { get; set; }
        public bool SendReminder { get; set; }
        public DateTime RemindAt { get; set; }


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
        [Id("18faf993-316a-4990-8ffd-8bda40f61164")]
        [AssociationId("b6c8df26-71f6-49a8-86d0-f38b9717fdc4")]
        [RoleId("96f92902-be8e-41f8-893a-afe4e93ef6d5")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Party Originator { get; set; }

        #region Allors
        [Id("39077571-13b2-4cc4-be85-517dbc11703e")]
        [AssociationId("be25f23d-6c17-4940-abe6-b6936244bcea")]
        [RoleId("f956749a-b0b3-45a4-a4b8-b0bf913d24c2")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Party Receiver { get; set; }
        
        #region Versioning
        #region Allors
        [Id("B75445E9-5742-4463-B0B2-8A140F22A0B3")]
        [AssociationId("013CFA6C-2A85-497E-86FC-EF6F774F25CA")]
        [RoleId("BAA86925-BB14-41A2-AC97-7A5A6F97CAD0")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public WebSiteCommunicationVersion CurrentVersion { get; set; }

        #region Allors
        [Id("F790CEE1-05BB-44DF-9869-3F497DFF267D")]
        [AssociationId("39B69443-95CD-46B7-BEFF-3082E31018C3")]
        [RoleId("01A4FA37-B67E-422B-96C9-A4DE0541F241")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public WebSiteCommunicationVersion[] AllVersions { get; set; }
        #endregion

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