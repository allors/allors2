namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("05964e28-2c1d-4837-a887-2255f157e889")]
    #endregion
    public partial class LetterCorrespondence : CommunicationEvent 
    {
        #region inherited properties

        public SecurityToken OwnerSecurityToken { get; set; }

        public AccessControl OwnerAccessControl { get; set; }

        public DateTime ScheduledStart { get; set; }

        public Party[] ToParties { get; set; }

        public ContactMechanism[] ContactMechanisms { get; set; }

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

        public Person Owner { get; set; }

        public CommunicationEventStatus CurrentCommunicationEventStatus { get; set; }

        public string Note { get; set; }

        public DateTime ActualStart { get; set; }
        public bool SendNotification { get; set; }
        public bool SendReminder { get; set; }
        public DateTime RemindAt { get; set; }

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
        [Id("3e0f1be5-0685-48d6-922f-6e971110b414")]
        [AssociationId("d063c86e-bbee-41b9-9823-10e96c69c5a0")]
        [RoleId("14ca37a9-7ce4-4d2a-b7ba-1a43bccc1664")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        public PostalAddress[] PostalAddresses { get; set; }

        #region Allors
        [Id("e8fd2c39-bcb7-4914-8cd3-6dcc6a7a9997")]
        [AssociationId("d5ed6948-f657-4d47-89c8-d860e2971138")]
        [RoleId("b65552b5-99c7-4b91-b9b6-a70ec35c3ae2")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Required]
        [Workspace]
        public Party[] Originators { get; set; }

        #region Allors
        [Id("ece02647-000a-4373-8f01-f4b7d1c75dd5")]
        [AssociationId("e580ed8f-a7a4-40c3-9c0a-4cdbe95354a6")]
        [RoleId("dde368dc-c198-4744-b3b2-1a2e0d2976e4")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Required]
        [Workspace]
        public Party[] Receivers { get; set; }

        #region Allors
        [Id("87B8680D-4161-420B-82B9-3A4E08A572B3")]
        [AssociationId("9F127485-9CDE-41A2-8B71-36F8BD1652EE")]
        [RoleId("22158DC4-A38D-4D48-AA48-19FA6B35B6AC")]
        #endregion
        [Required]
        [Workspace]
        public bool IncomingLetter { get; set; }


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