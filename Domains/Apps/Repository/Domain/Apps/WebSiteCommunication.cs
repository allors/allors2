namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("ecf2996a-7f8b-45d5-afac-56c88c62136a")]
    #endregion
    public partial class WebSiteCommunication : CommunicationEvent, IWebSiteCommunication 
    {
        #region inherited properties

        public SecurityToken OwnerSecurityToken { get; set; }

        public AccessControl OwnerAccessControl { get; set; }

        public DateTime ScheduledStart { get; set; }

        public Party[] ToParties { get; set; }

        public ContactMechanism[] ContactMechanisms { get; set; }

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

        public Party Originator { get; set; }
        public Party Receiver { get; set; }

        #endregion

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
        [Id("9AC388B3-E4A0-4E40-A598-A07E37EF9AF2")]
        [AssociationId("D0E2EC86-D999-4B00-9CB7-76A8FE584DC8")]
        [RoleId("002DDE7A-B2C5-4A90-80B2-33096B2C52E2")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public WebSiteCommunicationVersion PreviousVersion { get; set; }

        #region Allors
        [Id("F790CEE1-05BB-44DF-9869-3F497DFF267D")]
        [AssociationId("39B69443-95CD-46B7-BEFF-3082E31018C3")]
        [RoleId("01A4FA37-B67E-422B-96C9-A4DE0541F241")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public WebSiteCommunicationVersion[] AllVersions { get; set; }

        #region Allors
        [Id("E0FDB8D4-E6A7-450E-8E39-CC310F6DCD25")]
        [AssociationId("7CD76B5E-0EFE-446D-BB5C-214248FE0174")]
        [RoleId("76ADC3A6-F1F5-4A2F-80F1-BEF7DC8204D8")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public WebSiteCommunicationVersion CurrentStateVersion { get; set; }

        #region Allors
        [Id("04319056-BA62-40E9-80E9-2DF272E37FD6")]
        [AssociationId("0F48D1D2-8856-4678-9CCA-CA8BB7A84024")]
        [RoleId("84BB3445-0AF9-43DF-B1F0-7C7F52D2209C")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public WebSiteCommunicationVersion[] AllStateVersions { get; set; }

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