// <copyright file="RequestForInformation.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("eab85f26-c3f4-4f47-97dc-8f9429856c00")]
    #endregion
    [Plural("RequestsForInformation")]
    public partial class RequestForInformation : Request, Versioned
    {
        #region inherited properties

        public ObjectState[] PreviousObjectStates { get; set; }

        public ObjectState[] LastObjectStates { get; set; }

        public ObjectState[] ObjectStates { get; set; }

        public RequestState PreviousRequestState { get; set; }

        public RequestState LastRequestState { get; set; }

        public RequestState RequestState { get; set; }

        public InternalOrganisation Recipient { get; set; }

        public string InternalComment { get; set; }

        public string Description { get; set; }

        public DateTime RequestDate { get; set; }

        public DateTime RequiredResponseDate { get; set; }

        public RequestItem[] RequestItems { get; set; }

        public string RequestNumber { get; set; }

        public RespondingParty[] RespondingParties { get; set; }

        public Party Originator { get; set; }

        public Currency Currency { get; set; }

        public ContactMechanism FullfillContactMechanism { get; set; }

        public string EmailAddress { get; set; }

        public string TelephoneNumber { get; set; }

        public string TelephoneCountryCode { get; set; }

        public Person ContactPerson { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public string Comment { get; set; }

        public LocalisedText[] LocalisedComments { get; set; }

        public User CreatedBy { get; set; }

        public User LastModifiedBy { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastModifiedDate { get; set; }

        public PrintDocument PrintDocument { get; set; }

        #endregion

        #region Versioning
        #region Allors
        [Id("07D23C22-963B-485D-8E5C-F7962AE050A8")]
        [AssociationId("148E8D4D-FB66-45AA-A996-BF286E247B38")]
        [RoleId("B8632272-190A-43A0-8B6D-912A4524C82D")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public RequestForInformationVersion CurrentVersion { get; set; }

        #region Allors
        [Id("1E80AB51-8E49-4655-A982-C78B6ABEE202")]
        [AssociationId("8C62A675-C1E8-4788-BE3C-2A01FB465E24")]
        [RoleId("C8D212BF-3770-46C2-9BBA-BC081457AAB8")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public RequestForInformationVersion[] AllVersions { get; set; }
        #endregion

        #region inherited methods
        public void Cancel() { }

        public void Reject() { }

        public void Submit() { }

        public void Hold() { }

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnInit()
        {
        }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        public void Print() { }

        public void CreateQuote() { }
        #endregion
    }
}
