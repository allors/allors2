// <copyright file="LetterCorrespondence.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;
    using Allors.Repository.Attributes;

    #region Allors
    [Id("05964e28-2c1d-4837-a887-2255f157e889")]
    #endregion
    public partial class LetterCorrespondence : CommunicationEvent, Versioned
    {
        #region inherited properties
        public CommunicationEventState PreviousCommunicationEventState { get; set; }

        public CommunicationEventState LastCommunicationEventState { get; set; }

        public CommunicationEventState CommunicationEventState { get; set; }

        public ObjectState[] PreviousObjectStates { get; set; }

        public ObjectState[] LastObjectStates { get; set; }

        public ObjectState[] ObjectStates { get; set; }

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

        public Restriction[] Restrictions { get; set; }

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
        [Id("3e0f1be5-0685-48d6-922f-6e971110b414")]
        [AssociationId("d063c86e-bbee-41b9-9823-10e96c69c5a0")]
        [RoleId("14ca37a9-7ce4-4d2a-b7ba-1a43bccc1664")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public PostalAddress PostalAddress { get; set; }

        #region Versioning
        #region Allors
        [Id("9A42BAF7-D2CA-46D6-95E2-28198DA719FA")]
        [AssociationId("B168D640-F4B7-4142-9A0B-77FC405C9512")]
        [RoleId("0700BA1C-CB54-4D1C-8B00-8DE505E503C0")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public LetterCorrespondenceVersion CurrentVersion { get; set; }

        #region Allors
        [Id("1BD17B22-88CB-4F44-8B27-8EB8BCD9C963")]
        [AssociationId("FD6F1CE4-D23F-4B99-9CF8-71C3A7D50599")]
        [RoleId("3ED25C01-401F-42D4-ABAA-C24EA95E0BAA")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public LetterCorrespondenceVersion[] AllVersions { get; set; }
        #endregion

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnInit()
        {
        }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        public void Cancel() { }

        public void Close() { }

        public void Reopen() { }

        public void Delete() { }

        #endregion

        public string WorkItemDescription { get; set; }
    }
}
