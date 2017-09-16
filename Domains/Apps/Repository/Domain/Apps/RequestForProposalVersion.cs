namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("6E0ADC71-1246-446E-B3E3-E03F590BEDA6")]
    #endregion
    public partial class RequestForProposalVersion : IRequestForProposal
    {
        #region inherited properties
        public string InternalComment { get; set; }
        public string Description { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime RequiredResponseDate { get; set; }
        public RequestItem[] RequestItems { get; set; }
        public string RequestNumber { get; set; }
        public RespondingParty[] RespondingParties { get; set; }
        public Party Originator { get; set; }
        public Currency Currency { get; set; }
        public RequestObjectState CurrentObjectState { get; set; }
        public ContactMechanism FullfillContactMechanism { get; set; }
        public string EmailAddress { get; set; }
        public string TelephoneNumber { get; set; }
        public string TelephoneCountryCode { get; set; }
        public Permission[] DeniedPermissions { get; set; }
        public SecurityToken[] SecurityTokens { get; set; }
        public ObjectState PreviousObjectState { get; set; }
        public ObjectState LastObjectState { get; set; }
       #endregion

        #region Allors
        [Id("BF21E9CC-5CEA-4733-9D9B-E16D50A275B1")]
        [AssociationId("82D76B81-3E83-4E18-9B6F-6ABCF311698E")]
        [RoleId("F80811D6-2E20-4740-950C-A4B78A5F3256")]
        #endregion
        [Workspace]
        public DateTime TimeStamp { get; set; }

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }
        #endregion
    }
}