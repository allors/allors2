namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("1e67320b-9680-4477-bf1b-70ccd24ab758")]
    #endregion
    public partial class FaxCommunication : CommunicationEvent, IFaxCommunication 
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
        public TelecommunicationsNumber OutgoingFaxNumber { get; set; }

        #endregion

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
        [Id("9B80D9EB-DBE6-4E65-9DDF-B7C546572146")]
        [AssociationId("A145B201-D4B1-4763-A9A1-A249A38E6B1D")]
        [RoleId("1961EC21-F53F-4296-B950-A6A987E2F25A")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public FaxCommunicationVersion PreviousVersion { get; set; }

        #region Allors
        [Id("E4B068A0-15ED-422A-AE2F-EAA6BC975618")]
        [AssociationId("91FC34B8-3628-4EA7-9D0D-D79267570A7F")]
        [RoleId("404A7C4B-BA4D-4DEE-ABDB-D98C45D1B74A")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public FaxCommunicationVersion[] AllVersions { get; set; }

        #region Allors
        [Id("D0299189-FE47-49AD-B244-32DD04C32305")]
        [AssociationId("204C4CC4-2CE4-482D-9F42-D58B9E4999BD")]
        [RoleId("44DB4ACA-C917-4EB5-A8D1-9DDEE35545C7")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public FaxCommunicationVersion CurrentStateVersion { get; set; }

        #region Allors
        [Id("43307B17-C559-485F-9EC8-51DB20B38B27")]
        [AssociationId("3F30A7EC-3F99-40DF-BE72-83EA303D5A72")]
        [RoleId("6C0E4FD2-AF96-4887-B0C1-E055522E4586")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public FaxCommunicationVersion[] AllStateVersions { get; set; }

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